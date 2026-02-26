using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using BookIt.Notifications.Email;
using BookIt.Notifications.Sms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastructure.Services;

/// <summary>
/// Hangfire job that sends reminder notifications for a specific appointment.
/// Loaded from DI, so it can access the database and notification providers.
/// </summary>
public sealed class AppointmentReminderJob
{
    private readonly BookItDbContext _context;
    private readonly IEmailNotificationService _emailService;
    private readonly SmsProviderFactory _smsFactory;
    private readonly ILogger<AppointmentReminderJob> _logger;

    public AppointmentReminderJob(
        BookItDbContext context,
        IEmailNotificationService emailService,
        SmsProviderFactory smsFactory,
        ILogger<AppointmentReminderJob> logger)
    {
        _context = context;
        _emailService = emailService;
        _smsFactory = smsFactory;
        _logger = logger;
    }

    public async Task SendAsync(Guid appointmentId, Guid tenantId, int minutesBefore, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Services).ThenInclude(s => s.Service)
            .Include(a => a.Tenant)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.TenantId == tenantId, cancellationToken);

        if (appointment is null || appointment.Status == Core.Enums.AppointmentStatus.Cancelled)
        {
            _logger.LogInformation("Skipping reminder for appointment {Id} — not found or cancelled.", appointmentId);
            return;
        }

        var tenant = appointment.Tenant;
        var serviceName = appointment.Services.FirstOrDefault()?.Service?.Name ?? "Appointment";

        // Load custom reminder email template if one exists for this tenant
        CustomEmailTemplate? customTemplate = null;
        if (tenant.EnableEmailReminders && !string.IsNullOrEmpty(tenant.SendGridApiKey))
        {
            var activeTemplates = await _context.EmailTemplates
                .Where(t => t.TenantId == tenant.Id &&
                            t.TemplateType == EmailTemplateType.AppointmentReminder &&
                            t.IsActive)
                .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
                .ToListAsync(cancellationToken);

            if (activeTemplates.Count > 1)
                _logger.LogWarning(
                    "Tenant {TenantId} has {Count} active AppointmentReminder email templates; using the most recently updated one.",
                    tenant.Id, activeTemplates.Count);

            var raw = activeTemplates.FirstOrDefault();
            if (raw != null)
                customTemplate = new CustomEmailTemplate(raw.SubjectLine, raw.HtmlBody);
        }

        // Send email reminder
        if (tenant.EnableEmailReminders && !string.IsNullOrEmpty(tenant.SendGridApiKey))
        {
            await _emailService.SendAppointmentReminderAsync(
                tenant.SendGridApiKey,
                tenant.SendGridFromEmail ?? tenant.ContactEmail ?? "noreply@bookit.app",
                tenant.SendGridFromName ?? tenant.Name,
                appointment.CustomerEmail,
                appointment.CustomerName,
                tenant.Name,
                serviceName,
                appointment.StartTime,
                minutesBefore,
                tenant.Address,
                appointment.MeetingLink,
                customTemplate,
                cancellationToken);
        }

        // Send SMS reminder
        if (tenant.EnableSmsReminders && tenant.EnableSmsNotifications && !string.IsNullOrEmpty(appointment.CustomerPhone))
        {
            var providerName = tenant.SmsProvider.ToString().ToLowerInvariant();
            // Build credential string without storing secrets in named variables that could be captured by structured logging
            bool hasCredential = tenant.SmsProvider switch
            {
                Core.Enums.SmsProvider.Twilio =>
                    !string.IsNullOrEmpty(tenant.TwilioAccountSid) &&
                    !string.IsNullOrEmpty(tenant.TwilioAuthToken) &&
                    !string.IsNullOrEmpty(tenant.TwilioFromNumber),
                _ =>
                    !string.IsNullOrEmpty(tenant.ClickSendUsername) &&
                    !string.IsNullOrEmpty(tenant.ClickSendApiKey)
            };

            if (hasCredential)
            {
                var whenText = minutesBefore switch
                {
                    <= 60 => $"in {minutesBefore} minutes",
                    < 1440 => $"in {minutesBefore / 60} hours",
                    1440 => "tomorrow",
                    _ => $"in {minutesBefore / 1440} days"
                };

                var smsBody = $"Reminder: Your {serviceName} appointment at {tenant.Name} is {whenText} ({appointment.StartTime:h:mm tt on d MMM}).";
                var credential = tenant.SmsProvider switch
                {
                    Core.Enums.SmsProvider.Twilio =>
                        string.Concat(tenant.TwilioAccountSid, ":", tenant.TwilioAuthToken, ":", tenant.TwilioFromNumber),
                    _ =>
                        string.Concat(tenant.ClickSendUsername, ":", tenant.ClickSendApiKey)
                };

                var provider = _smsFactory.Get(providerName);
                var result = await provider.SendAsync(appointment.CustomerPhone, smsBody, credential, cancellationToken);

                if (!result.Success)
                    _logger.LogWarning("SMS reminder failed for appointment {Id}: {Error}", appointmentId, result.ErrorMessage);
            }
        }

        // Mark as sent — record the time so subsequent alerts are not blocked
        appointment.ReminderSent = true;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
