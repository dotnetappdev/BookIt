using BookIt.API.Services;
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
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to seed data: {ex.Message}" });
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
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Failed to clear data: {ex.Message}" });
        }
    }
}
