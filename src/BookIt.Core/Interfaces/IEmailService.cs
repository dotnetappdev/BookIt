using BookIt.Core.Enums;

namespace BookIt.Core.Interfaces;

public interface IEmailService
{
    Task SendInterviewInvitationAsync(string toEmail, string candidateName, string companyName, string position, string bookingUrl, DateTime expiresAt);
    Task SendInterviewConfirmationAsync(string toEmail, string candidateName, string companyName, string position, DateTime slotStart, string? location, string? meetingLink, VideoConferenceProvider vcProvider = VideoConferenceProvider.None, string? conferenceMeetingId = null, string? conferencePassword = null, string? conferenceDialIn = null);
}
