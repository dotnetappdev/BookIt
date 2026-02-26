namespace BookIt.Notifications.Email;

/// <summary>
/// Email notification service for booking-related messages.
/// All methods accept credentials per-call for multi-tenant use.
/// </summary>
public interface IEmailNotificationService
{
    /// <summary>Sends a booking confirmation email to the customer.</summary>
    Task SendBookingConfirmationAsync(
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
        CancellationToken cancellationToken = default);

    /// <summary>Sends a reminder email to the customer ahead of their appointment.</summary>
    Task SendAppointmentReminderAsync(
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
        CancellationToken cancellationToken = default);

    /// <summary>Sends a booking cancellation notification email to the customer.</summary>
    Task SendBookingCancellationAsync(
        string sendGridApiKey,
        string fromEmail,
        string fromName,
        string toEmail,
        string customerName,
        string businessName,
        string serviceName,
        DateTime appointmentStart,
        string? reason,
        CancellationToken cancellationToken = default);
}
