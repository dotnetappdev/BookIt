namespace BookIt.Core.Interfaces;

public interface ITenantService
{
    Guid? GetCurrentTenantId();
    string? GetCurrentTenantSlug();
    bool IsValidTenantAccess(Guid resourceTenantId);
}
