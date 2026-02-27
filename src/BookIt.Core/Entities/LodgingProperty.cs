namespace BookIt.Core.Entities;

public class LodgingProperty : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
