using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

/// <summary>
/// Database-backed configuration for a subscription plan tier.
/// Allows super admins to manage plan names, prices, and feature limits.
/// </summary>
public class SubscriptionTier : BaseEntity
{
    public SubscriptionPlan Plan { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    // Pricing (per month)
    public decimal MonthlyPriceGbp { get; set; }
    public decimal MonthlyPriceUsd { get; set; }
    public decimal MonthlyPriceEur { get; set; }

    // Usage limits (-1 = unlimited)
    public int MaxServices { get; set; } = 3;
    public int MaxStaff { get; set; } = 1;
    public int MaxLocations { get; set; } = 1;
    public int MaxBookingsPerMonth { get; set; } = -1;

    // Feature flags
    public bool CanUseOnlinePayments { get; set; }
    public bool CanUseCustomForms { get; set; }
    public bool CanUseAiAssistant { get; set; }
    public bool CanUseInterviews { get; set; }
    public bool CanUseApiAccess { get; set; }
    public bool CanRemoveBranding { get; set; }
    public bool CanUseMultipleStaff { get; set; }
}
