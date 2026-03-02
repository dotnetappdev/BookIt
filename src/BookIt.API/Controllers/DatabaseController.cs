using BookIt.API.Services;
using BookIt.Core.DTOs;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseSeederService _seederService;

    public DatabaseController(IDatabaseSeederService seederService)
    {
        _seederService = seederService;
    }

    [HttpGet("status")]
    public async Task<ActionResult<object>> GetStatus()
    {
        var hasDemoData = await _seederService.HasDemoDataAsync();
        return Ok(new { hasDemoData });
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedDemoData()
    {
        try
        {
            await _seederService.SeedDemoDataAsync();
            return Ok(new { message = "Demo data seeded successfully" });
        }
        catch
        {
            return BadRequest(new { message = "Failed to seed demo data. Check server logs for details." });
        }
    }

    [HttpPost("clear")]
    public async Task<IActionResult> ClearDemoData()
    {
        try
        {
            await _seederService.ClearDemoDataAsync();
            return Ok(new { message = "Demo data cleared successfully" });
        }
        catch
        {
            return BadRequest(new { message = "Failed to clear demo data. Check server logs for details." });
        }
    }
}

/// <summary>
/// Tenant-scoped demo data endpoint accessible to TenantAdmin.
/// </summary>
[ApiController]
[Route("api/tenants/{tenantSlug}/demo-data")]
[Authorize(Policy = "TenantAdmin")]
public class TenantDemoDataController : ControllerBase
{
    private readonly IDatabaseSeederService _seederService;

    public TenantDemoDataController(IDatabaseSeederService seederService)
    {
        _seederService = seederService;
    }

    [HttpGet("status")]
    public async Task<ActionResult<DemoDataStatusResponse>> GetStatus(string tenantSlug)
    {
        var hasDemoData = await _seederService.HasTenantDemoDataAsync(tenantSlug);
        return Ok(new DemoDataStatusResponse { HasDemoData = hasDemoData });
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedTenantDemoData(string tenantSlug)
    {
        try
        {
            await _seederService.SeedTenantDemoDataAsync(tenantSlug);
            return Ok(new { message = "Demo data seeded successfully" });
        }
        catch
        {
            return BadRequest(new { message = "Failed to seed demo data. Check server logs for details." });
        }
    }

    [HttpPost("clear")]
    public async Task<IActionResult> ClearTenantDemoData(string tenantSlug)
    {
        try
        {
            await _seederService.ClearTenantDemoDataAsync(tenantSlug);
            return Ok(new { message = "Demo data cleared successfully" });
        }
        catch
        {
            return BadRequest(new { message = "Failed to clear demo data. Check server logs for details." });
        }
    }
}
