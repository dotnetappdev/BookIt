using BookIt.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastructure.Services;

public class ConsoleEmailService : IEmailService
{
    private readonly ILogger<ConsoleEmailService> _logger;

    public ConsoleEmailService(ILogger<ConsoleEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendInterviewInvitationAsync(string toEmail, string candidateName, string companyName, string position, string bookingUrl, DateTime expiresAt)
    {
        _logger.LogInformation(
            "[EMAIL] Interview invitation to {Email} ({Name})\nCompany: {Company}\nPosition: {Position}\nBooking URL: {Url}\nExpires: {Expires}",
            toEmail, candidateName, companyName, position, bookingUrl, expiresAt);
        return Task.CompletedTask;
    }

    public Task SendInterviewConfirmationAsync(string toEmail, string candidateName, string companyName, string position, DateTime slotStart, string? location, string? meetingLink)
    {
        _logger.LogInformation(
            "[EMAIL] Interview confirmation to {Email} ({Name})\nCompany: {Company}\nPosition: {Position}\nSlot: {Slot}\nLocation: {Location}\nMeeting: {Meeting}",
            toEmail, candidateName, companyName, position, slotStart, location ?? "TBD", meetingLink ?? "N/A");
        return Task.CompletedTask;
    }
}
