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
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
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
