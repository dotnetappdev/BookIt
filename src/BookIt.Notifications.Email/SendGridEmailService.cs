using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BookIt.Notifications.Email;

/// <summary>
/// SendGrid-backed implementation of <see cref="IEmailNotificationService"/>.
/// Credentials are accepted per-call for multi-tenant use.
/// </summary>
public sealed class SendGridEmailService : IEmailNotificationService
{
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(ILogger<SendGridEmailService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task SendBookingConfirmationAsync(
        string sendGridApiKey,
        string fromEmail,
        string fromName,
        string toEmail,
        string customerName,
        string businessName,
        string serviceName,
        DateTime appointmentStart,
        DateTime appointmentEnd,
        string? location,
        string? meetingLink,
        string? bookingPin,
        CancellationToken cancellationToken = default)
    {
        var html = EmailTemplates.BookingConfirmation(
            customerName, businessName, serviceName,
            appointmentStart, appointmentEnd,
            location, meetingLink, bookingPin);

        await SendAsync(
            sendGridApiKey, fromEmail, fromName, toEmail,
            customerName,
            $"Booking Confirmed — {serviceName} on {appointmentStart:d MMM}",
            html, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SendAppointmentReminderAsync(
        string sendGridApiKey,
        string fromEmail,
        string fromName,
        string toEmail,
        string customerName,
        string businessName,
        string serviceName,
        DateTime appointmentStart,
        int minutesBefore,
        string? location,
        string? meetingLink,
        CancellationToken cancellationToken = default)
    {
        var html = EmailTemplates.AppointmentReminder(
            customerName, businessName, serviceName,
            appointmentStart, minutesBefore, location, meetingLink);

        var whenText = minutesBefore switch
        {
            <= 60 => $"in {minutesBefore} minutes",
            < 1440 => $"in {minutesBefore / 60} hours",
            1440 => "tomorrow",
            _ => $"in {minutesBefore / 1440} days"
        };

        await SendAsync(
            sendGridApiKey, fromEmail, fromName, toEmail,
            customerName,
            $"Reminder: {serviceName} {whenText}",
            html, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SendBookingCancellationAsync(
        string sendGridApiKey,
        string fromEmail,
        string fromName,
        string toEmail,
        string customerName,
        string businessName,
        string serviceName,
        DateTime appointmentStart,
        string? reason,
        CancellationToken cancellationToken = default)
    {
        var html = EmailTemplates.BookingCancellation(
            customerName, businessName, serviceName, appointmentStart, reason);

        await SendAsync(
            sendGridApiKey, fromEmail, fromName, toEmail,
            customerName,
            $"Booking Cancelled — {serviceName}",
            html, cancellationToken);
    }

    private async Task SendAsync(
        string apiKey,
        string fromEmail,
        string fromName,
        string toEmail,
        string toName,
        string subject,
        string htmlContent,
        CancellationToken cancellationToken)
    {
        var client = new SendGridClient(apiKey);
        var msg = MailHelper.CreateSingleEmail(
            new EmailAddress(fromEmail, fromName),
            new EmailAddress(toEmail, toName),
            subject,
            plainTextContent: null,
            htmlContent: htmlContent);

        var response = await client.SendEmailAsync(msg, cancellationToken);

        if ((int)response.StatusCode >= 400)
        {
            var body = await response.Body.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning(
                "SendGrid email to {To} failed: {Status} — {Body}",
                toEmail, response.StatusCode, body);
        }
        else
        {
            _logger.LogInformation(
                "SendGrid email '{Subject}' sent to {To}", subject, toEmail);
        }
    }
}
