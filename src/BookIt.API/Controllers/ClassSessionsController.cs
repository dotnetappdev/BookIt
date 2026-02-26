using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/class-sessions")]
public class ClassSessionsController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly IAppointmentService _appointmentService;
    private readonly ITenantService _tenantService;

    public ClassSessionsController(BookItDbContext context, IAppointmentService appointmentService, ITenantService tenantService)
    {
        _context = context;
        _appointmentService = appointmentService;
        _tenantService = tenantService;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    /// <summary>Get upcoming class sessions (gym classes, swimming sessions, etc.)</summary>
    [HttpGet]
    public async Task<IActionResult> GetSessions(string tenantSlug, [FromQuery] Guid? serviceId, [FromQuery] int days = 14)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var sessions = await _appointmentService.GetUpcomingClassSessionsAsync(tenant.Id, serviceId, days);

        return Ok(sessions.Select(s => new
        {
            s.Id,
            s.Name,
            s.Description,
            s.SessionDate,
            StartTime = s.StartTime.ToString("HH:mm"),
            s.DurationMinutes,
            s.MaxCapacity,
            s.CurrentBookings,
            s.SpotsRemaining,
            s.IsFull,
            s.Price,
            s.Status,
            s.Location,
            s.ClassType,
            ServiceId = s.ServiceId,
            ServiceName = s.Service?.Name,
            StaffId = s.StaffId,
            StaffName = s.Staff?.FullName,
            InstructorIds = s.InstructorIds
        }));
    }

    /// <summary>Get a specific class session</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSession(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var session = await _context.ClassSessions
            .Include(cs => cs.Service)
            .Include(cs => cs.Staff)
            .Include(cs => cs.Bookings)
            .FirstOrDefaultAsync(cs => cs.TenantId == tenant.Id && cs.Id == id && !cs.IsDeleted);

        if (session == null) return NotFound();

        return Ok(new
        {
            session.Id,
            session.Name,
            session.Description,
            session.SessionDate,
            StartTime = session.StartTime.ToString("HH:mm"),
            session.DurationMinutes,
            session.MaxCapacity,
            session.CurrentBookings,
            session.SpotsRemaining,
            session.IsFull,
            session.Price,
            session.Status,
            session.Location,
            session.ClassType,
            ServiceId = session.ServiceId,
            ServiceName = session.Service?.Name,
            StaffId = session.StaffId,
            StaffName = session.Staff?.FullName,
            InstructorIds = session.InstructorIds,
            BookingsCount = session.Bookings.Count
        });
    }

    /// <summary>Create a new class session (admin only)</summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSession(string tenantSlug, [FromBody] CreateClassSessionRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.TenantId == tenant.Id && s.Id == request.ServiceId && !s.IsDeleted);

        if (service == null) return BadRequest(new { message = "Service not found" });

        var session = new ClassSession
        {
            TenantId = tenant.Id,
            ServiceId = request.ServiceId,
            StaffId = request.InstructorIds.Any() ? request.InstructorIds.First() : request.StaffId,
            InstructorIdsJson = request.InstructorIds.Any()
                ? System.Text.Json.JsonSerializer.Serialize(request.InstructorIds)
                : null,
            Name = request.Name,
            Description = request.Description,
            SessionDate = request.SessionDate,
            StartTime = request.StartTime,
            DurationMinutes = request.DurationMinutes > 0 ? request.DurationMinutes : service.DurationMinutes,
            MaxCapacity = request.MaxCapacity > 0 ? request.MaxCapacity : service.MaxCapacity,
            Price = request.Price > 0 ? request.Price : service.Price,
            Location = request.Location,
            ClassType = request.ClassType ?? service.ClassType ?? ClassType.General,
            Status = SessionStatus.Scheduled
        };

        var created = await _appointmentService.CreateClassSessionAsync(session);
        return CreatedAtAction(nameof(GetSession), new { tenantSlug, id = created.Id },
            new { created.Id, created.Name, created.SessionDate, created.MaxCapacity, created.SpotsRemaining });
    }

    /// <summary>Book a spot in a class session</summary>
    [HttpPost("{id}/book")]
    public async Task<IActionResult> BookSession(string tenantSlug, Guid id, [FromBody] BookClassSessionRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var session = await _context.ClassSessions
            .Include(cs => cs.Service)
            .FirstOrDefaultAsync(cs => cs.TenantId == tenant.Id && cs.Id == id && !cs.IsDeleted);

        if (session == null) return NotFound();
        if (session.IsFull) return Conflict(new { message = "This session is fully booked" });

        var sessionStart = session.SessionDate.Date.Add(session.StartTime.ToTimeSpan());
        var appointment = new Appointment
        {
            TenantId = tenant.Id,
            StaffId = session.StaffId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            CustomerNotes = request.Notes,
            StartTime = sessionStart,
            EndTime = sessionStart.AddMinutes(session.DurationMinutes),
            TotalAmount = session.Price * request.ParticipantCount,
            Status = AppointmentStatus.Confirmed,
            MeetingType = MeetingType.InPerson,
            ConfirmationToken = Guid.NewGuid().ToString("N")[..12].ToUpper(),
            Services = new List<Core.Entities.AppointmentService>
            {
                new() { ServiceId = session.ServiceId, PriceAtBooking = session.Price, DurationAtBooking = session.DurationMinutes }
            }
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var booked = await _appointmentService.BookClassSessionAsync(tenant.Id, id, appointment.Id, request.ParticipantCount);
        if (!booked)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return Conflict(new { message = "Could not book this session - it may now be full" });
        }

        return Ok(new
        {
            appointment.Id,
            appointment.ConfirmationToken,
            SessionName = session.Name,
            session.SessionDate,
            StartTime = session.StartTime.ToString("HH:mm"),
            request.ParticipantCount,
            SpotsRemaining = session.SpotsRemaining,
            TotalAmount = appointment.TotalAmount
        });
    }

    /// <summary>Update a class session (admin only)</summary>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSession(string tenantSlug, Guid id, [FromBody] UpdateClassSessionRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var session = await _context.ClassSessions
            .Include(cs => cs.Service)
            .Include(cs => cs.Staff)
            .FirstOrDefaultAsync(cs => cs.TenantId == tenant.Id && cs.Id == id && !cs.IsDeleted);

        if (session == null) return NotFound();

        session.Name = request.Name;
        session.Description = request.Description;
        session.SessionDate = request.SessionDate;
        session.StartTime = request.StartTime;
        session.DurationMinutes = request.DurationMinutes > 0 ? request.DurationMinutes : session.DurationMinutes;
        session.MaxCapacity = request.MaxCapacity > 0 ? request.MaxCapacity : session.MaxCapacity;
        session.Price = request.Price >= 0 ? request.Price : session.Price;
        session.Location = request.Location;
        if (request.ClassType.HasValue) session.ClassType = request.ClassType.Value;
        if (request.InstructorIds.Any())
        {
            session.StaffId = request.InstructorIds.First();
            session.InstructorIdsJson = System.Text.Json.JsonSerializer.Serialize(request.InstructorIds);
        }
        session.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { session.Id, session.Name, session.SessionDate, session.MaxCapacity, InstructorIds = session.InstructorIds });
    }

    /// <summary>Cancel a class session (admin only)</summary>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelSession(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var session = await _context.ClassSessions
            .FirstOrDefaultAsync(cs => cs.TenantId == tenant.Id && cs.Id == id && !cs.IsDeleted);

        if (session == null) return NotFound();

        session.Status = SessionStatus.Cancelled;
        session.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class CreateClassSessionRequest
{
    public Guid ServiceId { get; set; }
    public Guid? StaffId { get; set; }
    public List<Guid> InstructorIds { get; set; } = new();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int MaxCapacity { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public ClassType? ClassType { get; set; }
}

public class UpdateClassSessionRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int MaxCapacity { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public ClassType? ClassType { get; set; }
    public List<Guid> InstructorIds { get; set; } = new();
}

public class BookClassSessionRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? Notes { get; set; }
    public int ParticipantCount { get; set; } = 1;
}
