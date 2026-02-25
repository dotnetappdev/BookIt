using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly IAppointmentService _appointmentService;
    private readonly ITenantService _tenantService;

    public AppointmentsController(BookItDbContext context, IAppointmentService appointmentService, ITenantService tenantService)
    {
        _context = context;
        _appointmentService = appointmentService;
        _tenantService = tenantService;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    [HttpGet("slots")]
    public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableSlots(
        string tenantSlug,
        [FromQuery] Guid serviceId,
        [FromQuery] Guid? staffId,
        [FromQuery] DateOnly date)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var slots = await _appointmentService.GetAvailableSlotsAsync(tenant.Id, serviceId, staffId, date);
        return Ok(slots);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentResponse>> CreateAppointment(
        string tenantSlug,
        [FromBody] CreateAppointmentRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var serviceIds = request.ServiceIds.Any() ? request.ServiceIds : new List<Guid> { request.ServiceId };
        var services = await _context.Services
            .Where(s => serviceIds.Contains(s.Id) && s.TenantId == tenant.Id && !s.IsDeleted)
            .ToListAsync();

        if (!services.Any()) return BadRequest(new { message = "No valid services found" });

        var totalMinutes = services.Sum(s => s.DurationMinutes);
        var totalAmount = services.Sum(s => s.Price);
        var startTime = request.StartTime;
        var endTime = startTime.AddMinutes(totalMinutes);

        var appointment = new Appointment
        {
            TenantId = tenant.Id,
            StaffId = request.StaffId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            CustomerNotes = request.CustomerNotes,
            StartTime = startTime,
            EndTime = endTime,
            TotalAmount = totalAmount,
            MeetingType = request.MeetingType,
            CustomFormDataJson = request.CustomFormDataJson,
            Status = AppointmentStatus.Pending,
            PaymentStatus = PaymentStatus.Unpaid,
            Services = services.Select(s => new Core.Entities.AppointmentService
            {
                ServiceId = s.Id,
                PriceAtBooking = s.Price,
                DurationAtBooking = s.DurationMinutes
            }).ToList()
        };

        if (request.MeetingType != MeetingType.InPerson && !string.IsNullOrEmpty(tenant.DefaultMeetingLink))
        {
            appointment.MeetingLink = tenant.DefaultMeetingLink;
        }

        var created = await _appointmentService.CreateAppointmentAsync(appointment);

        return CreatedAtAction(nameof(GetAppointment), new { tenantSlug, id = created.Id },
            MapToResponse(created, services));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentResponse>> GetAppointment(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var appointment = await _appointmentService.GetAppointmentAsync(tenant.Id, id);
        if (appointment == null) return NotFound();

        if (appointment.TenantId != tenant.Id)
            return Forbid();

        return Ok(MapToResponse(appointment, null));
    }

    [HttpGet("confirm/{token}")]
    public async Task<ActionResult<AppointmentResponse>> GetByConfirmationToken(string tenantSlug, string token)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var appointment = await _context.Appointments
            .Include(a => a.Services).ThenInclude(s => s.Service)
            .Include(a => a.Staff)
            .FirstOrDefaultAsync(a => a.TenantId == tenant.Id && a.ConfirmationToken == token);

        if (appointment == null) return NotFound();

        return Ok(MapToResponse(appointment, null));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments(
        string tenantSlug,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] AppointmentStatus? status)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var query = _context.Appointments
            .Include(a => a.Services).ThenInclude(s => s.Service)
            .Include(a => a.Staff)
            .Where(a => a.TenantId == tenant.Id && !a.IsDeleted);

        if (from.HasValue) query = query.Where(a => a.StartTime >= from.Value);
        if (to.HasValue) query = query.Where(a => a.StartTime <= to.Value);
        if (status.HasValue) query = query.Where(a => a.Status == status.Value);

        var appointments = await query.OrderByDescending(a => a.StartTime).ToListAsync();
        return Ok(appointments.Select(a => MapToResponse(a, null)));
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelAppointment(string tenantSlug, Guid id, [FromBody] string? reason)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        await _appointmentService.CancelAppointmentAsync(tenant.Id, id, reason);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmAppointment(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        await _appointmentService.ConfirmAppointmentAsync(tenant.Id, id);
        return NoContent();
    }

    private static AppointmentResponse MapToResponse(Appointment a, List<Service>? services)
    {
        return new AppointmentResponse
        {
            Id = a.Id,
            TenantId = a.TenantId,
            CustomerName = a.CustomerName,
            CustomerEmail = a.CustomerEmail,
            CustomerPhone = a.CustomerPhone,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            PaymentStatus = a.PaymentStatus,
            TotalAmount = a.TotalAmount,
            MeetingType = a.MeetingType,
            MeetingLink = a.MeetingLink,
            ConfirmationToken = a.ConfirmationToken,
            StaffName = a.Staff?.FullName,
            Services = a.Services.Select(s => new ServiceSummary
            {
                Id = s.ServiceId,
                Name = s.Service?.Name ?? (services?.FirstOrDefault(sv => sv.Id == s.ServiceId)?.Name ?? ""),
                Price = s.PriceAtBooking,
                DurationMinutes = s.DurationAtBooking
            }).ToList()
        };
    }
}
