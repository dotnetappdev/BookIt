using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public string? CategoryName { get; set; }
    public bool AllowOnlineBooking { get; set; }
    public MeetingType DefaultMeetingType { get; set; }
}

public class CreateServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public int BufferMinutes { get; set; } = 0;
    public Guid? CategoryId { get; set; }
    public bool AllowOnlineBooking { get; set; } = true;
    public MeetingType DefaultMeetingType { get; set; } = MeetingType.InPerson;
}
