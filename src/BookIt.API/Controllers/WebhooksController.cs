using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[Authorize]
[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class WebhooksController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;

    public WebhooksController(BookItDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WebhookResponse>>> GetWebhooks(string tenantSlug)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var webhooks = await _context.Webhooks
            .Where(w => w.TenantId == tenant.Id)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();

        return Ok(webhooks.Select(MapToResponse));
    }

    [HttpPost]
    public async Task<ActionResult<WebhookResponse>> CreateWebhook(string tenantSlug, [FromBody] CreateWebhookRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var webhook = new Webhook
        {
            TenantId = tenant.Id,
            Url = request.Url,
            Secret = request.Secret,
            Events = request.Events,
            IsActive = true
        };

        _context.Webhooks.Add(webhook);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWebhook), new { tenantSlug, id = webhook.Id }, MapToResponse(webhook));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WebhookResponse>> GetWebhook(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var webhook = await _context.Webhooks.FirstOrDefaultAsync(w => w.Id == id && w.TenantId == tenant.Id);
        if (webhook == null) return NotFound();

        return Ok(MapToResponse(webhook));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WebhookResponse>> UpdateWebhook(string tenantSlug, Guid id, [FromBody] UpdateWebhookRequest request)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var webhook = await _context.Webhooks.FirstOrDefaultAsync(w => w.Id == id && w.TenantId == tenant.Id);
        if (webhook == null) return NotFound();

        webhook.Url = request.Url;
        webhook.Secret = request.Secret;
        webhook.Events = request.Events;
        webhook.IsActive = request.IsActive;
        webhook.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToResponse(webhook));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWebhook(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var webhook = await _context.Webhooks.FirstOrDefaultAsync(w => w.Id == id && w.TenantId == tenant.Id);
        if (webhook == null) return NotFound();

        webhook.IsDeleted = true;
        webhook.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/deliveries")]
    public async Task<ActionResult<IEnumerable<WebhookDeliveryResponse>>> GetDeliveries(string tenantSlug, Guid id)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        if (!_tenantService.IsValidTenantAccess(tenant.Id)) return Forbid();

        var deliveries = await _context.WebhookDeliveries
            .Where(d => d.WebhookId == id && d.Webhook.TenantId == tenant.Id)
            .OrderByDescending(d => d.DeliveredAt)
            .Take(50)
            .ToListAsync();

        return Ok(deliveries.Select(d => new WebhookDeliveryResponse
        {
            Id = d.Id,
            WebhookId = d.WebhookId,
            Event = d.Event,
            StatusCode = d.StatusCode,
            Success = d.Success,
            AttemptCount = d.AttemptCount,
            DeliveredAt = d.DeliveredAt
        }));
    }

    private static WebhookResponse MapToResponse(Webhook w) => new()
    {
        Id = w.Id,
        TenantId = w.TenantId,
        Url = w.Url,
        Events = w.Events,
        IsActive = w.IsActive,
        CreatedAt = w.CreatedAt
    };
}
