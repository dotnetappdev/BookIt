using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
[Authorize]
public class AuditTrailController : ControllerBase
{
    private readonly BookItDbContext _context;

    public AuditTrailController(BookItDbContext context)
    {
        _context = context;
    }

    private async Task<Tenant?> GetTenantAsync(string tenantSlug)
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

    [HttpGet]
    public async Task<ActionResult<PagedResult<AuditLogResponse>>> GetAuditLogs(
        string tenantSlug,
        [FromQuery] AuditLogQueryParams query)
    {
        var tenant = await GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var q = _context.AuditLogs
            .Where(a => a.TenantId == tenant.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.EntityName))
            q = q.Where(a => a.EntityName == query.EntityName);

        if (!string.IsNullOrWhiteSpace(query.Action))
            q = q.Where(a => a.Action == query.Action);

        if (!string.IsNullOrWhiteSpace(query.ChangedBy))
            q = q.Where(a => a.ChangedBy != null && a.ChangedBy.Contains(query.ChangedBy));

        if (query.From.HasValue)
            q = q.Where(a => a.ChangedAt >= query.From.Value);

        if (query.To.HasValue)
            q = q.Where(a => a.ChangedAt <= query.To.Value);

        var totalCount = await q.CountAsync();
        var items = await q
            .OrderByDescending(a => a.ChangedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(a => new AuditLogResponse
            {
                Id = a.Id,
                TenantId = a.TenantId,
                EntityName = a.EntityName,
                EntityId = a.EntityId,
                Action = a.Action,
                ChangedBy = a.ChangedBy,
                ChangedAt = a.ChangedAt,
                OldValues = a.OldValues,
                NewValues = a.NewValues
            })
            .ToListAsync();

        return Ok(new PagedResult<AuditLogResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        });
    }
}
