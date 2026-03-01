using BookIt.Core.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace BookIt.Web.Middleware;

/// <summary>
/// Detects a tenant subdomain in the incoming request host, validates it against the
/// database via the BookIt API, and stores the resolved tenant slug in
/// <c>HttpContext.Items["TenantSlug"]</c> so that controllers and filters can pick it
/// up when no slug is present in the URL path.
///
/// Results are cached for <see cref="CacheTtl"/> to avoid an API round-trip on every
/// request.  Cache entries for unknown subdomains are stored as negative results so
/// that invalid subdomains also benefit from caching.
/// </summary>
public class SubdomainTenantMiddleware
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);
    private const string CachePrefix = "subdomain:";

    private readonly RequestDelegate _next;
    private readonly string? _baseDomain;
    private readonly string _apiBaseUrl;

    public SubdomainTenantMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _baseDomain = configuration["SubdomainConfig:BaseDomain"];
        _apiBaseUrl = (configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5248").TrimEnd('/');
    }

    public async Task InvokeAsync(HttpContext context, IMemoryCache cache, IHttpClientFactory httpClientFactory)
    {
        var candidateSubdomain = SubdomainHelper.ExtractTenantSlug(
            context.Request.Host.Value, _baseDomain);

        if (!string.IsNullOrEmpty(candidateSubdomain))
        {
            var cacheKey = CachePrefix + candidateSubdomain;

            if (!cache.TryGetValue(cacheKey, out string? resolvedSlug))
            {
                resolvedSlug = await ResolveSlugFromApiAsync(httpClientFactory, candidateSubdomain);
                cache.Set(cacheKey, resolvedSlug, CacheTtl);
            }

            if (!string.IsNullOrEmpty(resolvedSlug))
                context.Items["TenantSlug"] = resolvedSlug;
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
            if (doc.RootElement.TryGetProperty("slug", out var slug))
                return slug.GetString();
        }
        catch
        {
            // If the API is unavailable, fall back gracefully (no tenant resolution)
        }
        return null;
    }
}
