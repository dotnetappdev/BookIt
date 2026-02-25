using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class StaffAvailability : BaseEntity
{
    public Guid StaffId { get; set; }
    public Staff Staff { get; set; } = null!;
    public DayOfWeekFlag DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;
}
