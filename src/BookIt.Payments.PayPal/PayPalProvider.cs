using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BookIt.Payments.PayPal;

/// <summary>
/// PayPal Orders v2 provider. Keeps all PayPal HTTP API calls isolated from the
/// rest of the application.
/// </summary>
public sealed class PayPalProvider : IPayPalProvider
{
    private const string LiveBaseUrl = "https://api-m.paypal.com";
    private const string SandboxBaseUrl = "https://api-m.sandbox.paypal.com";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PayPalProvider> _logger;

    public PayPalProvider(IHttpClientFactory httpClientFactory, ILogger<PayPalProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<string> CreateOrderAsync(
        string clientId,
        string clientSecret,
        decimal amount,
        string currency,
        string referenceId,
        string description,
        bool useSandbox = false)
    {
        var baseUrl = useSandbox ? SandboxBaseUrl : LiveBaseUrl;
        var token = await GetAccessTokenAsync(clientId, clientSecret, baseUrl);

        var client = CreateClient(baseUrl, token);

        var payload = new
        {
            intent = "CAPTURE",
            purchase_units = new[]
            {
                new
                {
                    reference_id = referenceId,
                    description,
                    amount = new
                    {
                        currency_code = currency.ToUpperInvariant(),
                        value = amount.ToString("F2")
                    }
                }
            }
        };

        var response = await client.PostAsync("/v2/checkout/orders",
            new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        var orderId = doc.GetProperty("id").GetString()!;

        _logger.LogInformation("Created PayPal Order {OrderId}", orderId);
        return orderId;
    }

    public async Task<bool> CaptureOrderAsync(
        string clientId,
        string clientSecret,
        string orderId,
        bool useSandbox = false)
    {
        var baseUrl = useSandbox ? SandboxBaseUrl : LiveBaseUrl;
        var token = await GetAccessTokenAsync(clientId, clientSecret, baseUrl);

        var client = CreateClient(baseUrl, token);

        var response = await client.PostAsync($"/v2/checkout/orders/{orderId}/capture",
            new StringContent("{}", Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("PayPal capture failed for Order {OrderId}: {Status}", orderId, response.StatusCode);
            return false;
        }

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        var status = doc.GetProperty("status").GetString();

        _logger.LogInformation("Captured PayPal Order {OrderId}, status={Status}", orderId, status);
        return status == "COMPLETED";
    }

    private HttpClient CreateClient(string baseUrl, string accessToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }

    private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string baseUrl)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(baseUrl);
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        var response = await client.PostAsync("/v1/oauth2/token",
            new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        return doc.GetProperty("access_token").GetString()!;
    }
}
