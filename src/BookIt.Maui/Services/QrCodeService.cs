using QRCoder;
using System.Drawing;

namespace BookIt.Maui.Services;

/// <summary>
/// Generates QR code images as Base64 PNG strings for use in Blazor components.
/// </summary>
public class QrCodeService
{
    /// <summary>
    /// Generates a QR code for the given data and returns it as a Base64-encoded PNG data URI.
    /// </summary>
    public string GenerateQrDataUri(string data, int pixelsPerModule = 5)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.M);
        using var qrCode = new PngByteQRCode(qrData);
        var bytes = qrCode.GetGraphic(pixelsPerModule);
        return $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
    }
}
