using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class CreatePaymentIntentRequest
{
    public Guid TenantId { get; set; }
    public Guid AppointmentId { get; set; }
    public PaymentProvider Provider { get; set; }
}

public class PaymentIntentResponse
{
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public PaymentProvider Provider { get; set; }
}

public class PaymentWebhookRequest
{
    public string Payload { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public PaymentProvider Provider { get; set; }
}
