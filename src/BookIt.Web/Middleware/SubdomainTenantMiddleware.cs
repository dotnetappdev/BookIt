using BookIt.Core.Helpers;

namespace BookIt.Web.Middleware;

/// <summary>
/// Detects a tenant subdomain in the incoming request host and stores the resolved
/// tenant slug in <c>HttpContext.Items["TenantSlug"]</c> so that controllers and
/// filters can pick it up when no slug is present in the URL path.
/// </summary>
public class SubdomainTenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string? _baseDomain;

    public SubdomainTenantMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _baseDomain = configuration["SubdomainConfig:BaseDomain"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var slug = SubdomainHelper.ExtractTenantSlug(
            context.Request.Host.Value,
            _baseDomain);

        if (!string.IsNullOrEmpty(slug))
            context.Items["TenantSlug"] = slug;

        await _next(context);
    }
}
