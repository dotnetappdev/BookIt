using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.DTOs;

public class StaffResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public Guid? ClientId { get; set; }
    public string? ClientName { get; set; }
    public List<StaffServiceItem> Services { get; set; } = new();
}

public class StaffServiceItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateStaffRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "First name must not exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone number must not exceed 30 characters.")]
    public string Phone { get; set; } = string.Empty;

    [Url(ErrorMessage = "Photo URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Photo URL must not exceed 2000 characters.")]
    public string? PhotoUrl { get; set; }

    [StringLength(1000, ErrorMessage = "Bio must not exceed 1000 characters.")]
    public string? Bio { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(0, 9999, ErrorMessage = "Sort order must be between 0 and 9999.")]
    public int SortOrder { get; set; }

    public bool SendInvite { get; set; } = true;
    public Guid? ClientId { get; set; }
}

public class UpdateStaffRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "First name must not exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone number must not exceed 30 characters.")]
    public string? Phone { get; set; }

    [Url(ErrorMessage = "Photo URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Photo URL must not exceed 2000 characters.")]
    public string? PhotoUrl { get; set; }

    [StringLength(1000, ErrorMessage = "Bio must not exceed 1000 characters.")]
    public string? Bio { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(0, 9999, ErrorMessage = "Sort order must be between 0 and 9999.")]
    public int SortOrder { get; set; }
}

public class AssignStaffServicesRequest
{
    public List<Guid> ServiceIds { get; set; } = new();
}

public class StaffInvitationResponse
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}

public class AcceptStaffInvitationRequest
{
    [Required(ErrorMessage = "Invitation token is required.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 128 characters.")]
    public string Password { get; set; } = string.Empty;
}
