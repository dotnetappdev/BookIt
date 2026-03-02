namespace BookIt.Core.Entities;

public class PropertyAmenity
{
    public Guid LodgingPropertyId { get; set; }
    public LodgingProperty LodgingProperty { get; set; } = null!;
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
