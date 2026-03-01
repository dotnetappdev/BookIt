using BookIt.Core.Helpers;

namespace BookIt.Blazor.Middleware;

/// <summary>
/// Rewrites incoming paths for tenant subdomain requests so that the Blazor router
/// matches existing <c>/{tenantSlug}/...</c> page routes without any changes to
/// individual page components.
///
/// Example: a request for <c>demo-barber.bookit.com/admin/calendar</c> is internally
/// rewritten to <c>/demo-barber/admin/calendar</c> before the Blazor router runs.
///
/// Also stores the slug in <c>HttpContext.Items["TenantSlug"]</c> so the login page
/// can pre-populate the slug field.
/// </summary>
public class SubdomainRewriteMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string? _baseDomain;

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
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var slug = SubdomainHelper.ExtractTenantSlug(context.Request.Host.Value, _baseDomain);

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
}
