using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Enums;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly BookItDbContext _context;

    public SubscriptionsController(BookItDbContext context)
    {
        _context = context;
    }

    /// <summary>Get all subscriptions for the tenant (most recent first).</summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<SubscriptionResponse>>> GetSubscriptions(string tenantSlug)
    {
        var tenant = await _context.Tenants.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        var subs = await _context.Subscriptions.AsNoTracking()
            .Where(s => s.TenantId == tenant.Id)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return Ok(subs.Select(MapResponse).ToList());
    }

    /// <summary>Get the current (most recent active/trialing) subscription.</summary>
    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult<SubscriptionResponse>> GetCurrentSubscription(string tenantSlug)
    {
        var tenant = await _context.Tenants.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        var sub = await _context.Subscriptions.AsNoTracking()
            .Where(s => s.TenantId == tenant.Id
                && (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Trialing))
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        if (sub == null)
        {
            // Return a synthesised Free/Trialing record when no persisted subscription exists
            return Ok(new SubscriptionResponse
            {
                Id = Guid.Empty,
                Plan = SubscriptionPlan.Free,
                Status = SubscriptionStatus.Trialing,
                MonthlyPrice = 0,
                Currency = tenant.Currency ?? "GBP",
                TrialEndsAt = DateTime.UtcNow.AddDays(14),
                CurrentPeriodStart = DateTime.UtcNow,
                CurrentPeriodEnd = DateTime.UtcNow.AddDays(14),
            });
        }

        return Ok(MapResponse(sub));
    }

    /// <summary>Select/upgrade a plan — creates or updates the subscription record.</summary>
    [HttpPost("select")]
    [Authorize]
    public async Task<ActionResult<SubscriptionResponse>> SelectPlan(
        string tenantSlug,
        [FromBody] SelectPlanRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        // Mark any existing active/trialing subscriptions as Cancelled
        var existing = await _context.Subscriptions
            .Where(s => s.TenantId == tenant.Id
                && (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Trialing))
            .ToListAsync();
        foreach (var e in existing)
        {
            e.Status = SubscriptionStatus.Cancelled;
            e.CancelledAt = DateTime.UtcNow;
        }

        var monthly = PlanMonthlyPrice(request.Plan, request.Currency ?? tenant.Currency ?? "GBP");
        var sub = new Subscription
        {
            TenantId = tenant.Id,
            Plan = request.Plan,
            Status = SubscriptionStatus.Active,
            PaymentProvider = request.PaymentProvider,
            MonthlyPrice = monthly,
            Currency = request.Currency ?? tenant.Currency ?? "GBP",
            CurrentPeriodStart = DateTime.UtcNow,
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1),
        };

        _context.Subscriptions.Add(sub);
        await _context.SaveChangesAsync();

        return Ok(MapResponse(sub));
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private static decimal PlanMonthlyPrice(SubscriptionPlan plan, string currency)
    {
        var gbpPrices = new Dictionary<SubscriptionPlan, decimal>
        {
            { SubscriptionPlan.Free,       0m },
            { SubscriptionPlan.Starter,    19m },
            { SubscriptionPlan.Pro,        49m },
            { SubscriptionPlan.Enterprise, 129m },
        };
        // NOTE: These are approximate display-only conversions.
        // For production billing, integrate a live FX rate service and store per-currency prices.
        var gbp = gbpPrices.GetValueOrDefault(plan, 0m);
        return currency.ToUpperInvariant() switch
        {
            "USD" => Math.Round(gbp * 1.27m, 2),
            "EUR" => Math.Round(gbp * 1.18m, 2),
            _     => gbp,  // default GBP
        };
    }

    private static SubscriptionResponse MapResponse(Subscription s) => new()
    {
        Id = s.Id,
        Plan = s.Plan,
        Status = s.Status,
        PaymentProvider = s.PaymentProvider,
        ProviderSubscriptionId = s.ProviderSubscriptionId,
        MonthlyPrice = s.MonthlyPrice,
        Currency = s.Currency,
        TrialEndsAt = s.TrialEndsAt,
        CurrentPeriodStart = s.CurrentPeriodStart,
        CurrentPeriodEnd = s.CurrentPeriodEnd,
        CancelledAt = s.CancelledAt,
        CancelAtPeriodEnd = s.CancelAtPeriodEnd,
        CreatedAt = s.CreatedAt,
    };
}
