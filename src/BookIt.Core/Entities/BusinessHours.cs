using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class BusinessHours : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public DayOfWeekFlag DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public bool IsClosed { get; set; } = false;
    public int SlotDurationMinutes { get; set; } = 60;
}
