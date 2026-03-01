using BookIt.Core.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace BookIt.Blazor.Middleware;

/// <summary>
/// Rewrites incoming paths for tenant subdomain requests so that the Blazor router
/// matches existing <c>/{tenantSlug}/...</c> page routes without any changes to
/// individual page components.
///
/// Example: a request for <c>demo-barber.bookit.com/admin/calendar</c> is internally
/// rewritten to <c>/demo-barber/admin/calendar</c> before the Blazor router runs.
///
/// The subdomain is validated against the <c>Tenant.Subdomain</c> column in the database
/// via the BookIt API (<c>GET /api/tenants/by-subdomain/{subdomain}</c>).  Results are
/// cached for <see cref="CacheTtl"/> to avoid an API round-trip on every request.
/// </summary>
public class SubdomainRewriteMiddleware
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);
    private const string CachePrefix = "subdomain:";

    private readonly RequestDelegate _next;
    private readonly string? _baseDomain;
    private readonly string _apiBaseUrl;

    // URL prefixes that identify tenant-owned pages and should be slug-prefixed
    private static readonly string[] TenantPaths = ["/admin", "/book"];

    // Prefixes that must never be rewritten (Blazor internals, auth, marketing pages)
    private static readonly string[] SkipPrefixes =
    [
        "/_blazor", "/_framework", "/_content", "/_vs",
        "/login", "/setup", "/pricing", "/super-admin", "/superadmin", "/staff-invite"
    ];

    public SubdomainRewriteMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _baseDomain = configuration["SubdomainConfig:BaseDomain"];
        _apiBaseUrl = (configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5248").TrimEnd('/');
    }

    public async Task InvokeAsync(HttpContext context, IMemoryCache cache, IHttpClientFactory httpClientFactory)
    {
        var candidateSubdomain = SubdomainHelper.ExtractTenantSlug(context.Request.Host.Value, _baseDomain);

        string? slug = null;
        if (!string.IsNullOrEmpty(candidateSubdomain))
        {
            var cacheKey = CachePrefix + candidateSubdomain;
            if (!cache.TryGetValue(cacheKey, out string? cachedSlug))
            {
                cachedSlug = await ResolveSlugFromApiAsync(httpClientFactory, candidateSubdomain);
                cache.Set(cacheKey, cachedSlug, CacheTtl);
            }
            slug = cachedSlug;
        }

        if (!string.IsNullOrEmpty(slug))
        {
            context.Items["TenantSlug"] = slug;

            var path = context.Request.Path.Value ?? "/";

            var alreadyPrefixed = path.StartsWith($"/{slug}/", StringComparison.OrdinalIgnoreCase)
                                  || path.Equals($"/{slug}", StringComparison.OrdinalIgnoreCase);

            var shouldSkip = SkipPrefixes.Any(p =>
                path.StartsWith(p, StringComparison.OrdinalIgnoreCase));

            var isTenantPath = TenantPaths.Any(p =>
                path.Equals(p, StringComparison.OrdinalIgnoreCase)
                || path.StartsWith(p + "/", StringComparison.OrdinalIgnoreCase));

            if (!alreadyPrefixed && !shouldSkip && isTenantPath)
                context.Request.Path = $"/{slug}{path}";
        }

        await _next(context);
    }

    private async Task<string?> ResolveSlugFromApiAsync(IHttpClientFactory factory, string subdomain)
    {
        try
        {
            using var client = factory.CreateClient();
            client.BaseAddress = new Uri(_apiBaseUrl);
            var response = await client.GetAsync($"/api/tenants/by-subdomain/{Uri.EscapeDataString(subdomain)}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("slug", out var slugProp))
                return slugProp.GetString();
        }
        catch
        {
            // If the API is unavailable, fall back gracefully (no tenant resolution)
        }
        return null;
    }
}
