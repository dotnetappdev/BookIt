using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class InterviewSlotsController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InterviewSlotsController(BookItDbContext context, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    // GET available slots for a tenant (public — used by candidate form)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InterviewSlotResponse>>> GetSlots(string tenantSlug, [FromQuery] Guid? serviceId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var query = _context.InterviewSlots
            .Include(s => s.Service)
            .Where(s => s.TenantId == tenant.Id && !s.IsBooked && s.SlotStart > DateTime.UtcNow && !s.IsDeleted);

        if (serviceId.HasValue)
            query = query.Where(s => s.ServiceId == serviceId.Value);

        var slots = await query.OrderBy(s => s.SlotStart).ToListAsync();

        return Ok(slots.Select(MapSlot));
    }

    // POST create a slot (admin)
    [HttpPost]
    [Authorize(Policy = "TenantAdmin")]
    public async Task<ActionResult<InterviewSlotResponse>> CreateSlot(string tenantSlug, [FromBody] CreateInterviewSlotRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId && s.TenantId == tenant.Id);
        if (service == null) return BadRequest(new { message = "Position not found" });

        Staff? staff = null;
        if (request.StaffId.HasValue)
            staff = await _context.Staff.FirstOrDefaultAsync(s => s.Id == request.StaffId.Value && s.TenantId == tenant.Id);

        var slot = new InterviewSlot
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            ServiceId = request.ServiceId,
            StaffId = request.StaffId,
            InterviewerName = staff?.FullName ?? request.InterviewerName,
            SlotStart = request.SlotStart,
            SlotEnd = request.SlotStart.AddMinutes(request.DurationMinutes),
            Location = request.Location,
            MeetingLink = request.MeetingLink,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.InterviewSlots.Add(slot);
        await _context.SaveChangesAsync();

        // Reload with service
        await _context.Entry(slot).Reference(s => s.Service).LoadAsync();
        return CreatedAtAction(nameof(GetSlots), new { tenantSlug }, MapSlot(slot));
    }

    // DELETE slot (admin)
    [HttpDelete("{slotId}")]
    [Authorize(Policy = "TenantAdmin")]
    public async Task<IActionResult> DeleteSlot(string tenantSlug, Guid slotId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var slot = await _context.InterviewSlots.FirstOrDefaultAsync(s => s.Id == slotId && s.TenantId == tenant.Id);
        if (slot == null) return NotFound();

        slot.IsDeleted = true;
        slot.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST send invitation to candidate (admin)
    [HttpPost("invitations")]
    [Authorize(Policy = "TenantAdmin")]
    public async Task<ActionResult<CandidateInvitationResponse>> SendInvitation(string tenantSlug, [FromBody] SendInvitationRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId && s.TenantId == tenant.Id);
        if (service == null) return BadRequest(new { message = "Position not found" });

        var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N")[..8];
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var invitation = new CandidateInvitation
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            ServiceId = request.ServiceId,
            CandidateName = request.CandidateName,
            CandidateEmail = request.CandidateEmail,
            CandidatePhone = request.CandidatePhone,
            Token = token,
            ExpiresAt = expiresAt,
            InvitedBy = User?.Identity?.Name ?? "Admin",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.CandidateInvitations.Add(invitation);
        await _context.SaveChangesAsync();

        // Build booking URL
        var httpRequest = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = httpRequest != null ? $"{httpRequest.Scheme}://{httpRequest.Host}" : "http://localhost:5040";
        var bookingUrl = $"{baseUrl}/{tenantSlug}/interview/{token}";

        await _emailService.SendInterviewInvitationAsync(
            request.CandidateEmail, request.CandidateName,
            tenant.Name, service.Name, bookingUrl, expiresAt);

        return Ok(new CandidateInvitationResponse
        {
            Id = invitation.Id,
            Token = token,
            CandidateName = request.CandidateName,
            CandidateEmail = request.CandidateEmail,
            PositionName = service.Name,
            ExpiresAt = expiresAt,
            IsUsed = false,
            BookingUrl = bookingUrl
        });
    }

    // GET invitation details by token (public — used by candidate form)
    [HttpGet("invitations/{token}")]
    public async Task<ActionResult<InvitationDetailResponse>> GetInvitation(string tenantSlug, string token)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var invitation = await _context.CandidateInvitations
            .Include(i => i.Service)
            .FirstOrDefaultAsync(i => i.Token == token && i.TenantId == tenant.Id && !i.IsDeleted);

        if (invitation == null) return NotFound(new { message = "Invitation not found" });

        var availableSlots = await _context.InterviewSlots
            .Include(s => s.Service)
            .Where(s => s.TenantId == tenant.Id && s.ServiceId == invitation.ServiceId
                        && !s.IsBooked && s.SlotStart > DateTime.UtcNow && !s.IsDeleted)
            .OrderBy(s => s.SlotStart)
            .ToListAsync();

        return Ok(new InvitationDetailResponse
        {
            Id = invitation.Id,
            Token = token,
            CandidateName = invitation.CandidateName,
            CandidateEmail = invitation.CandidateEmail,
            ServiceId = invitation.ServiceId,
            PositionName = invitation.Service?.Name ?? "",
            PositionDescription = invitation.Service?.Description,
            TenantName = tenant.Name,
            TenantLogoUrl = tenant.LogoUrl,
            TenantAddress = tenant.Address,
            TenantSlug = tenant.Slug,
            IsExpired = invitation.ExpiresAt < DateTime.UtcNow,
            IsUsed = invitation.IsUsed,
            AvailableSlots = availableSlots.Select(MapSlot).ToList()
        });
    }

    // GET list of invitations (admin)
    [HttpGet("invitations")]
    [Authorize(Policy = "TenantAdmin")]
    public async Task<ActionResult<IEnumerable<CandidateInvitationResponse>>> GetInvitations(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var invitations = await _context.CandidateInvitations
            .Include(i => i.Service)
            .Where(i => i.TenantId == tenant.Id && !i.IsDeleted)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        var httpRequest = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = httpRequest != null ? $"{httpRequest.Scheme}://{httpRequest.Host}" : "http://localhost:5040";

        return Ok(invitations.Select(i => new CandidateInvitationResponse
        {
            Id = i.Id,
            Token = i.Token,
            CandidateName = i.CandidateName,
            CandidateEmail = i.CandidateEmail,
            PositionName = i.Service?.Name ?? "",
            ExpiresAt = i.ExpiresAt,
            IsUsed = i.IsUsed,
            BookingUrl = $"{baseUrl}/{tenantSlug}/interview/{i.Token}"
        }));
    }

    // POST book a slot via invitation token (public — candidate submits form)
    [HttpPost("invitations/{token}/book")]
    public async Task<ActionResult> BookSlot(string tenantSlug, string token, [FromBody] BookInterviewRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var invitation = await _context.CandidateInvitations
            .Include(i => i.Service)
            .FirstOrDefaultAsync(i => i.Token == token && i.TenantId == tenant.Id && !i.IsDeleted);

        if (invitation == null) return NotFound(new { message = "Invitation not found" });
        if (invitation.IsUsed) return BadRequest(new { message = "This invitation has already been used" });
        if (invitation.ExpiresAt < DateTime.UtcNow) return BadRequest(new { message = "This invitation has expired" });

        var slot = await _context.InterviewSlots
            .Include(s => s.Service)
            .FirstOrDefaultAsync(s => s.Id == request.SlotId && s.TenantId == tenant.Id && !s.IsBooked && !s.IsDeleted);

        if (slot == null) return BadRequest(new { message = "This slot is no longer available" });

        // Book the slot
        slot.IsBooked = true;
        slot.BookedByInvitationId = invitation.Id;
        slot.UpdatedAt = DateTime.UtcNow;

        // Update invitation
        invitation.IsUsed = true;
        invitation.BookedSlotId = slot.Id;
        invitation.CandidateName = request.CandidateName;
        invitation.CandidateEmail = request.CandidateEmail;
        invitation.CandidatePhone = request.CandidatePhone;
        invitation.LinkedInUrl = request.LinkedInUrl;
        invitation.Notes = request.Notes;
        invitation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Send confirmation email
        await _emailService.SendInterviewConfirmationAsync(
            request.CandidateEmail, request.CandidateName,
            tenant.Name, slot.Service?.Name ?? invitation.Service?.Name ?? "",
            slot.SlotStart, slot.Location, slot.MeetingLink);

        return Ok(new { message = "Interview scheduled successfully", slotStart = slot.SlotStart, location = slot.Location, meetingLink = slot.MeetingLink });
    }

    private static InterviewSlotResponse MapSlot(InterviewSlot s) => new()
    {
        Id = s.Id,
        ServiceId = s.ServiceId,
        PositionName = s.Service?.Name ?? "",
        InterviewerName = s.InterviewerName,
        SlotStart = s.SlotStart,
        SlotEnd = s.SlotEnd,
        IsBooked = s.IsBooked,
        Location = s.Location,
        MeetingLink = s.MeetingLink
    };
}
