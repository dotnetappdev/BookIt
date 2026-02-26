using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{slug}/email-templates")]
[Authorize]
public class EmailTemplatesController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;

    public EmailTemplatesController(BookItDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmailTemplateResponse>>> GetTemplates(string slug)
    {
        var tenant = await GetTenantAsync(slug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var templates = await _context.EmailTemplates
            .Where(t => t.TenantId == tenant.Id)
            .OrderBy(t => t.TemplateType)
            .ToListAsync();

        return Ok(templates.Select(Map));
    }

    [HttpPost]
    public async Task<ActionResult<EmailTemplateResponse>> CreateTemplate(
        string slug, [FromBody] UpsertEmailTemplateRequest request)
    {
        var tenant = await GetTenantAsync(slug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var template = new EmailTemplate
        {
            TenantId = tenant.Id,
            TemplateType = request.TemplateType,
            Name = request.Name,
            SubjectLine = request.SubjectLine,
            HtmlBody = request.HtmlBody,
            IsActive = request.IsActive,
        };

        _context.EmailTemplates.Add(template);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTemplate), new { slug, id = template.Id }, Map(template));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmailTemplateResponse>> GetTemplate(string slug, Guid id)
    {
        var tenant = await GetTenantAsync(slug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var template = await _context.EmailTemplates
            .FirstOrDefaultAsync(t => t.Id == id && t.TenantId == tenant.Id);
        if (template == null) return NotFound();

        return Ok(Map(template));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmailTemplateResponse>> UpdateTemplate(
        string slug, Guid id, [FromBody] UpsertEmailTemplateRequest request)
    {
        var tenant = await GetTenantAsync(slug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var template = await _context.EmailTemplates
            .FirstOrDefaultAsync(t => t.Id == id && t.TenantId == tenant.Id);
        if (template == null) return NotFound();

        template.TemplateType = request.TemplateType;
        template.Name = request.Name;
        template.SubjectLine = request.SubjectLine;
        template.HtmlBody = request.HtmlBody;
        template.IsActive = request.IsActive;
        template.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(Map(template));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(string slug, Guid id)
    {
        var tenant = await GetTenantAsync(slug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var template = await _context.EmailTemplates
            .FirstOrDefaultAsync(t => t.Id == id && t.TenantId == tenant.Id);
        if (template == null) return NotFound();

        template.IsDeleted = true;
        template.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task<Tenant?> GetTenantAsync(string slug) =>
        await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);

    private static EmailTemplateResponse Map(EmailTemplate t) => new()
    {
        Id = t.Id,
        TemplateType = t.TemplateType,
        Name = t.Name,
        SubjectLine = t.SubjectLine,
        HtmlBody = t.HtmlBody,
        IsActive = t.IsActive,
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt,
    };
}
