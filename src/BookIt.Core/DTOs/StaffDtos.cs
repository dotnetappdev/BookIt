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
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public bool SendInvite { get; set; } = true;
    public Guid? ClientId { get; set; }
}

public class UpdateStaffRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;
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
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
