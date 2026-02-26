using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BookIt.Core.DTOs;

namespace BookIt.API.Services;

/// <summary>
/// Generates Google Wallet "Add to Google Wallet" save URLs for booking appointments.
///
/// Configuration (appsettings.json or user-secrets):
///   GoogleWallet:IssuerId           — Numeric issuer ID from the Google Pay & Wallet Console
///   GoogleWallet:ServiceAccountEmail — Service account email (ends in @*.iam.gserviceaccount.com)
///   GoogleWallet:PrivateKeyPem      — RSA private key in PEM format (-----BEGIN RSA PRIVATE KEY-----)
///
/// Setup steps:
///   1. Enable the Google Wallet API in Google Cloud Console
///   2. Create a service account and download the JSON key
///   3. Register as a Google Wallet issuer in the Google Pay & Wallet Console
///   4. Grant the service account "Google Wallet Object Issuer" permission
///
/// When IsConfigured is false the controller returns a 503 response with guidance.
/// </summary>
public class GoogleWalletService
{
    private readonly string? _issuerId;
    private readonly string? _serviceAccountEmail;
    private readonly string? _privateKeyPem;

    public GoogleWalletService(IConfiguration config)
    {
        _issuerId = config["GoogleWallet:IssuerId"];
        _serviceAccountEmail = config["GoogleWallet:ServiceAccountEmail"];
        _privateKeyPem = config["GoogleWallet:PrivateKeyPem"];
    }

    /// <summary>True when all required Google Wallet credentials are present in configuration.</summary>
    public bool IsConfigured =>
        !string.IsNullOrEmpty(_issuerId) &&
        !string.IsNullOrEmpty(_serviceAccountEmail) &&
        !string.IsNullOrEmpty(_privateKeyPem);

    /// <summary>
    /// Generates a Google Wallet "Add to Google Wallet" URL for the given appointment.
    /// The URL opens in the Android browser and prompts the user to save the pass.
    /// </summary>
    /// <returns>https://pay.google.com/gp/v/save/{signedJwt}</returns>
    public string GenerateSaveUrl(
        AppointmentResponse appointment,
        string businessName,
        string tenantSlug,
        string? membershipNumber = null)
    {
        var classId = $"{_issuerId}.bookit-{tenantSlug}";
        var objectId = $"{_issuerId}.bookit-apt-{appointment.Id:N}";
        var services = string.Join(", ", appointment.Services.Select(s => s.Name));

        var walletObject = new
        {
            id = objectId,
            classId,
            genericType = "GENERIC_TYPE_UNSPECIFIED",
            hexBackgroundColor = "#6c5ce7",
            logo = new
            {
                sourceUri = new { uri = "https://bookit.app/images/logo.png" },
                contentDescription = new { defaultValue = new { language = "en-GB", value = "BookIt Logo" } }
            },
            cardTitle = new { defaultValue = new { language = "en-GB", value = businessName } },
            subheader = new { defaultValue = new { language = "en-GB", value = "Booking Pass" } },
            header = new
            {
                defaultValue = new
                {
                    language = "en-GB",
                    value = services.Length > 0 ? services : "Appointment"
                }
            },
            textModulesData = BuildTextModules(appointment, membershipNumber),
            barcode = new
            {
                type = "QR_CODE",
                value = $"BOOKIT:{appointment.Id}:{appointment.BookingPin}:{appointment.StartTime:yyyyMMddHHmm}:{membershipNumber ?? "NONE"}",
                alternateText = appointment.BookingPin ?? ""
            },
            validTimeInterval = new
            {
                start = new { date = appointment.StartTime.AddHours(-2).ToString("yyyy-MM-ddTHH:mm:ssZ") },
                end = new { date = appointment.EndTime.AddHours(2).ToString("yyyy-MM-ddTHH:mm:ssZ") }
            },
            state = "ACTIVE"
        };

        var jwtPayload = new
        {
            iss = _serviceAccountEmail,
            aud = "google",
            typ = "savetowallet",
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            origins = Array.Empty<string>(),
            payload = new { genericObjects = new[] { walletObject } }
        };

        var jwt = BuildJwt(jwtPayload);
        return $"https://pay.google.com/gp/v/save/{jwt}";
    }

    private static object[] BuildTextModules(AppointmentResponse apt, string? membershipNumber)
    {
        var modules = new List<object>
        {
            new { id = "date", header = "DATE", body = apt.StartTime.ToString("dddd, d MMMM yyyy") },
            new { id = "time", header = "TIME", body = $"{apt.StartTime:h:mm tt} – {apt.EndTime:h:mm tt}" }
        };
        if (!string.IsNullOrEmpty(apt.StaffName))
            modules.Add(new { id = "staff", header = "WITH", body = apt.StaffName });
        if (!string.IsNullOrEmpty(apt.BookingPin))
            modules.Add(new { id = "pin", header = "BOOKING PIN", body = apt.BookingPin });
        if (!string.IsNullOrEmpty(membershipNumber))
            modules.Add(new { id = "membership", header = "MEMBERSHIP NO.", body = membershipNumber });
        return modules.ToArray();
    }

    private string BuildJwt(object payload)
    {
        var header = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(
            new { alg = "RS256", typ = "JWT" }));
        var body = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload));
        var unsigned = $"{header}.{body}";

        using var rsa = RSA.Create();
        rsa.ImportFromPem(_privateKeyPem!.AsSpan());
        var signatureBytes = rsa.SignData(
            Encoding.ASCII.GetBytes(unsigned),
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        return $"{unsigned}.{Base64UrlEncode(signatureBytes)}";
    }

    private static string Base64UrlEncode(byte[] data)
        => Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
