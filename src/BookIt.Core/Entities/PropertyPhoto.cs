namespace BookIt.Core.Entities;

public class PropertyPhoto : BaseEntity
{
    public Guid LodgingPropertyId { get; set; }
    public LodgingProperty LodgingProperty { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; } = false;
}
