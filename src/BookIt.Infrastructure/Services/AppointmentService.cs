using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly BookItDbContext _context;

    public AppointmentService(BookItDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(Guid tenantId, Guid serviceId, Guid? staffId, DateOnly date)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.TenantId == tenantId && s.Id == serviceId && !s.IsDeleted);

        if (service == null) return Enumerable.Empty<DateTime>();

        var dayOfWeek = (DayOfWeekFlag)((int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek);

        var businessHours = await _context.BusinessHours
            .FirstOrDefaultAsync(bh => bh.TenantId == tenantId && bh.DayOfWeek == dayOfWeek && !bh.IsClosed);

        if (businessHours == null) return Enumerable.Empty<DateTime>();

        var dateStart = date.ToDateTime(TimeOnly.MinValue);
        var dateEnd = date.ToDateTime(TimeOnly.MaxValue);

        var existingAppointments = await _context.Appointments
            .Where(a => a.TenantId == tenantId
                && (staffId == null || a.StaffId == staffId)
                && a.StartTime >= dateStart
                && a.StartTime <= dateEnd
                && a.Status != AppointmentStatus.Cancelled)
            .ToListAsync();

        if (staffId.HasValue)
        {
            var staffAvailability = await _context.StaffAvailabilities
                .FirstOrDefaultAsync(sa => sa.StaffId == staffId.Value && sa.DayOfWeek == dayOfWeek && sa.IsAvailable);

            if (staffAvailability == null) return Enumerable.Empty<DateTime>();
        }

        var slots = new List<DateTime>();
        var slotDuration = TimeSpan.FromMinutes(businessHours.SlotDurationMinutes);

        var current = date.ToDateTime(businessHours.OpenTime);
        var end = date.ToDateTime(businessHours.CloseTime).Subtract(TimeSpan.FromMinutes(service.DurationMinutes));

        while (current <= end)
        {
            var slotEnd = current.Add(TimeSpan.FromMinutes(service.DurationMinutes));

            // For group sessions: count bookings in this slot, allow up to MaxCapacity
            if (service.IsGroupSession && service.MaxCapacity > 1)
            {
                var bookingsInSlot = existingAppointments.Count(a =>
                    a.StartTime < slotEnd && a.EndTime > current);

                if (bookingsInSlot < service.MaxCapacity)
                    slots.Add(current);
            }
            else
            {
                var isAvailable = !existingAppointments.Any(a =>
                    a.StartTime < slotEnd && a.EndTime > current);

                if (isAvailable)
                    slots.Add(current);
            }

            current = current.Add(slotDuration);
        }

        return slots;
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
    {
        appointment.ConfirmationToken = Guid.NewGuid().ToString("N")[..12].ToUpper();
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment?> GetAppointmentAsync(Guid tenantId, Guid appointmentId)
    {
        return await _context.Appointments
            .Include(a => a.Services).ThenInclude(s => s.Service)
            .Include(a => a.Staff)
            .Include(a => a.Payments)
            .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.Id == appointmentId);
    }

    public async Task CancelAppointmentAsync(Guid tenantId, Guid appointmentId, string? reason)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.Id == appointmentId);

        if (appointment != null)
        {
            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = reason;
            appointment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task ConfirmAppointmentAsync(Guid tenantId, Guid appointmentId)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.Id == appointmentId);

        if (appointment != null)
        {
            appointment.Status = AppointmentStatus.Confirmed;
            appointment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ClassSession>> GetUpcomingClassSessionsAsync(Guid tenantId, Guid? serviceId = null, int days = 14)
    {
        var cutoff = DateTime.UtcNow.AddDays(days);
        var query = _context.ClassSessions
            .Include(cs => cs.Service)
            .Include(cs => cs.Staff)
            .Where(cs => cs.TenantId == tenantId
                && cs.SessionDate >= DateTime.UtcNow.Date
                && cs.SessionDate <= cutoff
                && cs.Status != SessionStatus.Cancelled
                && !cs.IsDeleted);

        if (serviceId.HasValue)
            query = query.Where(cs => cs.ServiceId == serviceId.Value);

        return await query.OrderBy(cs => cs.SessionDate).ThenBy(cs => cs.StartTime).ToListAsync();
    }

    public async Task<ClassSession> CreateClassSessionAsync(ClassSession session)
    {
        _context.ClassSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<bool> BookClassSessionAsync(Guid tenantId, Guid classSessionId, Guid appointmentId, int participantCount = 1)
    {
        var session = await _context.ClassSessions
            .FirstOrDefaultAsync(cs => cs.TenantId == tenantId && cs.Id == classSessionId && !cs.IsDeleted);

        if (session == null || session.IsFull || session.Status == SessionStatus.Cancelled)
            return false;

        if (session.CurrentBookings + participantCount > session.MaxCapacity)
            return false;

        var booking = new ClassSessionBooking
        {
            ClassSessionId = classSessionId,
            AppointmentId = appointmentId,
            BookedAt = DateTime.UtcNow,
            ParticipantCount = participantCount
        };

        session.CurrentBookings += participantCount;
        if (session.CurrentBookings >= session.MaxCapacity)
            session.Status = SessionStatus.Full;

        _context.ClassSessionBookings.Add(booking);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelClassSessionBookingAsync(Guid tenantId, Guid classSessionId, Guid appointmentId)
    {
        var booking = await _context.ClassSessionBookings
            .Include(csb => csb.ClassSession)
            .FirstOrDefaultAsync(csb => csb.ClassSessionId == classSessionId && csb.AppointmentId == appointmentId
                && csb.ClassSession.TenantId == tenantId);

        if (booking == null) return false;

        var session = booking.ClassSession;
        session.CurrentBookings = Math.Max(0, session.CurrentBookings - booking.ParticipantCount);
        if (session.Status == SessionStatus.Full)
            session.Status = SessionStatus.Scheduled;

        _context.ClassSessionBookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}
