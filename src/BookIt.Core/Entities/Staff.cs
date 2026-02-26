namespace BookIt.Core.Entities;

public class Staff : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public ICollection<StaffService> Services { get; set; } = new List<StaffService>();
    public ICollection<StaffAvailability> Availability { get; set; } = new List<StaffAvailability>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
