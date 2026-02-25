namespace BookIt.Infrastructure.Data;

public interface ITenantContext
{
    Guid? TenantId { get; }
}
