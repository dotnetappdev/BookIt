using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class InterviewSlot : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Guid ServiceId { get; set; }   // Job Position (Service entity)
    public Service Service { get; set; } = null!;
    public Guid? StaffId { get; set; }     // Interviewer (Staff entity)
    public Staff? Staff { get; set; }
    public string InterviewerName { get; set; } = string.Empty; // fallback if no Staff
    public DateTime SlotStart { get; set; }
    public DateTime SlotEnd { get; set; }
    public bool IsBooked { get; set; } = false;
    public string? Location { get; set; }        // "Video call", "Room 3A", etc.
    public string? MeetingLink { get; set; }

    // Video conference configuration
    public VideoConferenceProvider VideoConferenceProvider { get; set; } = VideoConferenceProvider.None;
    public string? ConferenceMeetingId { get; set; }
    public string? ConferencePassword { get; set; }
    public string? ConferenceHostUrl { get; set; }
    public string? ConferenceDialIn { get; set; }
    public Guid? BookedByInvitationId { get; set; }
}
