namespace BookIt.Core.Interfaces;

public interface IEmailService
{
    Task SendInterviewInvitationAsync(string toEmail, string candidateName, string companyName, string position, string bookingUrl, DateTime expiresAt);
    Task SendInterviewConfirmationAsync(string toEmail, string candidateName, string companyName, string position, DateTime slotStart, string? location, string? meetingLink);
}
