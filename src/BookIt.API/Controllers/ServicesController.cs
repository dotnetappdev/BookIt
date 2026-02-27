using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Helpers;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;

    public ServicesController(BookItDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceResponse>>> GetServices(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var services = await _context.Services
            .Include(s => s.Category)
            .Where(s => s.TenantId == tenant.Id && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.Category!.SortOrder).ThenBy(s => s.SortOrder)
            .Select(s => new ServiceResponse
            {
                Id = s.Id,
                TenantId = s.TenantId,
                Name = s.Name,
                Slug = s.Slug,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes,
                CategoryName = s.Category != null ? s.Category.Name : null,
                AllowOnlineBooking = s.AllowOnlineBooking,
                DefaultMeetingType = s.DefaultMeetingType
            })
            .ToListAsync();

        return Ok(services);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse>> GetService(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var s = await _context.Services.Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.TenantId == tenant.Id && s.Id == id && !s.IsDeleted);

        if (s == null) return NotFound();

        return Ok(new ServiceResponse
        {
            Id = s.Id,
            TenantId = s.TenantId,
            Name = s.Name,
            Slug = s.Slug,
            Description = s.Description,
            ImageUrl = s.ImageUrl,
            Price = s.Price,
            DurationMinutes = s.DurationMinutes,
            CategoryName = s.Category?.Name,
            AllowOnlineBooking = s.AllowOnlineBooking,
            DefaultMeetingType = s.DefaultMeetingType
        });
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<ServiceResponse>> GetServiceBySlug(string tenantSlug, string slug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var s = await _context.Services.Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.TenantId == tenant.Id && s.Slug == slug && s.IsActive && !s.IsDeleted);

        if (s == null) return NotFound();

        return Ok(new ServiceResponse
        {
            Id = s.Id,
            TenantId = s.TenantId,
            Name = s.Name,
            Slug = s.Slug,
            Description = s.Description,
            ImageUrl = s.ImageUrl,
            Price = s.Price,
            DurationMinutes = s.DurationMinutes,
            CategoryName = s.Category?.Name,
            AllowOnlineBooking = s.AllowOnlineBooking,
            DefaultMeetingType = s.DefaultMeetingType
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> CreateService(string tenantSlug, [FromBody] CreateServiceRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var baseSlug = !string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.GenerateSlug(request.Slug)
            : SlugHelper.GenerateSlug(request.Name);

        // Ensure slug uniqueness within the tenant
        var uniqueSlug = baseSlug;
        var suffix = 1;
        while (await _context.Services.AnyAsync(s => s.TenantId == tenant.Id && s.Slug == uniqueSlug && !s.IsDeleted))
            uniqueSlug = $"{baseSlug}-{++suffix}";

        var service = new Service
        {
            TenantId = tenant.Id,
            Name = request.Name,
            Slug = uniqueSlug,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            DurationMinutes = request.DurationMinutes,
            BufferMinutes = request.BufferMinutes,
            CategoryId = request.CategoryId,
            AllowOnlineBooking = request.AllowOnlineBooking,
            DefaultMeetingType = request.DefaultMeetingType
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetService), new { tenantSlug, id = service.Id }, new ServiceResponse
        {
            Id = service.Id,
            TenantId = service.TenantId,
            Name = service.Name,
            Slug = service.Slug,
            Description = service.Description,
            Price = service.Price,
            DurationMinutes = service.DurationMinutes,
            AllowOnlineBooking = service.AllowOnlineBooking,
            DefaultMeetingType = service.DefaultMeetingType
        });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(string tenantSlug, Guid id, [FromBody] CreateServiceRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.TenantId == tenant.Id && s.Id == id && !s.IsDeleted);

        if (service == null) return NotFound();

        service.Name = request.Name;

        // If a new slug is explicitly provided, apply it; otherwise generate from name when the service has no slug yet
        string? desiredBaseSlug = null;
        if (!string.IsNullOrWhiteSpace(request.Slug))
            desiredBaseSlug = SlugHelper.GenerateSlug(request.Slug);
        else if (service.Slug == null)
            desiredBaseSlug = SlugHelper.GenerateSlug(request.Name);

        if (desiredBaseSlug != null)
        {
            var uniqueSlug = desiredBaseSlug;
            var suffix = 1;
            while (await _context.Services.AnyAsync(s => s.TenantId == tenant.Id && s.Slug == uniqueSlug && s.Id != id && !s.IsDeleted))
                uniqueSlug = $"{desiredBaseSlug}-{++suffix}";
            service.Slug = uniqueSlug;
        }

        service.Description = request.Description;
        service.ImageUrl = request.ImageUrl;
        service.Price = request.Price;
        service.DurationMinutes = request.DurationMinutes;
        service.BufferMinutes = request.BufferMinutes;
        service.CategoryId = request.CategoryId;
        service.AllowOnlineBooking = request.AllowOnlineBooking;
        service.DefaultMeetingType = request.DefaultMeetingType;
        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.TenantId == tenant.Id && s.Id == id && !s.IsDeleted);

        if (service == null) return NotFound();

        service.IsDeleted = true;
        service.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
