using Microsoft.AspNetCore.Mvc.Filters;

namespace BookIt.Web.Filters;

/// <summary>
/// Global action filter that resolves the tenant slug from the subdomain (stored in
/// <c>HttpContext.Items["TenantSlug"]</c> by <see cref="Middleware.SubdomainTenantMiddleware"/>)
/// and injects it into the action argument when the route does not supply one.
/// This allows the same controller actions to serve both path-based and subdomain-based requests.
/// </summary>
public class SubdomainTenantFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var subdomainSlug = context.HttpContext.Items["TenantSlug"] as string;
        if (string.IsNullOrEmpty(subdomainSlug))
            return;

        if (!context.ActionArguments.TryGetValue("tenantSlug", out var existing)
            || string.IsNullOrEmpty(existing as string))
        {
            context.ActionArguments["tenantSlug"] = subdomainSlug;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
