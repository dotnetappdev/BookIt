using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/admin/subscription-tiers")]
public class SubscriptionTiersController : ControllerBase
{
    private readonly BookItDbContext _context;

    public SubscriptionTiersController(BookItDbContext context)
    {
        _context = context;
    }

    /// <summary>List all subscription tiers (public — used on tenant subscription pages).</summary>
    [HttpGet]
    public async Task<ActionResult<List<SubscriptionTierResponse>>> GetAll()
    {
        var tiers = await _context.SubscriptionTiers
            .AsNoTracking()
            .OrderBy(t => t.SortOrder)
            .ToListAsync();

        return Ok(tiers.Select(MapResponse).ToList());
    }

    /// <summary>Get a single subscription tier by its plan.</summary>
    [HttpGet("by-plan/{plan}")]
    public async Task<ActionResult<SubscriptionTierResponse>> GetByPlan(SubscriptionPlan plan)
    {
        var tier = await _context.SubscriptionTiers.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Plan == plan && !t.IsDeleted);

        if (tier == null) return NotFound();
        return Ok(MapResponse(tier));
    }

    /// <summary>Get a single subscription tier by id.</summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin,TenantAdmin,Manager")]
    public async Task<ActionResult<SubscriptionTierResponse>> GetById(Guid id)
    {
        var tier = await _context.SubscriptionTiers.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (tier == null) return NotFound();
        return Ok(MapResponse(tier));
    }

    /// <summary>Create a new subscription tier.</summary>
    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<SubscriptionTierResponse>> Create([FromBody] UpsertSubscriptionTierRequest request)
    {
        // Each plan should be unique
        if (await _context.SubscriptionTiers.AnyAsync(t => t.Plan == request.Plan && !t.IsDeleted))
            return Conflict(new { message = $"A tier for plan '{request.Plan}' already exists." });

        var tier = MapFromRequest(request);
        _context.SubscriptionTiers.Add(tier);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = tier.Id }, MapResponse(tier));
    }

    /// <summary>Update an existing subscription tier.</summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<SubscriptionTierResponse>> Update(Guid id, [FromBody] UpsertSubscriptionTierRequest request)
    {
        var tier = await _context.SubscriptionTiers
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (tier == null) return NotFound();

        // Check no other tier uses this plan
        if (await _context.SubscriptionTiers.AnyAsync(t => t.Plan == request.Plan && t.Id != id && !t.IsDeleted))
            return Conflict(new { message = $"A tier for plan '{request.Plan}' already exists." });

        tier.Plan = request.Plan;
        tier.Name = request.Name;
        tier.Description = request.Description;
        tier.SortOrder = request.SortOrder;
        tier.IsActive = request.IsActive;
        tier.MonthlyPriceGbp = request.MonthlyPriceGbp;
        tier.MonthlyPriceUsd = request.MonthlyPriceUsd;
        tier.MonthlyPriceEur = request.MonthlyPriceEur;
        tier.MaxServices = request.MaxServices;
        tier.MaxStaff = request.MaxStaff;
        tier.MaxLocations = request.MaxLocations;
        tier.MaxBookingsPerMonth = request.MaxBookingsPerMonth;
        tier.CanUseOnlinePayments = request.CanUseOnlinePayments;
        tier.CanUseCustomForms = request.CanUseCustomForms;
        tier.CanUseAiAssistant = request.CanUseAiAssistant;
        tier.CanUseInterviews = request.CanUseInterviews;
        tier.CanUseApiAccess = request.CanUseApiAccess;
        tier.CanRemoveBranding = request.CanRemoveBranding;
        tier.CanUseMultipleStaff = request.CanUseMultipleStaff;
        tier.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapResponse(tier));
    }

    /// <summary>Soft-delete a subscription tier.</summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var tier = await _context.SubscriptionTiers
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (tier == null) return NotFound();

        tier.IsDeleted = true;
        tier.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static SubscriptionTierResponse MapResponse(SubscriptionTier t) => new()
    {
        Id = t.Id,
        Plan = t.Plan,
        Name = t.Name,
        Description = t.Description,
        SortOrder = t.SortOrder,
        IsActive = t.IsActive,
        MonthlyPriceGbp = t.MonthlyPriceGbp,
        MonthlyPriceUsd = t.MonthlyPriceUsd,
        MonthlyPriceEur = t.MonthlyPriceEur,
        MaxServices = t.MaxServices,
        MaxStaff = t.MaxStaff,
        MaxLocations = t.MaxLocations,
        MaxBookingsPerMonth = t.MaxBookingsPerMonth,
        CanUseOnlinePayments = t.CanUseOnlinePayments,
        CanUseCustomForms = t.CanUseCustomForms,
        CanUseAiAssistant = t.CanUseAiAssistant,
        CanUseInterviews = t.CanUseInterviews,
        CanUseApiAccess = t.CanUseApiAccess,
        CanRemoveBranding = t.CanRemoveBranding,
        CanUseMultipleStaff = t.CanUseMultipleStaff,
    };

    private static SubscriptionTier MapFromRequest(UpsertSubscriptionTierRequest r) => new()
    {
        Plan = r.Plan,
        Name = r.Name,
        Description = r.Description,
        SortOrder = r.SortOrder,
        IsActive = r.IsActive,
        MonthlyPriceGbp = r.MonthlyPriceGbp,
        MonthlyPriceUsd = r.MonthlyPriceUsd,
        MonthlyPriceEur = r.MonthlyPriceEur,
        MaxServices = r.MaxServices,
        MaxStaff = r.MaxStaff,
        MaxLocations = r.MaxLocations,
        MaxBookingsPerMonth = r.MaxBookingsPerMonth,
        CanUseOnlinePayments = r.CanUseOnlinePayments,
        CanUseCustomForms = r.CanUseCustomForms,
        CanUseAiAssistant = r.CanUseAiAssistant,
        CanUseInterviews = r.CanUseInterviews,
        CanUseApiAccess = r.CanUseApiAccess,
        CanRemoveBranding = r.CanRemoveBranding,
        CanUseMultipleStaff = r.CanUseMultipleStaff,
    };
}
