using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/paypal")]
public class PayPalController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly IPayPalService _payPalService;

    public PayPalController(BookItDbContext context, IPayPalService payPalService)
    {
        _context = context;
        _payPalService = payPalService;
    }

    /// <summary>Create a PayPal order for an appointment</summary>
    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder(string tenantSlug, [FromBody] CreatePayPalOrderRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

        if (tenant == null) return NotFound();

        if (request.TenantId != Guid.Empty && request.TenantId != tenant.Id) return Forbid();

        try
        {
            var orderId = await _payPalService.CreateOrderAsync(tenant.Id, request.AppointmentId, request.Amount, request.Currency ?? tenant.Currency ?? "GBP");
            return Ok(new { orderId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Capture a PayPal order after customer approves payment</summary>
    [HttpPost("capture-order")]
    public async Task<IActionResult> CaptureOrder(string tenantSlug, [FromBody] CapturePayPalOrderRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

        if (tenant == null) return NotFound();

        var success = await _payPalService.CaptureOrderAsync(tenant.Id, request.OrderId);
        if (!success) return BadRequest(new { message = "Payment capture failed" });

        return Ok(new { message = "Payment captured successfully" });
    }
}

public class CreatePayPalOrderRequest
{
    public Guid TenantId { get; set; }
    public Guid AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
}

public class CapturePayPalOrderRequest
{
    public string OrderId { get; set; } = string.Empty;
}
