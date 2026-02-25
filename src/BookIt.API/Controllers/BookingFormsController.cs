using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

    private async Task<Tenant?> GetTenantAsync(string slug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);

    private static BookingFormResponse MapForm(BookingForm f) => new()
    {
        Id = f.Id,
        Name = f.Name,
        Description = f.Description,
        IsDefault = f.IsDefault,
        IsActive = f.IsActive,
        WelcomeMessage = f.WelcomeMessage,
        ConfirmationMessage = f.ConfirmationMessage,
        CollectPhone = f.CollectPhone,
        CollectNotes = f.CollectNotes,
        Fields = f.Fields
            .Where(ff => ff.IsActive && !ff.IsDeleted)
            .OrderBy(ff => ff.SortOrder)
            .Select(ff => new BookingFormFieldResponse
            {
                Id = ff.Id,
                Label = ff.Label,
                FieldName = ff.FieldName,
                FieldType = ff.FieldType,
                IsRequired = ff.IsRequired,
                IsActive = ff.IsActive,
                SortOrder = ff.SortOrder,
                Placeholder = ff.Placeholder,
                OptionsJson = ff.OptionsJson
            }).ToList()
    };

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingFormResponse>>> GetForms(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        var forms = await _context.BookingForms
            .Include(f => f.Fields)
            .Where(f => f.TenantId == tenant.Id && !f.IsDeleted)
            .OrderByDescending(f => f.IsDefault).ThenBy(f => f.Name)
            .ToListAsync();
        return Ok(forms.Select(MapForm));
    }

    [HttpGet("default")]
    public async Task<ActionResult<BookingFormResponse>> GetDefaultForm(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        var form = await _context.BookingForms
            .Include(f => f.Fields)
            .FirstOrDefaultAsync(f => f.TenantId == tenant.Id && f.IsDefault && f.IsActive && !f.IsDeleted);
        if (form == null) return NotFound();
        return Ok(MapForm(form));
    }

    [HttpGet("{formId}")]
    public async Task<ActionResult<BookingFormResponse>> GetForm(string tenantSlug, Guid formId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        var form = await _context.BookingForms
            .Include(f => f.Fields)
            .FirstOrDefaultAsync(f => f.Id == formId && f.TenantId == tenant.Id && !f.IsDeleted);
        if (form == null) return NotFound();
        return Ok(MapForm(form));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BookingFormResponse>> CreateForm(string tenantSlug, [FromBody] CreateBookingFormRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        if (request.IsDefault)
        {
            var existing = await _context.BookingForms.Where(f => f.TenantId == tenant.Id && f.IsDefault && !f.IsDeleted).ToListAsync();
            existing.ForEach(f => { f.IsDefault = false; f.UpdatedAt = DateTime.UtcNow; });
        }

        var form = new BookingForm
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Name = request.Name,
            Description = request.Description,
            IsDefault = request.IsDefault,
            IsActive = true,
            WelcomeMessage = request.WelcomeMessage,
            ConfirmationMessage = request.ConfirmationMessage,
            CollectPhone = request.CollectPhone,
            CollectNotes = request.CollectNotes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.BookingForms.Add(form);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetForm), new { tenantSlug, formId = form.Id }, MapForm(form));
    }

    [Authorize]
    [HttpDelete("{formId}")]
    public async Task<IActionResult> DeleteForm(string tenantSlug, Guid formId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();
        var form = await _context.BookingForms.FirstOrDefaultAsync(f => f.Id == formId && f.TenantId == tenant.Id && !f.IsDeleted);
        if (form == null) return NotFound();
        if (form.IsDefault) return BadRequest(new { message = "Cannot delete the default form" });
        form.IsDeleted = true;
        form.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Field endpoints ──

    [Authorize]
    [HttpPost("{formId}/fields")]
    public async Task<ActionResult<BookingFormFieldResponse>> AddField(string tenantSlug, Guid formId, [FromBody] AddFormFieldRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();
        var form = await _context.BookingForms.Include(f => f.Fields)
            .FirstOrDefaultAsync(f => f.Id == formId && f.TenantId == tenant.Id && !f.IsDeleted);
        if (form == null) return NotFound();

        var maxOrder = form.Fields.Any() ? form.Fields.Max(f => f.SortOrder) : -1;
        var field = new BookingFormField
        {
            Id = Guid.NewGuid(),
            BookingFormId = formId,
            Label = request.Label,
            FieldName = string.IsNullOrEmpty(request.FieldName)
                ? "field_" + Guid.NewGuid().ToString("N")[..8]
                : request.FieldName,
            FieldType = request.FieldType,
            IsRequired = request.IsRequired,
            IsActive = true,
            SortOrder = request.SortOrder > 0 ? request.SortOrder : maxOrder + 1,
            Placeholder = request.Placeholder,
            OptionsJson = request.OptionsJson,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.BookingFormFields.Add(field);
        await _context.SaveChangesAsync();
        return Ok(new BookingFormFieldResponse
        {
            Id = field.Id, Label = field.Label, FieldName = field.FieldName,
            FieldType = field.FieldType, IsRequired = field.IsRequired,
            IsActive = field.IsActive, SortOrder = field.SortOrder,
            Placeholder = field.Placeholder, OptionsJson = field.OptionsJson
        });
    }

    [Authorize]
    [HttpPut("{formId}/fields/{fieldId}")]
    public async Task<ActionResult<BookingFormFieldResponse>> UpdateField(string tenantSlug, Guid formId, Guid fieldId, [FromBody] UpdateFormFieldRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();
        var field = await _context.BookingFormFields
            .FirstOrDefaultAsync(f => f.Id == fieldId && f.BookingFormId == formId && !f.IsDeleted);
        if (field == null) return NotFound();
        field.Label = request.Label;
        field.FieldType = request.FieldType;
        field.IsRequired = request.IsRequired;
        field.Placeholder = request.Placeholder;
        field.OptionsJson = request.OptionsJson;
        field.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(new BookingFormFieldResponse
        {
            Id = field.Id, Label = field.Label, FieldName = field.FieldName,
            FieldType = field.FieldType, IsRequired = field.IsRequired,
            IsActive = field.IsActive, SortOrder = field.SortOrder,
            Placeholder = field.Placeholder, OptionsJson = field.OptionsJson
        });
    }

    [Authorize]
    [HttpDelete("{formId}/fields/{fieldId}")]
    public async Task<IActionResult> DeleteField(string tenantSlug, Guid formId, Guid fieldId)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();
        var field = await _context.BookingFormFields
            .FirstOrDefaultAsync(f => f.Id == fieldId && f.BookingFormId == formId && !f.IsDeleted);
        if (field == null) return NotFound();
        field.IsDeleted = true;
        field.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpPost("{formId}/fields/reorder")]
    public async Task<IActionResult> ReorderFields(string tenantSlug, Guid formId, [FromBody] ReorderFieldsRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();
        var fields = await _context.BookingFormFields
            .Where(f => f.BookingFormId == formId && !f.IsDeleted)
            .ToListAsync();
        for (int i = 0; i < request.FieldIds.Count; i++)
        {
            var field = fields.FirstOrDefault(f => f.Id == request.FieldIds[i]);
            if (field != null) { field.SortOrder = i; field.UpdatedAt = DateTime.UtcNow; }
        }
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
