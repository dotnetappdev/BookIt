using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace BookIt.Infrastructure.Services;

public class HttpTenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpTenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id");
            if (claim != null && Guid.TryParse(claim.Value, out var tenantId))
                return tenantId;

            // Try from route values
            var routeTenantSlug = _httpContextAccessor.HttpContext?.Request.RouteValues["tenantSlug"]?.ToString();
            if (!string.IsNullOrEmpty(routeTenantSlug))
            {
                // Store slug in context items for later lookup
                _httpContextAccessor.HttpContext!.Items["TenantSlug"] = routeTenantSlug;
            }

            return null;
        }
    }
}
