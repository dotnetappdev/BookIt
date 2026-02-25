using BookIt.Core.DTOs;
using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly IPaymentService _paymentService;

    public PaymentsController(BookItDbContext context, IPaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    [HttpPost("create-intent")]
    public async Task<ActionResult<PaymentIntentResponse>> CreatePaymentIntent(
        string tenantSlug,
        [FromBody] CreatePaymentIntentRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

        if (tenant == null) return NotFound();

        if (request.TenantId != tenant.Id)
            return Forbid();

        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.TenantId == tenant.Id && a.Id == request.AppointmentId);

        if (appointment == null) return NotFound();

        var clientSecret = await _paymentService.CreatePaymentIntentAsync(
            tenant.Id,
            request.AppointmentId,
            appointment.TotalAmount,
            tenant.Currency ?? "GBP",
            request.Provider);

        return Ok(new PaymentIntentResponse
        {
            ClientSecret = clientSecret,
            PaymentIntentId = clientSecret,
            Amount = appointment.TotalAmount,
            Currency = tenant.Currency ?? "GBP",
            Provider = request.Provider
        });
    }

    [HttpPost("stripe/webhook")]
    public async Task<IActionResult> StripeWebhook(string tenantSlug)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);

        if (tenant == null) return NotFound();

        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            await _paymentService.ConfirmPaymentAsync(tenant.Id, json, PaymentProvider.Stripe);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
