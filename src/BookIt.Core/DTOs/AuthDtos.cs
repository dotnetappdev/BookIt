using System.ComponentModel.DataAnnotations;
using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 128 characters.")]
    public string Password { get; set; } = string.Empty;

    [RegularExpression(@"^[a-z0-9\-]{2,100}$", ErrorMessage = "Business slug may only contain lowercase letters, digits and hyphens.")]
    public string? TenantSlug { get; set; }
}

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 128 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string? Phone { get; set; }

    [StringLength(50, ErrorMessage = "Membership number must not exceed 50 characters.")]
    public string? MembershipNumber { get; set; }

    [RegularExpression(@"^[a-z0-9\-]{2,100}$", ErrorMessage = "Business slug may only contain lowercase letters, digits and hyphens.")]
    public string? TenantSlug { get; set; }
}

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid TenantId { get; set; }
    public string TenantSlug { get; set; } = string.Empty;
    public string? MembershipNumber { get; set; }
}

public class TenantSetupRequest
{
    [Required(ErrorMessage = "Business name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Business name must be between 2 and 200 characters.")]
    public string BusinessName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Admin email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string AdminEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Admin password is required.")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 128 characters.")]
    public string AdminPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    public string AdminFirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    public string AdminLastName { get; set; } = string.Empty;

    public BusinessType BusinessType { get; set; }

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string? Phone { get; set; }

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters.")]
    public string? Address { get; set; }

    public string? TimeZone { get; set; } = "UTC";
    public string? Currency { get; set; } = "GBP";
    public string? ConnectionString { get; set; }
}
