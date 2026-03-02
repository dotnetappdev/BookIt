using System.ComponentModel.DataAnnotations;
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

    [Required(ErrorMessage = "Interviewer name is required.")]
    [StringLength(200, ErrorMessage = "Interviewer name must not exceed 200 characters.")]
    public string InterviewerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slot start date and time are required.")]
    public DateTime SlotStart { get; set; }

    [Range(15, 480, ErrorMessage = "Duration must be between 15 and 480 minutes.")]
    public int DurationMinutes { get; set; } = 60;

    [StringLength(500, ErrorMessage = "Location must not exceed 500 characters.")]
    public string? Location { get; set; }

    [Url(ErrorMessage = "Meeting link must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Meeting link must not exceed 2000 characters.")]
    public string? MeetingLink { get; set; }

    public VideoConferenceProvider VideoConferenceProvider { get; set; } = VideoConferenceProvider.None;

    [StringLength(200, ErrorMessage = "Meeting ID must not exceed 200 characters.")]
    public string? ConferenceMeetingId { get; set; }

    [StringLength(200, ErrorMessage = "Meeting password must not exceed 200 characters.")]
    public string? ConferencePassword { get; set; }

    [Url(ErrorMessage = "Host URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Host URL must not exceed 2000 characters.")]
    public string? ConferenceHostUrl { get; set; }

    [Phone(ErrorMessage = "Dial-in number must be a valid phone number.")]
    [StringLength(50, ErrorMessage = "Dial-in number must not exceed 50 characters.")]
    public string? ConferenceDialIn { get; set; }
}

public class SendInvitationRequest
{
    public Guid ServiceId { get; set; }

    [Required(ErrorMessage = "Candidate name is required.")]
    [StringLength(200, ErrorMessage = "Candidate name must not exceed 200 characters.")]
    public string CandidateName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Candidate email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string CandidateEmail { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string? CandidatePhone { get; set; }
}

public class CandidateInvitationResponse
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string? CandidatePhone { get; set; }
    public Guid ServiceId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public string? BookingUrl { get; set; }
}

public class BookInterviewRequest
{
    public Guid SlotId { get; set; }

    [Required(ErrorMessage = "Candidate name is required.")]
    [StringLength(200, ErrorMessage = "Candidate name must not exceed 200 characters.")]
    public string CandidateName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Candidate email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(254, ErrorMessage = "Email must not exceed 254 characters.")]
    public string CandidateEmail { get; set; } = string.Empty;

    [Phone(ErrorMessage = "A valid phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone must not exceed 30 characters.")]
    public string? CandidatePhone { get; set; }

    [Url(ErrorMessage = "LinkedIn URL must be a valid URL.")]
    [StringLength(500, ErrorMessage = "LinkedIn URL must not exceed 500 characters.")]
    public string? LinkedInUrl { get; set; }

    [StringLength(2000, ErrorMessage = "Notes must not exceed 2000 characters.")]
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
