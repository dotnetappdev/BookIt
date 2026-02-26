using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class InterviewSlotResponse
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string InterviewerName { get; set; } = string.Empty;
    public DateTime SlotStart { get; set; }
    public DateTime SlotEnd { get; set; }
    public bool IsBooked { get; set; }
    public string? Location { get; set; }
    public string? MeetingLink { get; set; }
    public VideoConferenceProvider VideoConferenceProvider { get; set; }
    public string? ConferenceMeetingId { get; set; }
    public string? ConferencePassword { get; set; }
    public string? ConferenceHostUrl { get; set; }
    public string? ConferenceDialIn { get; set; }
}

public class CreateInterviewSlotRequest
{
    public Guid ServiceId { get; set; }
    public Guid? StaffId { get; set; }
    public string InterviewerName { get; set; } = string.Empty;
    public DateTime SlotStart { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public string? Location { get; set; }
    public string? MeetingLink { get; set; }
    public VideoConferenceProvider VideoConferenceProvider { get; set; } = VideoConferenceProvider.None;
    public string? ConferenceMeetingId { get; set; }
    public string? ConferencePassword { get; set; }
    public string? ConferenceHostUrl { get; set; }
    public string? ConferenceDialIn { get; set; }
}

public class SendInvitationRequest
{
    public Guid ServiceId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string? CandidatePhone { get; set; }
}

public class CandidateInvitationResponse
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public string? BookingUrl { get; set; }
}

public class BookInterviewRequest
{
    public Guid SlotId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string? CandidatePhone { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? Notes { get; set; }
}

public class InvitationDetailResponse
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public Guid ServiceId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string? PositionDescription { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string? TenantLogoUrl { get; set; }
    public string? TenantAddress { get; set; }
    public string TenantSlug { get; set; } = string.Empty;
    public bool IsExpired { get; set; }
    public bool IsUsed { get; set; }
    public List<InterviewSlotResponse> AvailableSlots { get; set; } = new();
}
