using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BookIt.Notifications.Email;

/// <summary>
/// SendGrid-backed implementation of <see cref="IEmailNotificationService"/>.
/// Credentials are accepted per-call for multi-tenant use.
/// When a <see cref="CustomEmailTemplate"/> is provided the built-in template is bypassed
/// and placeholders are substituted before sending.
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
        CustomEmailTemplate? customTemplate = null,
        CancellationToken cancellationToken = default)
    {
        string subject;
        string html;

        if (customTemplate != null)
        {
            var placeholders = BuildPlaceholders(customerName, businessName, serviceName,
                appointmentStart, appointmentEnd: appointmentEnd,
                location: location, meetingLink: meetingLink, bookingPin: bookingPin);
            subject = ApplyPlaceholders(customTemplate.Subject, placeholders);
            html = ApplyPlaceholders(customTemplate.HtmlBody, placeholders);
        }
        else
        {
            html = EmailTemplates.BookingConfirmation(
                customerName, businessName, serviceName,
                appointmentStart, appointmentEnd,
                location, meetingLink, bookingPin);
            subject = $"Booking Confirmed \u2014 {serviceName} on {appointmentStart:d MMM}";
        }

        await SendAsync(sendGridApiKey, fromEmail, fromName, toEmail, customerName, subject, html, cancellationToken);
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
        CustomEmailTemplate? customTemplate = null,
        CancellationToken cancellationToken = default)
    {
        var whenText = minutesBefore switch
        {
            <= 60 => $"in {minutesBefore} minutes",
            < 1440 => $"in {minutesBefore / 60} hours",
            1440 => "tomorrow",
            _ => $"in {minutesBefore / 1440} days"
        };

        string subject;
        string html;

        if (customTemplate != null)
        {
            var placeholders = BuildPlaceholders(customerName, businessName, serviceName,
                appointmentStart, location: location, meetingLink: meetingLink,
                minutesBefore: minutesBefore, whenText: whenText);
            subject = ApplyPlaceholders(customTemplate.Subject, placeholders);
            html = ApplyPlaceholders(customTemplate.HtmlBody, placeholders);
        }
        else
        {
            html = EmailTemplates.AppointmentReminder(
                customerName, businessName, serviceName,
                appointmentStart, minutesBefore, location, meetingLink);
            subject = $"Reminder: {serviceName} {whenText}";
        }

        await SendAsync(sendGridApiKey, fromEmail, fromName, toEmail, customerName, subject, html, cancellationToken);
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
        CustomEmailTemplate? customTemplate = null,
        CancellationToken cancellationToken = default)
    {
        string subject;
        string html;

        if (customTemplate != null)
        {
            var placeholders = BuildPlaceholders(customerName, businessName, serviceName,
                appointmentStart, cancellationReason: reason);
            subject = ApplyPlaceholders(customTemplate.Subject, placeholders);
            html = ApplyPlaceholders(customTemplate.HtmlBody, placeholders);
        }
        else
        {
            html = EmailTemplates.BookingCancellation(customerName, businessName, serviceName, appointmentStart, reason);
            subject = $"Booking Cancelled \u2014 {serviceName}";
        }

        await SendAsync(sendGridApiKey, fromEmail, fromName, toEmail, customerName, subject, html, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SendBookingApprovedAsync(
        string sendGridApiKey,
        string fromEmail,
        string fromName,
        string toEmail,
        string customerName,
        string businessName,
        DateTime appointmentStart,
        string? confirmationToken,
        CancellationToken cancellationToken = default)
    {
        var html = EmailTemplates.BookingApproved(customerName, businessName, appointmentStart, confirmationToken);
        var subject = $"Booking Approved \u2014 {businessName} on {appointmentStart:d MMM}";
        await SendAsync(sendGridApiKey, fromEmail, fromName, toEmail, customerName, subject, html, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SendBookingDeclinedAsync(
        string sendGridApiKey,
        string fromEmail,
        string fromName,
        string toEmail,
        string customerName,
        string businessName,
        DateTime appointmentStart,
        string? reason,
        CancellationToken cancellationToken = default)
    {
        var html = EmailTemplates.BookingDeclined(customerName, businessName, appointmentStart, reason);
        var subject = $"Booking Declined \u2014 {businessName}";
        await SendAsync(sendGridApiKey, fromEmail, fromName, toEmail, customerName, subject, html, cancellationToken);
    }

    // Placeholder helpers

    private static Dictionary<string, string> BuildPlaceholders(
        string customerName,
        string businessName,
        string serviceName,
        DateTime appointmentStart,
        DateTime? appointmentEnd = null,
        string? location = null,
        string? meetingLink = null,
        string? bookingPin = null,
        string? cancellationReason = null,
        int minutesBefore = 0,
        string? whenText = null) => new(StringComparer.OrdinalIgnoreCase)
    {
        ["CustomerName"]        = customerName,
        ["BusinessName"]        = businessName,
        ["ServiceName"]         = serviceName,
        ["AppointmentDate"]     = appointmentStart.ToString("dddd, d MMMM yyyy"),
        ["AppointmentTime"]     = appointmentStart.ToString("h:mm tt"),
        ["AppointmentDateTime"] = appointmentStart.ToString("dddd, d MMMM yyyy 'at' h:mm tt"),
        ["Location"]            = location ?? "",
        ["MeetingLink"]         = meetingLink ?? "",
        ["BookingPin"]          = bookingPin ?? "",
        ["CancellationReason"]  = cancellationReason ?? "",
        ["MinutesBefore"]       = minutesBefore.ToString(),
        ["WhenText"]            = whenText ?? "",
        ["AppointmentEnd"]      = appointmentEnd?.ToString("h:mm tt") ?? "",
    };

    private static string ApplyPlaceholders(string template, Dictionary<string, string> placeholders)
    {
        foreach (var (key, value) in placeholders)
            template = template.Replace($"{{{{{key}}}}}", value, StringComparison.OrdinalIgnoreCase);
        return template;
    }

    // Core send

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
                "SendGrid email to {To} failed: {Status} \u2014 {Body}",
                toEmail, response.StatusCode, body);
        }
        else
        {
            _logger.LogInformation(
                "SendGrid email '{Subject}' sent to {To}", subject, toEmail);
        }
    }
}
