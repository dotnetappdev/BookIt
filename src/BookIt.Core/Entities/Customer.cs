namespace BookIt.Core.Entities;

/// <summary>
/// A customer profile belonging to a tenant. Tracks contact details, preferences,
/// and aggregated booking statistics for the admin CRM.
/// </summary>
public class Customer : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }

    /// <summary>Gym / club / loyalty scheme membership number.</summary>
    public string? MembershipNumber { get; set; }

    public bool MarketingOptIn { get; set; }
    public bool SmsOptIn { get; set; }

    // Computed / cached stats (updated on each booking)
    public int TotalBookings { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastVisit { get; set; }

    /// <summary>Optional link to an authenticated user account (if the customer registered).</summary>
    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
