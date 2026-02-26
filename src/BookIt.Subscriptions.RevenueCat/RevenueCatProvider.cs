using System.Net.Http.Headers;
using System.Text.Json;
using BookIt.Core.Enums;
using Microsoft.Extensions.Logging;

namespace BookIt.Subscriptions.RevenueCat;

/// <summary>
/// RevenueCat REST API provider. Keeps all RevenueCat SDK references isolated
/// from the rest of the application so the library is independently reusable.
/// </summary>
public sealed class RevenueCatProvider : IRevenueCatProvider
{
    private const string BaseUrl = "https://api.revenuecat.com";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RevenueCatProvider> _logger;

    public RevenueCatProvider(IHttpClientFactory httpClientFactory, ILogger<RevenueCatProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<RevenueCatCustomer> GetOrCreateCustomerAsync(
        string apiKey,
        string appUserId,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient(apiKey);
        var response = await client.GetAsync($"/v1/subscribers/{Uri.EscapeDataString(appUserId)}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonSerializer.Deserialize<JsonElement>(json);

        var subscriber = doc.GetProperty("subscriber");
        var plan = ParseEntitlements(subscriber, "premium");
        DateTime? expiresAt = null;

        if (subscriber.TryGetProperty("entitlements", out var entitlements) &&
            entitlements.ValueKind == JsonValueKind.Object)
        {
            foreach (var entitlement in entitlements.EnumerateObject())
            {
                if (entitlement.Value.TryGetProperty("expires_date", out var expProp) &&
                    expProp.ValueKind != JsonValueKind.Null)
                {
                    if (DateTime.TryParse(expProp.GetString(), out var exp))
                        expiresAt = exp;
                }
            }
        }

        _logger.LogInformation("RevenueCat customer {AppUserId}: plan={Plan}", appUserId, plan);

        return new RevenueCatCustomer(appUserId, plan, plan > SubscriptionPlan.Free, expiresAt);
    }

    /// <inheritdoc />
    public async Task<SubscriptionPlan> GetEntitlementPlanAsync(
        string apiKey,
        string appUserId,
        string entitlementId = "premium",
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient(apiKey);
        var response = await client.GetAsync($"/v1/subscribers/{Uri.EscapeDataString(appUserId)}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("RevenueCat lookup failed for {AppUserId}: {Status}", appUserId, response.StatusCode);
            return SubscriptionPlan.Free;
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        var subscriber = doc.GetProperty("subscriber");

        return ParseEntitlements(subscriber, entitlementId);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<RevenueCatTier>> GetOfferingsAsync(
        string apiKey,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient(apiKey);
        var response = await client.GetAsync("/v1/offerings", cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonSerializer.Deserialize<JsonElement>(json);

        var tiers = new List<RevenueCatTier>();

        if (!doc.TryGetProperty("offerings", out var offerings) ||
            offerings.ValueKind != JsonValueKind.Array)
            return tiers;

        foreach (var offering in offerings.EnumerateArray())
        {
            if (!offering.TryGetProperty("packages", out var packages)) continue;
            foreach (var pkg in packages.EnumerateArray())
            {
                var planName = pkg.TryGetProperty("identifier", out var id) ? id.GetString() ?? "" : "";
                var plan = ParsePlanName(planName);
                var monthlyId = pkg.TryGetProperty("monthly_product_identifier", out var mProp)
                    ? mProp.GetString() ?? "" : "";
                var annualId = pkg.TryGetProperty("annual_product_identifier", out var aProp)
                    ? aProp.GetString() ?? "" : "";
                var price = pkg.TryGetProperty("price", out var priceProp)
                    ? priceProp.GetDecimal() : 0m;

                tiers.Add(new RevenueCatTier(plan, planName, monthlyId, annualId, price, price * 10m, "GBP"));
            }
        }

        return tiers;
    }

    private HttpClient CreateClient(string apiKey)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseUrl);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Add("X-Platform", "web");
        return client;
    }

    private static SubscriptionPlan ParseEntitlements(JsonElement subscriber, string entitlementId)
    {
        if (!subscriber.TryGetProperty("entitlements", out var entitlements) ||
            entitlements.ValueKind != JsonValueKind.Object)
            return SubscriptionPlan.Free;

        // Check each entitlement for an active, non-expired grant
        foreach (var entitlement in entitlements.EnumerateObject())
        {
            if (!entitlement.Value.TryGetProperty("expires_date", out var exp)) continue;
            if (exp.ValueKind == JsonValueKind.Null) return SubscriptionPlan.Enterprise; // lifetime

            if (DateTime.TryParse(exp.GetString(), out var expiresAt) && expiresAt > DateTime.UtcNow)
            {
                // Map entitlement identifier to plan
                return ParsePlanName(entitlement.Name);
            }
        }

        return SubscriptionPlan.Free;
    }

    private static SubscriptionPlan ParsePlanName(string name) =>
        name.ToLowerInvariant() switch
        {
            var n when n.Contains("enterprise") => SubscriptionPlan.Enterprise,
            var n when n.Contains("pro") => SubscriptionPlan.Pro,
            var n when n.Contains("starter") => SubscriptionPlan.Starter,
            _ => SubscriptionPlan.Free
        };
}
