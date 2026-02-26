using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using BookIt.Notifications.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class StaffController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailNotificationService _emailService;
    private readonly IConfiguration _configuration;

    public StaffController(
        BookItDbContext context, 
        ITenantService tenantService,
        UserManager<ApplicationUser> userManager,
        IEmailNotificationService emailService,
        IConfiguration configuration)
    {
        _context = context;
        _tenantService = tenantService;
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffResponse>>> GetStaff(string tenantSlug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        var staff = await _context.Staff
            .Include(s => s.Services).ThenInclude(ss => ss.Service)
            .Include(s => s.Client)
            .Where(s => s.TenantId == tenant.Id && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.SortOrder)
            .ToListAsync();

        return Ok(staff.Select(MapToResponse));
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<StaffResponse>>> GetAllStaff(string tenantSlug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var staff = await _context.Staff
            .Include(s => s.Services).ThenInclude(ss => ss.Service)
            .Include(s => s.Client)
            .Where(s => s.TenantId == tenant.Id && !s.IsDeleted)
            .OrderBy(s => s.SortOrder)
            .ToListAsync();

        return Ok(staff.Select(MapToResponse));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<StaffResponse>> GetStaffById(string tenantSlug, Guid id)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var staff = await _context.Staff
            .Include(s => s.Services).ThenInclude(ss => ss.Service)
            .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenant.Id && !s.IsDeleted);

        if (staff == null) return NotFound();
        return Ok(MapToResponse(staff));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<StaffResponse>> CreateStaff(string tenantSlug, [FromBody] CreateStaffRequest request)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new { message = "Email is required" });

        if (string.IsNullOrWhiteSpace(request.Phone))
            return BadRequest(new { message = "Phone is required" });

        var staff = new Staff
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            PhotoUrl = request.PhotoUrl,
            Bio = request.Bio,
            IsActive = request.IsActive,
            SortOrder = request.SortOrder,
            ClientId = request.ClientId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        if (request.SendInvite)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var invitation = new StaffInvitation
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                StaffId = staff.Id,
                Email = request.Email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.StaffInvitations.Add(invitation);
            await _context.SaveChangesAsync();

            try
            {
                var sendGridKey = _configuration["SendGrid:ApiKey"] ?? "";
                var fromEmail = tenant.ContactEmail ?? "noreply@bookit.app";
                var fromName = tenant.Name;
                var inviteLink = $"{_configuration["AppUrl"]}/staff-invite/{token}";

                await _emailService.SendStaffInvitationAsync(
                    sendGridKey, fromEmail, fromName, request.Email,
                    staff.FullName, tenant.Name, inviteLink, invitation.ExpiresAt);
            }
            catch (Exception ex)
            {
                // Log but don't fail the request
                Console.WriteLine($"Failed to send staff invitation email: {ex.Message}");
            }
        }

        return CreatedAtAction(nameof(GetStaffById), new { tenantSlug, id = staff.Id }, MapToResponse(staff));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<StaffResponse>> UpdateStaff(string tenantSlug, Guid id, [FromBody] UpdateStaffRequest request)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var staff = await _context.Staff
            .Include(s => s.Services).ThenInclude(ss => ss.Service)
            .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenant.Id && !s.IsDeleted);

        if (staff == null) return NotFound();

        staff.FirstName = request.FirstName;
        staff.LastName = request.LastName;
        staff.Email = request.Email;
        staff.Phone = request.Phone;
        staff.PhotoUrl = request.PhotoUrl;
        staff.Bio = request.Bio;
        staff.IsActive = request.IsActive;
        staff.SortOrder = request.SortOrder;
        staff.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToResponse(staff));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(string tenantSlug, Guid id)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var staff = await _context.Staff
            .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenant.Id && !s.IsDeleted);

        if (staff == null) return NotFound();

        staff.IsDeleted = true;
        staff.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}/services")]
    public async Task<IActionResult> AssignServices(string tenantSlug, Guid id, [FromBody] AssignStaffServicesRequest request)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var staff = await _context.Staff
            .Include(s => s.Services)
            .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenant.Id && !s.IsDeleted);

        if (staff == null) return NotFound();

        // Remove existing service assignments
        _context.Set<StaffService>().RemoveRange(staff.Services);

        // Add new assignments (only services belonging to this tenant)
        var validServiceIds = await _context.Services
            .Where(s => s.TenantId == tenant.Id && !s.IsDeleted && request.ServiceIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();

        foreach (var serviceId in validServiceIds)
        {
            _context.Set<StaffService>().Add(new StaffService { StaffId = id, ServiceId = serviceId });
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("api/tenants/staff/accept-invitation")]
    public async Task<ActionResult<AuthResponse>> AcceptInvitation([FromBody] AcceptStaffInvitationRequest request)
    {
        var invitation = await _context.StaffInvitations
            .Include(i => i.Staff)
            .Include(i => i.Tenant)
            .FirstOrDefaultAsync(i => i.Token == request.Token && !i.IsDeleted);

        if (invitation == null || invitation.IsUsed || invitation.ExpiresAt < DateTime.UtcNow)
            return BadRequest(new { message = "Invalid or expired invitation" });

        var staff = invitation.Staff;
        var tenant = invitation.Tenant;

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Email = staff.Email,
            UserName = staff.Email,
            FirstName = staff.FirstName,
            LastName = staff.LastName,
            PhoneNumber = staff.Phone,
            Role = UserRole.Staff,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

        await _userManager.AddToRoleAsync(user, "Staff");

        staff.UserId = user.Id;
        invitation.IsUsed = true;
        invitation.UsedAt = DateTime.UtcNow;
        invitation.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Account created successfully. You can now sign in." });
    }

    [HttpGet("api/tenants/staff/invitation/{token}")]
    public async Task<ActionResult<StaffInvitationResponse>> GetInvitation(string token)
    {
        var invitation = await _context.StaffInvitations
            .Include(i => i.Staff)
            .Include(i => i.Tenant)
            .FirstOrDefaultAsync(i => i.Token == token && !i.IsDeleted);

        if (invitation == null || invitation.IsUsed || invitation.ExpiresAt < DateTime.UtcNow)
            return BadRequest(new { message = "Invalid or expired invitation" });

        return Ok(new StaffInvitationResponse
        {
            Id = invitation.Id,
            Token = invitation.Token,
            Email = invitation.Email,
            StaffName = invitation.Staff.FullName,
            ExpiresAt = invitation.ExpiresAt,
            IsUsed = invitation.IsUsed
        });
    }

    private static StaffResponse MapToResponse(Staff s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        LastName = s.LastName,
        Email = s.Email,
        Phone = s.Phone,
        PhotoUrl = s.PhotoUrl,
        Bio = s.Bio,
        IsActive = s.IsActive,
        SortOrder = s.SortOrder,
        ClientId = s.ClientId,
        ClientName = s.Client?.CompanyName,
        Services = s.Services.Select(ss => new StaffServiceItem { Id = ss.Service.Id, Name = ss.Service.Name }).ToList()
    };
}
