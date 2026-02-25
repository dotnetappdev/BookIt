using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class CreateAppointmentRequest
{
    public Guid TenantId { get; set; }
    public Guid ServiceId { get; set; }
    public List<Guid> ServiceIds { get; set; } = new();
    public Guid? StaffId { get; set; }
    public DateTime StartTime { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerNotes { get; set; }
    public MeetingType MeetingType { get; set; } = MeetingType.InPerson;
    public string? CustomFormDataJson { get; set; }
    public PaymentProvider? PaymentProvider { get; set; }
}

public class AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public MeetingType MeetingType { get; set; }
    public string? MeetingLink { get; set; }
    public string? ConfirmationToken { get; set; }
    public string? BookingPin { get; set; }
    public List<ServiceSummary> Services { get; set; } = new();
    public string? StaffName { get; set; }
}

public class ServiceSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
}

public class AvailableSlotsRequest
{
    public Guid TenantId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid? StaffId { get; set; }
    public DateOnly Date { get; set; }
}
