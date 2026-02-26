using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using BookIt.Core.DTOs;

namespace BookIt.API.Services;

/// <summary>
/// Generates Apple Wallet PKPass files for booking appointments.
///
/// Configuration (appsettings.json or user-secrets):
///   AppleWallet:PassTypeIdentifier  — e.g. "pass.com.bookit.app"
///   AppleWallet:TeamIdentifier      — 10-character Apple Team ID
///   AppleWallet:CertificateBase64   — Base64 of your .p12 / .pfx pass type certificate
///   AppleWallet:CertificatePassword — Password for the .p12 certificate
///
/// These credentials are obtained from the Apple Developer Portal.
/// When IsConfigured is false the controller returns a 503 response with guidance.
/// </summary>
public class AppleWalletService
{
    private readonly string? _passTypeIdentifier;
    private readonly string? _teamIdentifier;
    private readonly string? _p12Base64;
    private readonly string? _p12Password;

    public AppleWalletService(IConfiguration config)
    {
        _passTypeIdentifier = config["AppleWallet:PassTypeIdentifier"];
        _teamIdentifier = config["AppleWallet:TeamIdentifier"];
        _p12Base64 = config["AppleWallet:CertificateBase64"];
        _p12Password = config["AppleWallet:CertificatePassword"];
    }

    /// <summary>True when all required Apple Wallet credentials are present in configuration.</summary>
    public bool IsConfigured =>
        !string.IsNullOrEmpty(_passTypeIdentifier) &&
        !string.IsNullOrEmpty(_teamIdentifier) &&
        !string.IsNullOrEmpty(_p12Base64);

    /// <summary>
    /// Generates a signed .pkpass ZIP archive for the given appointment.
    /// </summary>
    /// <returns>Raw bytes of the .pkpass file (application/vnd.apple.pkpass)</returns>
    public byte[] GeneratePass(AppointmentResponse appointment, string businessName, string? membershipNumber = null)
    {
        var passJson = BuildPassJson(appointment, businessName, membershipNumber);
        var passBytes = Encoding.UTF8.GetBytes(passJson);
        var passHash = Convert.ToHexString(SHA1.HashData(passBytes)).ToLowerInvariant();

        var manifest = JsonSerializer.Serialize(new Dictionary<string, string>
        {
            ["pass.json"] = passHash
        });
        var manifestBytes = Encoding.UTF8.GetBytes(manifest);
        var signature = SignManifest(manifestBytes);

        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            AddEntry(zip, "pass.json", passBytes);
            AddEntry(zip, "manifest.json", manifestBytes);
            AddEntry(zip, "signature", signature);
        }
        return ms.ToArray();
    }

    private string BuildPassJson(AppointmentResponse apt, string businessName, string? membershipNumber)
    {
        var services = string.Join(", ", apt.Services.Select(s => s.Name));

        var secondaryFields = new List<object>
        {
            new { key = "date", label = "DATE", value = apt.StartTime.ToString("d MMM yyyy") },
            new { key = "time", label = "TIME", value = $"{apt.StartTime:h:mm tt} – {apt.EndTime:h:mm tt}" }
        };

        var auxiliaryFields = new List<object>();
        if (!string.IsNullOrEmpty(apt.StaffName))
            auxiliaryFields.Add(new { key = "staff", label = "WITH", value = apt.StaffName });
        if (!string.IsNullOrEmpty(membershipNumber))
            auxiliaryFields.Add(new { key = "membership", label = "MEMBERSHIP NO.", value = membershipNumber });

        var backFields = new List<object>();
        if (!string.IsNullOrEmpty(apt.BookingPin))
            backFields.Add(new { key = "pin", label = "Booking PIN", value = apt.BookingPin });
        if (!string.IsNullOrEmpty(apt.CustomerEmail))
            backFields.Add(new { key = "email", label = "Email", value = apt.CustomerEmail });

        var pass = new
        {
            formatVersion = 1,
            passTypeIdentifier = _passTypeIdentifier,
            serialNumber = apt.Id.ToString("N"),
            teamIdentifier = _teamIdentifier,
            organizationName = businessName,
            description = $"Booking at {businessName}",
            foregroundColor = "rgb(255, 255, 255)",
            backgroundColor = "rgb(108, 92, 231)",
            labelColor = "rgb(230, 220, 255)",
            eventTicket = new
            {
                primaryFields = new[]
                {
                    new { key = "service", label = "SERVICE", value = services.Length > 0 ? services : "Appointment" }
                },
                secondaryFields = secondaryFields.ToArray(),
                auxiliaryFields = auxiliaryFields.ToArray(),
                backFields = backFields.ToArray()
            },
            barcode = new
            {
                message = $"BOOKIT:{apt.Id}:{apt.BookingPin}:{apt.StartTime:yyyyMMddHHmm}:{membershipNumber ?? "NONE"}",
                format = "PKBarcodeFormatQR",
                messageEncoding = "iso-8859-1",
                altText = apt.BookingPin ?? apt.Id.ToString("N")[..8]
            }
        };

        return JsonSerializer.Serialize(pass, new JsonSerializerOptions { WriteIndented = false });
    }

    private byte[] SignManifest(byte[] manifestBytes)
    {
        if (string.IsNullOrEmpty(_p12Base64))
            return Array.Empty<byte>();

        var certBytes = Convert.FromBase64String(_p12Base64);
        using var cert = X509CertificateLoader.LoadPkcs12(
            certBytes,
            _p12Password,
            X509KeyStorageFlags.EphemeralKeySet | X509KeyStorageFlags.Exportable);

        var contentInfo = new ContentInfo(manifestBytes);
        var cms = new SignedCms(contentInfo, detached: true);
        var signer = new CmsSigner(SubjectIdentifierType.SubjectKeyIdentifier, cert)
        {
            IncludeOption = X509IncludeOption.WholeChain,
            DigestAlgorithm = new Oid("2.16.840.1.101.3.4.2.1") // SHA-256
        };
        cms.ComputeSignature(signer);
        return cms.Encode();
    }

    private static void AddEntry(ZipArchive zip, string name, byte[] data)
    {
        var entry = zip.CreateEntry(name, CompressionLevel.NoCompression);
        using var stream = entry.Open();
        stream.Write(data);
    }
}
