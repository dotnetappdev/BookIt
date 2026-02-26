using BookIt.Core.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastructure.Services;

/// <summary>
/// Hangfire-based reminder scheduler. Enqueues delayed jobs that fire reminder
/// notifications at the configured alert offsets before each appointment.
/// </summary>
public sealed class HangfireReminderScheduler : IReminderScheduler
{
    private readonly IBackgroundJobClient _jobClient;
    private readonly ILogger<HangfireReminderScheduler> _logger;

    public HangfireReminderScheduler(IBackgroundJobClient jobClient, ILogger<HangfireReminderScheduler> logger)
    {
        _jobClient = jobClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public void ScheduleReminders(
        Guid appointmentId,
        Guid tenantId,
        DateTime appointmentStart,
        string[] reminderAlertMinutes)
    {
        foreach (var alert in reminderAlertMinutes)
        {
            if (!int.TryParse(alert.Trim(), out var minutes) || minutes <= 0) continue;

            var fireAt = appointmentStart.AddMinutes(-minutes);
            if (fireAt <= DateTime.UtcNow) continue;   // skip if already past

            var jobId = _jobClient.Schedule<AppointmentReminderJob>(
                job => job.SendAsync(appointmentId, tenantId, minutes, CancellationToken.None),
                fireAt - DateTime.UtcNow);

            _logger.LogInformation(
                "Scheduled reminder for appointment {Id} at {FireAt} (jobId={JobId})",
                appointmentId, fireAt, jobId);
        }
    }

    /// <inheritdoc />
    public void CancelReminders(Guid appointmentId)
    {
        // Hangfire doesn't expose a query-by-parameter API in the basic client.
        // The AppointmentReminderJob already re-checks appointment status at fire time
        // and skips sending if the appointment is cancelled, providing a safe fallback.
        _logger.LogInformation("Reminder cancellation requested for appointment {Id} — job will self-skip if cancelled.", appointmentId);
    }
}
