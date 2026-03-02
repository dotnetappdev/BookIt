using System.ComponentModel.DataAnnotations;
using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class SubscriptionTierResponse
{
    public Guid Id { get; set; }
    public SubscriptionPlan Plan { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    // Pricing
    public decimal MonthlyPriceGbp { get; set; }
    public decimal MonthlyPriceUsd { get; set; }
    public decimal MonthlyPriceEur { get; set; }

    // Limits
    public int MaxServices { get; set; }
    public int MaxStaff { get; set; }
    public int MaxLocations { get; set; }
    public int MaxBookingsPerMonth { get; set; }

    // Features
    public bool CanUseOnlinePayments { get; set; }
    public bool CanUseCustomForms { get; set; }
    public bool CanUseAiAssistant { get; set; }
    public bool CanUseInterviews { get; set; }
    public bool CanUseApiAccess { get; set; }
    public bool CanRemoveBranding { get; set; }
    public bool CanUseMultipleStaff { get; set; }
}

public class UpsertSubscriptionTierRequest
{
    public SubscriptionPlan Plan { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string? Description { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    [Range(0, 100000, ErrorMessage = "GBP price must be between 0 and 100,000.")]
    public decimal MonthlyPriceGbp { get; set; }

    [Range(0, 100000, ErrorMessage = "USD price must be between 0 and 100,000.")]
    public decimal MonthlyPriceUsd { get; set; }

    [Range(0, 100000, ErrorMessage = "EUR price must be between 0 and 100,000.")]
    public decimal MonthlyPriceEur { get; set; }

    [Range(-1, int.MaxValue, ErrorMessage = "Max services must be -1 (unlimited) or a positive number.")]
    public int MaxServices { get; set; } = 3;

    [Range(-1, int.MaxValue, ErrorMessage = "Max staff must be -1 (unlimited) or a positive number.")]
    public int MaxStaff { get; set; } = 1;

    [Range(-1, int.MaxValue, ErrorMessage = "Max locations must be -1 (unlimited) or a positive number.")]
    public int MaxLocations { get; set; } = 1;

    [Range(-1, int.MaxValue, ErrorMessage = "Max bookings per month must be -1 (unlimited) or a positive number.")]
    public int MaxBookingsPerMonth { get; set; } = -1;

    public bool CanUseOnlinePayments { get; set; }
    public bool CanUseCustomForms { get; set; }
    public bool CanUseAiAssistant { get; set; }
    public bool CanUseInterviews { get; set; }
    public bool CanUseApiAccess { get; set; }
    public bool CanRemoveBranding { get; set; }
    public bool CanUseMultipleStaff { get; set; }
}
