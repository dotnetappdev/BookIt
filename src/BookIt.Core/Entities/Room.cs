using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class Room : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Guid? LodgingPropertyId { get; set; }
    public LodgingProperty? LodgingProperty { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RoomType RoomType { get; set; } = RoomType.Double;
    public int Capacity { get; set; } = 2;
    public decimal BaseRate { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public ICollection<RoomPhoto> Photos { get; set; } = new List<RoomPhoto>();
    public ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
    public ICollection<RoomRate> Rates { get; set; } = new List<RoomRate>();
}
