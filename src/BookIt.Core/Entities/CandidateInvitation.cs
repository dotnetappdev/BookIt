namespace BookIt.Core.Entities;

public class CandidateInvitation : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Guid ServiceId { get; set; }   // Job Position
    public Service Service { get; set; } = null!;
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string? CandidatePhone { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? Notes { get; set; }
    public string Token { get; set; } = string.Empty;  // unique booking token
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
    public Guid? BookedSlotId { get; set; }
    public InterviewSlot? BookedSlot { get; set; }
    public string InvitedBy { get; set; } = string.Empty; // admin user name
}
