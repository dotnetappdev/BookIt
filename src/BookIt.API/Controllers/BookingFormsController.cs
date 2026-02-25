using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/booking-forms")]
public class BookingFormsController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;

    public BookingFormsController(BookItDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingForm>>> GetForms(string tenantSlug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        var forms = await _context.BookingForms
            .Include(f => f.Fields)
            .Where(f => f.TenantId == tenant.Id && f.IsActive && !f.IsDeleted)
            .ToListAsync();

        return Ok(forms);
    }

    [HttpGet("default")]
    public async Task<ActionResult<BookingForm>> GetDefaultForm(string tenantSlug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        var form = await _context.BookingForms
            .Include(f => f.Fields)
            .FirstOrDefaultAsync(f => f.TenantId == tenant.Id && f.IsDefault && f.IsActive && !f.IsDeleted);

        if (form == null) return NotFound();
        return Ok(form);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BookingForm>> CreateForm(string tenantSlug, [FromBody] BookingForm form)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        form.TenantId = tenant.Id;
        form.Id = Guid.NewGuid();
        form.CreatedAt = DateTime.UtcNow;

        _context.BookingForms.Add(form);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetForms), new { tenantSlug }, form);
    }
}
