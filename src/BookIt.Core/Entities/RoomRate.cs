namespace BookIt.Core.Entities;

public class RoomRate : BaseEntity
{
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Rate { get; set; }
    public string? Label { get; set; }
}
