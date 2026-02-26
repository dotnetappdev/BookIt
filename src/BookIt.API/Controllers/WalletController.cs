using BookIt.API.Services;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

/// <summary>
/// Generates native wallet passes for Apple Wallet (iOS) and Google Wallet (Android).
///
/// Requires platform-specific configuration — see AppleWalletService and GoogleWalletService.
/// When credentials are not configured, endpoints return 503 with setup instructions.
/// </summary>
[ApiController]
[Route("api/tenants/{tenantSlug}/appointments/{id:guid}/wallet")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly AppleWalletService _appleWallet;
    private readonly GoogleWalletService _googleWallet;

    public WalletController(
        BookItDbContext context,
        AppleWalletService appleWallet,
        GoogleWalletService googleWallet)
    {
        _context = context;
        _appleWallet = appleWallet;
        _googleWallet = googleWallet;
    }

    /// <summary>
    /// Returns a signed Apple Wallet .pkpass file for an appointment.
    /// Opened on iOS it shows "Add to Apple Wallet" prompt.
    /// </summary>
    [HttpGet("apple")]
    public async Task<IActionResult> GetApplePass(string tenantSlug, Guid id)
    {
        if (!_appleWallet.IsConfigured)
            return StatusCode(503, new
            {
                error = "Apple Wallet is not configured on this server.",
                setup = "Set AppleWallet:PassTypeIdentifier, AppleWallet:TeamIdentifier, AppleWallet:CertificateBase64, and AppleWallet:CertificatePassword in appsettings.json or user-secrets."
            });

        var (appointment, tenantName, membershipNumber) = await LoadPassDataAsync(tenantSlug, id);
        if (appointment == null)
            return NotFound();

        var passBytes = _appleWallet.GeneratePass(appointment, tenantName, membershipNumber);
        return File(passBytes, "application/vnd.apple.pkpass", $"bookit-{id:N}.pkpass");
    }

    /// <summary>
    /// Returns a Google Wallet "Add to Google Wallet" save URL for an appointment.
    /// Opening this URL on Android prompts the user to save the pass.
    /// </summary>
    [HttpGet("google")]
    public async Task<IActionResult> GetGoogleWalletUrl(string tenantSlug, Guid id)
    {
        if (!_googleWallet.IsConfigured)
            return StatusCode(503, new
            {
                error = "Google Wallet is not configured on this server.",
                setup = "Set GoogleWallet:IssuerId, GoogleWallet:ServiceAccountEmail, and GoogleWallet:PrivateKeyPem in appsettings.json or user-secrets."
            });

        var (appointment, tenantName, membershipNumber) = await LoadPassDataAsync(tenantSlug, id);
        if (appointment == null)
            return NotFound();

        var saveUrl = _googleWallet.GenerateSaveUrl(appointment, tenantName, tenantSlug, membershipNumber);
        return Ok(new { url = saveUrl });
    }

    // ── helpers ───────────────────────────────────────────────────────────────

    private async Task<(Core.DTOs.AppointmentResponse? appointment, string tenantName, string? membershipNumber)>
        LoadPassDataAsync(string tenantSlug, Guid appointmentId)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return (null, "", null);

        var apt = await _context.Appointments
            .Include(a => a.Services).ThenInclude(s => s.Service)
            .Include(a => a.Staff)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.TenantId == tenant.Id);
        if (apt == null) return (null, "", null);

        // Attempt to look up the customer's membership number
        var customer = await _context.Set<Core.Entities.Customer>()
            .FirstOrDefaultAsync(c => c.TenantId == tenant.Id && c.Email == apt.CustomerEmail);

        var response = new Core.DTOs.AppointmentResponse
        {
            Id = apt.Id,
            TenantId = apt.TenantId,
            CustomerName = apt.CustomerName,
            CustomerEmail = apt.CustomerEmail,
            CustomerPhone = apt.CustomerPhone,
            StartTime = apt.StartTime,
            EndTime = apt.EndTime,
            Status = apt.Status,
            PaymentStatus = apt.PaymentStatus,
            TotalAmount = apt.TotalAmount,
            MeetingType = apt.MeetingType,
            MeetingLink = apt.MeetingLink,
            ConfirmationToken = apt.ConfirmationToken,
            BookingPin = apt.BookingPin,
            StaffName = apt.Staff?.FullName,
            Services = apt.Services.Select(s => new Core.DTOs.ServiceSummary
            {
                Id = s.ServiceId,
                Name = s.Service?.Name ?? "",
                Price = s.PriceAtBooking,
                DurationMinutes = s.DurationAtBooking
            }).ToList()
        };

        return (response, tenant.Name, customer?.MembershipNumber);
    }
}
