using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class Subscription : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Free;
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trialing;
    public string? ProviderSubscriptionId { get; set; }
    public string? ProviderCustomerId { get; set; }
    public string? RevenueCatCustomerId { get; set; }
    public PaymentProvider? PaymentProvider { get; set; }
    public decimal MonthlyPrice { get; set; }
    public string Currency { get; set; } = "GBP";
    public DateTime? TrialEndsAt { get; set; }
    public DateTime? CurrentPeriodStart { get; set; }
    public DateTime? CurrentPeriodEnd { get; set; }
    public DateTime? CancelledAt { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
}
