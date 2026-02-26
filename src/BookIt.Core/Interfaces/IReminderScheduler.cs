namespace BookIt.Core.Interfaces;

/// <summary>
/// Schedules and cancels reminder jobs for appointments.
/// Implementations may use Hangfire, Quartz, or any other job scheduler.
/// </summary>
public interface IReminderScheduler
{
    /// <summary>
    /// Schedules email and/or SMS reminder jobs for the given appointment
    /// according to the tenant's configured alert offsets.
    /// </summary>
    void ScheduleReminders(
        Guid appointmentId,
        Guid tenantId,
        DateTime appointmentStart,
        string[] reminderAlertMinutes);

    /// <summary>
    /// Cancels all pending reminder jobs for the given appointment.
    /// Called when an appointment is cancelled or rescheduled.
    /// </summary>
    void CancelReminders(Guid appointmentId);
}
