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
    public async Task<ActionResult<IEnumerable<Staff>>> GetStaff(string tenantSlug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        var staff = await _context.Staff
            .Include(s => s.Services).ThenInclude(ss => ss.Service)
            .Where(s => s.TenantId == tenant.Id && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.SortOrder)
            .ToListAsync();

        return Ok(staff.Select(s => new
        {
            s.Id,
            s.FirstName,
            s.LastName,
            s.FullName,
            s.Email,
            s.Phone,
            s.PhotoUrl,
            s.Bio,
            Services = s.Services.Select(ss => new { ss.Service.Id, ss.Service.Name })
        }));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateStaff(string tenantSlug, [FromBody] Staff staff)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        staff.TenantId = tenant.Id;
        staff.Id = Guid.NewGuid();
        staff.CreatedAt = DateTime.UtcNow;

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStaff), new { tenantSlug }, staff);
    }
}
