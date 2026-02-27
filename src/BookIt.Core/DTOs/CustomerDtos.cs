using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.DTOs;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
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
    public string? MembershipNumber { get; set; }
    public bool MarketingOptIn { get; set; }
    public bool SmsOptIn { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastVisit { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCustomerRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string? Phone { get; set; }

    [Phone(ErrorMessage = "A valid mobile number is required.")]
    [StringLength(30, ErrorMessage = "Mobile must not exceed 30 characters.")]
    public string? Mobile { get; set; }

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters.")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "City must not exceed 100 characters.")]
    public string? City { get; set; }

    [StringLength(20, ErrorMessage = "Post code must not exceed 20 characters.")]
    public string? PostCode { get; set; }

    [StringLength(100, ErrorMessage = "Country must not exceed 100 characters.")]
    public string? Country { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }

    [StringLength(2000, ErrorMessage = "Notes must not exceed 2000 characters.")]
    public string? Notes { get; set; }

    [StringLength(500, ErrorMessage = "Tags must not exceed 500 characters.")]
    public string? Tags { get; set; }

    [StringLength(50, ErrorMessage = "Membership number must not exceed 50 characters.")]
    public string? MembershipNumber { get; set; }

    public bool MarketingOptIn { get; set; }
    public bool SmsOptIn { get; set; }
}

public class UpdateCustomerRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string? Phone { get; set; }

    [Phone(ErrorMessage = "A valid mobile number is required.")]
    [StringLength(30, ErrorMessage = "Mobile must not exceed 30 characters.")]
    public string? Mobile { get; set; }

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters.")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "City must not exceed 100 characters.")]
    public string? City { get; set; }

    [StringLength(20, ErrorMessage = "Post code must not exceed 20 characters.")]
    public string? PostCode { get; set; }

    [StringLength(100, ErrorMessage = "Country must not exceed 100 characters.")]
    public string? Country { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }

    [StringLength(2000, ErrorMessage = "Notes must not exceed 2000 characters.")]
    public string? Notes { get; set; }

    [StringLength(500, ErrorMessage = "Tags must not exceed 500 characters.")]
    public string? Tags { get; set; }

    [StringLength(50, ErrorMessage = "Membership number must not exceed 50 characters.")]
    public string? MembershipNumber { get; set; }

    public bool MarketingOptIn { get; set; }
    public bool SmsOptIn { get; set; }
}

public class TenantListResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string BusinessTypeName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public int CustomerCount { get; set; }
    public int AppointmentCount { get; set; }
}
