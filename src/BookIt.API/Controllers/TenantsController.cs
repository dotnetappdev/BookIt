using System.IO.Compression;
using System.Text;
using BookIt.Core.DTOs;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants")]
public class TenantsController : ControllerBase
{
    private readonly BookItDbContext _context;
    private readonly ITenantService _tenantService;

    public TenantsController(BookItDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    [HttpGet("by-subdomain/{subdomain}")]
    public async Task<ActionResult<object>> GetTenantBySubdomain(string subdomain)
    {
        if (string.IsNullOrWhiteSpace(subdomain))
            return BadRequest();

        var normalized = subdomain.ToLowerInvariant().Trim();
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Subdomain == normalized
                                   && t.SubdomainApproved
                                   && !t.IsDeleted
                                   && t.IsActive);

        if (tenant == null) return NotFound();

        return Ok(new { slug = tenant.Slug });
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<TenantResponse>> GetTenant(string slug)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted && t.IsActive);

        if (tenant == null) return NotFound();

        return Ok(MapToResponse(tenant));
    }

    [Authorize]
    [HttpPut("{slug}")]
    public async Task<ActionResult<TenantResponse>> UpdateTenant(string slug, [FromBody] UpdateTenantRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);

        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        tenant.Name = request.Name;
        tenant.BusinessType = request.BusinessType;
        tenant.CustomBusinessType = request.CustomBusinessType;
        tenant.LogoUrl = request.LogoUrl;
        tenant.PrimaryColor = request.PrimaryColor;
        tenant.SecondaryColor = request.SecondaryColor;
        tenant.Theme = request.Theme;
        tenant.BookingPageTitle = request.BookingPageTitle;
        tenant.BannerImageUrl = request.BannerImageUrl;
        tenant.ContactEmail = request.ContactEmail;
        tenant.ContactPhone = request.ContactPhone;
        tenant.Address = request.Address;
        tenant.City = request.City;
        tenant.PostCode = request.PostCode;
        tenant.Country = request.Country;
        tenant.Website = request.Website;
        tenant.TimeZone = request.TimeZone;
        tenant.Currency = request.Currency;
        tenant.AllowOnlineBooking = request.AllowOnlineBooking;
        tenant.RequirePaymentUpfront = request.RequirePaymentUpfront;
        tenant.SendReminders = request.SendReminders;
        tenant.ReminderHoursBefore = request.ReminderHoursBefore;
        tenant.EnableStripe = request.EnableStripe;
        tenant.EnablePayPal = request.EnablePayPal;
        tenant.EnableApplePay = request.EnableApplePay;
        tenant.StripePublishableKey = request.StripePublishableKey;
        tenant.PayPalClientId = request.PayPalClientId;
        tenant.AllowedEmbedDomains = request.AllowedEmbedDomains;
        tenant.CustomCss = request.CustomCss;
        tenant.DefaultMeetingLink = request.DefaultMeetingLink;
        tenant.OpenAiApiKey = request.OpenAiApiKey;
        tenant.ElevenLabsApiKey = request.ElevenLabsApiKey;
        tenant.ElevenLabsVoiceId = request.ElevenLabsVoiceId;
        tenant.VapiPublicKey = request.VapiPublicKey;
        tenant.EnableAiChat = request.EnableAiChat;
        tenant.EnableSoftDelete = request.EnableSoftDelete;
        tenant.UpdatedAt = DateTime.UtcNow;

        // Subdomain update — validate uniqueness
        if (!string.IsNullOrWhiteSpace(request.Subdomain))
        {
            var normalized = request.Subdomain.ToLowerInvariant().Trim();
            var conflict = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Subdomain == normalized && t.Id != tenant.Id && !t.IsDeleted);
            if (conflict != null)
                return Conflict(new { message = $"Subdomain '{normalized}' is already in use." });
            tenant.Subdomain = normalized;
        }
        else
        {
            tenant.Subdomain = null;
        }

        // Only super admins may approve a subdomain; TenantAdmins can request but not approve.
        var isSuperAdmin = User.IsInRole("SuperAdmin");
        if (isSuperAdmin)
            tenant.SubdomainApproved = request.SubdomainApproved;
        // else: SubdomainApproved is left unchanged (non-superadmin callers cannot grant approval)

        if (!string.IsNullOrEmpty(request.StripeSecretKey))
            tenant.StripeSecretKey = request.StripeSecretKey;
        if (!string.IsNullOrEmpty(request.PayPalClientSecret))
            tenant.PayPalClientSecret = request.PayPalClientSecret;

        await _context.SaveChangesAsync();

        return Ok(MapToResponse(tenant));
    }

    [Authorize]
    [HttpGet("{slug}/business-hours")]
    public async Task<ActionResult<IEnumerable<BusinessHours>>> GetBusinessHours(string slug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        var hours = await _context.BusinessHours
            .Where(bh => bh.TenantId == tenant.Id && !bh.IsDeleted)
            .OrderBy(bh => bh.DayOfWeek)
            .ToListAsync();

        return Ok(hours);
    }

    [Authorize]
    [HttpPut("{slug}/business-hours")]
    public async Task<IActionResult> UpdateBusinessHours(string slug, [FromBody] List<BusinessHours> hours)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        foreach (var hour in hours)
        {
            if (hour.TenantId != tenant.Id)
                return Forbid();

            var existing = await _context.BusinessHours.FindAsync(hour.Id);
            if (existing != null && existing.TenantId == tenant.Id)
            {
                existing.OpenTime = hour.OpenTime;
                existing.CloseTime = hour.CloseTime;
                existing.IsClosed = hour.IsClosed;
                existing.SlotDurationMinutes = hour.SlotDurationMinutes;
                existing.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{slug}/ai-config")]
    public async Task<ActionResult<object>> GetAiConfig(string slug)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted && t.IsActive);
        if (tenant == null) return NotFound();

        return Ok(new
        {
            vapiPublicKey = tenant.VapiPublicKey,
            elevenLabsVoiceId = tenant.ElevenLabsVoiceId,
            enableAiChat = tenant.EnableAiChat
        });
    }

    /// <summary>Deactivate the tenant account (marks IsActive = false; does not delete data).</summary>
    [Authorize]
    [HttpPost("{slug}/deactivate")]
    public async Task<IActionResult> DeactivateAccount(string slug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        tenant.IsActive = false;
        tenant.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Permanently delete the tenant account and all associated data.</summary>
    [Authorize]
    [HttpDelete("{slug}")]
    public async Task<IActionResult> DeleteAccount(string slug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        tenant.IsDeleted = true;
        tenant.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Export all tenant data as a zip file containing CSV files.</summary>
    [Authorize]
    [HttpGet("{slug}/export-data")]
    public async Task<IActionResult> ExportData(string slug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug && !t.IsDeleted);
        if (tenant == null) return NotFound();

        if (!_tenantService.IsValidTenantAccess(tenant.Id))
            return Forbid();

        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            // Appointments CSV
            var appointments = await _context.Appointments
                .Where(a => a.TenantId == tenant.Id && !a.IsDeleted)
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            var apptCsv = new StringBuilder();
            apptCsv.AppendLine("Id,CustomerName,CustomerEmail,CustomerPhone,StartTime,EndTime,Status,TotalAmount,Notes");
            foreach (var a in appointments)
                apptCsv.AppendLine($"{a.Id},{CsvEscape(a.CustomerName)},{CsvEscape(a.CustomerEmail)},{CsvEscape(a.CustomerPhone ?? "")},{a.StartTime:yyyy-MM-dd HH:mm},{a.EndTime:yyyy-MM-dd HH:mm},{a.Status},{a.TotalAmount},{CsvEscape(a.CustomerNotes ?? "")}");
            AddZipEntry(zip, "appointments.csv", apptCsv.ToString());

            // Customers CSV
            var customers = await _context.Customers
                .Where(c => c.TenantId == tenant.Id && !c.IsDeleted)
                .ToListAsync();

            var custCsv = new StringBuilder();
            custCsv.AppendLine("Id,FirstName,LastName,Email,Phone,Notes,CreatedAt");
            foreach (var c in customers)
                custCsv.AppendLine($"{c.Id},{CsvEscape(c.FirstName)},{CsvEscape(c.LastName)},{CsvEscape(c.Email)},{CsvEscape(c.Phone ?? "")},{CsvEscape(c.Notes ?? "")},{c.CreatedAt:yyyy-MM-dd}");
            AddZipEntry(zip, "customers.csv", custCsv.ToString());

            // Services CSV
            var services = await _context.Services
                .Where(s => s.TenantId == tenant.Id && !s.IsDeleted)
                .ToListAsync();

            var svcCsv = new StringBuilder();
            svcCsv.AppendLine("Id,Name,Description,Price,DurationMinutes,AllowOnlineBooking");
            foreach (var s in services)
                svcCsv.AppendLine($"{s.Id},{CsvEscape(s.Name)},{CsvEscape(s.Description ?? "")},{s.Price},{s.DurationMinutes},{s.AllowOnlineBooking}");
            AddZipEntry(zip, "services.csv", svcCsv.ToString());
        }

        ms.Seek(0, SeekOrigin.Begin);
        return File(ms.ToArray(), "application/zip", $"bookit-export-{slug}-{DateTime.UtcNow:yyyyMMdd}.zip");
    }

    private static void AddZipEntry(ZipArchive zip, string name, string content)
    {
        var entry = zip.CreateEntry(name);
        using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
        writer.Write(content);
    }

    private static string CsvEscape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }

    private static TenantResponse MapToResponse(Tenant t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Slug = t.Slug,
        BusinessType = t.BusinessType,
        CustomBusinessType = t.CustomBusinessType,
        LogoUrl = t.LogoUrl,
        PrimaryColor = t.PrimaryColor,
        SecondaryColor = t.SecondaryColor,
        Theme = t.Theme,
        BookingPageTitle = t.BookingPageTitle,
        BannerImageUrl = t.BannerImageUrl,
        ContactEmail = t.ContactEmail,
        ContactPhone = t.ContactPhone,
        Address = t.Address,
        City = t.City,
        PostCode = t.PostCode,
        Country = t.Country,
        Website = t.Website,
        TimeZone = t.TimeZone,
        Currency = t.Currency,
        AllowOnlineBooking = t.AllowOnlineBooking,
        RequirePaymentUpfront = t.RequirePaymentUpfront,
        EnableStripe = t.EnableStripe,
        EnablePayPal = t.EnablePayPal,
        EnableApplePay = t.EnableApplePay,
        StripePublishableKey = t.StripePublishableKey,
        PayPalClientId = t.PayPalClientId,
        EnableSoftDelete = t.EnableSoftDelete,
        Subdomain = t.Subdomain,
        SubdomainApproved = t.SubdomainApproved
    };
}
