using System.ComponentModel.DataAnnotations;

namespace BookIt.Core.DTOs;

public class ClientResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public int StaffCount { get; set; }
    public bool EnableSoftDelete { get; set; } = true;
}

public class CreateClientRequest
{
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 200 characters.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact name is required.")]
    [StringLength(200, ErrorMessage = "Contact name must not exceed 200 characters.")]
    public string ContactName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters.")]
    public string? Address { get; set; }

    [StringLength(2000, ErrorMessage = "Notes must not exceed 2000 characters.")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 128 characters.")]
    public string Password { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public bool EnableSoftDelete { get; set; } = true;
}

public class UpdateClientRequest
{
    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 200 characters.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact name is required.")]
    [StringLength(200, ErrorMessage = "Contact name must not exceed 200 characters.")]
    public string ContactName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Address must not exceed 500 characters.")]
    public string? Address { get; set; }

    [StringLength(2000, ErrorMessage = "Notes must not exceed 2000 characters.")]
    public string? Notes { get; set; }

    public bool IsActive { get; set; }
    public bool EnableSoftDelete { get; set; } = true;
}
