namespace BookIt.Core.Entities;

public class StaffService
{
    public Guid StaffId { get; set; }
    public Staff Staff { get; set; } = null!;
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;
}
