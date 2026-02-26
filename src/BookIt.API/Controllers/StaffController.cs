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
public class StaffController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;

    public StaffController(BookItDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffResponse>>> GetStaff(string tenantSlug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        var staff = await _context.Staff
            .Include(s => s.Services).ThenInclude(ss => ss.Service)
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
            CreatedAt = DateTime.UtcNow
        };

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

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
        Services = s.Services.Select(ss => new StaffServiceItem { Id = ss.Service.Id, Name = ss.Service.Name }).ToList()
    };
}
