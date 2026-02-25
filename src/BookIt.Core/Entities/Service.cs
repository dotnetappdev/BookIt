using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class Service : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Guid? CategoryId { get; set; }
    public ServiceCategory? Category { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public int BufferMinutes { get; set; } = 0; // cleanup time between appointments
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public bool AllowOnlineBooking { get; set; } = true;
    public int MaxConcurrentBookings { get; set; } = 1;
    public MeetingType DefaultMeetingType { get; set; } = MeetingType.InPerson;
    public ICollection<StaffService> StaffServices { get; set; } = new List<StaffService>();
    public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
}
