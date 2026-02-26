using BookIt.Core.DTOs;
using BookIt.UI.Shared.Services;
using System.Text;

namespace BookIt.Maui.Services;

/// <summary>
/// Generates wallet-compatible pass files and coordinates device sharing/saving.
/// 
/// Platform support:
/// - iOS: Apple Wallet PKPass via server-side signing; ICS calendar as fallback
/// - Android: Google Wallet JWT save URL; ICS calendar as fallback
/// - Both: native Share sheet for the QR image data URI
///
/// Note: Full Apple PKPass (.pkpass) and Google Wallet JWT passes require
/// platform developer certificates (Apple Developer Program / Google Pay & Wallet Console)
/// and server-side signing. When credentials are not configured on the server,
/// falls back to ICS calendar export.
/// </summary>
public class WalletPassService
{
    private readonly BookItApiService _api;

    public WalletPassService(BookItApiService api)
    {
        _api = api;
    }

    // ── Apple Wallet ──────────────────────────────────────────────────────────

    /// <summary>
    /// Downloads the .pkpass from the server and opens it in iOS Wallet app.
    /// Falls back to ICS export when the server is not configured for Apple Wallet.
    /// </summary>
    public async Task AddToAppleWalletAsync(
        AppointmentResponse appointment,
        string tenantSlug,
        string businessName,
        string? membershipNumber = null)
    {
        try
        {
            var passBytes = await _api.GetAppleWalletPassAsync(tenantSlug, appointment.Id);
            if (passBytes != null && passBytes.Length > 0)
            {
                var tempFile = Path.Combine(FileSystem.Current.CacheDirectory,
                    $"bookit-{appointment.Id:N}.pkpass");
                await File.WriteAllBytesAsync(tempFile, passBytes);
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(tempFile, "application/vnd.apple.pkpass"),
                    Title = "Add to Apple Wallet"
                });
                return;
            }
        }
        catch
        {
            // Server not configured — fall through to ICS
        }

        // Fallback: ICS calendar export
        await AddToCalendarAsync(appointment, businessName, membershipNumber);
    }

    // ── Google Wallet ─────────────────────────────────────────────────────────

    /// <summary>
    /// Opens the Google Wallet "Add to Google Wallet" URL in the browser.
    /// Falls back to ICS export when the server is not configured.
    /// </summary>
    public async Task AddToGoogleWalletAsync(
        AppointmentResponse appointment,
        string tenantSlug,
        string businessName,
        string? membershipNumber = null)
    {
        try
        {
            var saveUrl = await _api.GetGoogleWalletUrlAsync(tenantSlug, appointment.Id);
            if (!string.IsNullOrEmpty(saveUrl))
            {
                await Launcher.Default.OpenAsync(new Uri(saveUrl));
                return;
            }
        }
        catch
        {
            // Server not configured — fall through to ICS
        }

        // Fallback: ICS calendar export
        await AddToCalendarAsync(appointment, businessName, membershipNumber);
    }

    // ── ICS Calendar (cross-platform fallback) ────────────────────────────────

    /// <summary>
    /// Generates an ICS (iCalendar) calendar event string for an appointment.
    /// The event can be opened on both iOS and Android to add to the device calendar.
    /// </summary>
    public string GenerateIcs(
        AppointmentResponse appointment,
        string businessName,
        string? membershipNumber = null)
    {
        var uid = $"bookit-{appointment.Id}@bookit.app";
        var dtStart = appointment.StartTime.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
        var dtEnd   = appointment.EndTime.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
        var dtStamp = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
        var services = string.Join(", ", appointment.Services.Select(s => s.Name));

        var descParts = new List<string>();
        descParts.Add($"Booking with {businessName}");
        if (!string.IsNullOrEmpty(services))
            descParts.Add($"Service: {services}");
        if (!string.IsNullOrEmpty(appointment.BookingPin))
            descParts.Add($"PIN: {appointment.BookingPin}");
        if (!string.IsNullOrEmpty(membershipNumber))
            descParts.Add($"Membership No: {membershipNumber}");
        if (!string.IsNullOrEmpty(appointment.StaffName))
            descParts.Add($"With: {appointment.StaffName}");

        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:-//BookIt//BookIt MAUI//EN");
        sb.AppendLine("CALSCALE:GREGORIAN");
        sb.AppendLine("METHOD:PUBLISH");
        sb.AppendLine("BEGIN:VEVENT");
        sb.AppendLine($"UID:{uid}");
        sb.AppendLine($"DTSTAMP:{dtStamp}");
        sb.AppendLine($"DTSTART:{dtStart}");
        sb.AppendLine($"DTEND:{dtEnd}");
        sb.AppendLine($"SUMMARY:{EscapeIcs(services.Length > 0 ? $"{services} @ {businessName}" : $"Appointment @ {businessName}")}");
        sb.AppendLine($"DESCRIPTION:{EscapeIcs(string.Join("\\n", descParts))}");
        sb.AppendLine("STATUS:CONFIRMED");
        sb.AppendLine("END:VEVENT");
        sb.AppendLine("END:VCALENDAR");
        return sb.ToString();
    }

    /// <summary>
    /// Saves the ICS content to a temporary file and opens it using the platform
    /// calendar integration. On iOS this adds the event to Apple Calendar.
    /// On Android this opens Google Calendar or the default calendar app.
    /// </summary>
    public async Task AddToCalendarAsync(
        AppointmentResponse appointment,
        string businessName,
        string? membershipNumber = null)
    {
        var ics = GenerateIcs(appointment, businessName, membershipNumber);
        var tempFile = Path.Combine(FileSystem.Current.CacheDirectory, $"bookit-{appointment.Id:N}.ics");
        await File.WriteAllTextAsync(tempFile, ics, Encoding.UTF8);
        await Launcher.Default.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(tempFile),
            Title = "Add to Calendar"
        });
    }

    /// <summary>
    /// Shares the appointment QR code image (as a data URI PNG) via the native
    /// platform share sheet. The user can save it to Photos, send via Messages, etc.
    /// </summary>
    public async Task ShareQrPassAsync(
        AppointmentResponse appointment,
        string businessName,
        string qrDataUri)
    {
        if (string.IsNullOrEmpty(qrDataUri)) return;

        // Robustly parse data URI: data:[<mediatype>][;base64],<data>
        var commaIndex = qrDataUri.IndexOf(',');
        if (commaIndex < 0)
            throw new ArgumentException("Invalid data URI: missing comma separator.", nameof(qrDataUri));

        var header = qrDataUri[..commaIndex];
        if (!header.Contains("base64", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Invalid data URI: expected base64-encoded content.", nameof(qrDataUri));

        var base64 = qrDataUri[(commaIndex + 1)..].Trim();
        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(base64);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Invalid data URI: base64 content could not be decoded.", nameof(qrDataUri), ex);
        }

        var tempFile = Path.Combine(FileSystem.Current.CacheDirectory, $"qr-pass-{appointment.Id:N}.png");
        await File.WriteAllBytesAsync(tempFile, bytes);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = $"BookIt Pass – {businessName}",
            File = new ShareFile(tempFile, "image/png")
        });
    }

    private static string EscapeIcs(string value)
        => value.Replace("\\", "\\\\").Replace(";", "\\;").Replace(",", "\\,").Replace("\n", "\\n");
}

