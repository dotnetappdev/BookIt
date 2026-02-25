using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BookIt.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentTenantId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id");
        if (claim != null && Guid.TryParse(claim.Value, out var tenantId))
            return tenantId;
        return null;
    }

    public string? GetCurrentTenantSlug()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_slug")?.Value;
    }

    public bool IsValidTenantAccess(Guid resourceTenantId)
    {
        // Super admins can access any tenant
        var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("role")?.Value;
        if (roleClaim == ((int)UserRole.SuperAdmin).ToString()) return true; // SuperAdmin

        var currentTenantId = GetCurrentTenantId();
        return currentTenantId.HasValue && currentTenantId.Value == resourceTenantId;
    }
}
