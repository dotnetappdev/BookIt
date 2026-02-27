namespace BookIt.Core.Entities;

public class RoomPhoto : BaseEntity
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; } = false;
}
