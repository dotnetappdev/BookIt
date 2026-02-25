using BookIt.Core.Entities;

namespace BookIt.Core.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(Guid tenantId, Guid serviceId, Guid? staffId, DateOnly date);
    Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    Task<Appointment?> GetAppointmentAsync(Guid tenantId, Guid appointmentId);
    Task CancelAppointmentAsync(Guid tenantId, Guid appointmentId, string? reason);
    Task ConfirmAppointmentAsync(Guid tenantId, Guid appointmentId);

    // Class session methods (for gyms, swimming, classes)
    Task<IEnumerable<ClassSession>> GetUpcomingClassSessionsAsync(Guid tenantId, Guid? serviceId = null, int days = 14);
    Task<ClassSession> CreateClassSessionAsync(ClassSession session);
    Task<bool> BookClassSessionAsync(Guid tenantId, Guid classSessionId, Guid appointmentId, int participantCount = 1);
    Task<bool> CancelClassSessionBookingAsync(Guid tenantId, Guid classSessionId, Guid appointmentId);
}
