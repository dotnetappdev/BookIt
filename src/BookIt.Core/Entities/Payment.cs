using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class Payment : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";
    public PaymentProvider Provider { get; set; }
    public PaymentStatus Status { get; set; }
    public string? ProviderTransactionId { get; set; }
    public string? ProviderPaymentIntentId { get; set; }
    public string? ProviderCustomerId { get; set; }
    public string? FailureReason { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? RefundedAt { get; set; }
    public decimal? RefundAmount { get; set; }
    public string? Metadata { get; set; }
}
