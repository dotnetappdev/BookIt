using BookIt.Core.Entities;

namespace BookIt.Core.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(Guid tenantId, Guid serviceId, Guid? staffId, DateOnly date);
    Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    Task<Appointment?> GetAppointmentAsync(Guid tenantId, Guid appointmentId);
    Task CancelAppointmentAsync(Guid tenantId, Guid appointmentId, string? reason);
    Task ConfirmAppointmentAsync(Guid tenantId, Guid appointmentId);
}
