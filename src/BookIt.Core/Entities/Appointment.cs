using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class Appointment : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Guid? StaffId { get; set; }
    public Staff? Staff { get; set; }
    public Guid? CustomerId { get; set; }
    public ApplicationUser? Customer { get; set; }

    // Customer details (for guest bookings)
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerNotes { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public string? CancellationReason { get; set; }
    public string? InternalNotes { get; set; }

    // Payment
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    public decimal TotalAmount { get; set; }
    public string? PaymentReference { get; set; }
    public PaymentProvider? PaymentProvider { get; set; }

    // Virtual meeting
    public MeetingType MeetingType { get; set; } = MeetingType.InPerson;
    public string? MeetingLink { get; set; }
    public string? MeetingId { get; set; }
    public string? MeetingPassword { get; set; }
    public string? MeetingInstructions { get; set; }

    // Booking form custom data
    public string? CustomFormDataJson { get; set; }

    // Confirmation
    public string? ConfirmationToken { get; set; }
    public bool ReminderSent { get; set; }
    public string? BookingPin { get; set; }  // 6-char alphanumeric, e.g. "AB3X7K"

    public ICollection<AppointmentService> Services { get; set; } = new List<AppointmentService>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
