namespace BookIt.Core.Entities;

public class AppConfiguration : BaseEntity
{
    public Guid? TenantId { get; set; } // null = global settings
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public bool IsEncrypted { get; set; }
    public string? Description { get; set; }
}
