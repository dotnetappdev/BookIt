import SwiftUI
import CoreImage.CIFilterBuiltins

/// Generates a QR code image from a string using CoreImage — no external dependencies.
struct QRCodeView: View {
    let content: String
    var size: CGFloat = 200

    var body: some View {
        if let uiImage = QRCodeGenerator.generateUIImage(from: content, size: size) {
            Image(uiImage: uiImage)
                .interpolation(.none)
                .resizable()
                .scaledToFit()
                .frame(width: size, height: size)
        } else {
            Rectangle()
                .fill(Color(hex: "3a3a3a"))
                .frame(width: size, height: size)
                .overlay(
                    Image(systemName: "qrcode")
                        .font(.system(size: size * 0.3))
                        .foregroundColor(Color(hex: "6a6a6a"))
                )
        }
    }
}

// MARK: - QR Code Generator (CoreImage)

enum QRCodeGenerator {

    /// Returns a UIImage of the QR code for `content` rendered at `size × size` points.
    static func generateUIImage(from content: String, size: CGFloat) -> UIImage? {
        let filter = CIFilter.qrCodeGenerator()
        guard let data = content.data(using: .utf8) else { return nil }
        filter.message = data
        filter.correctionLevel = "M"

        guard let outputImage = filter.outputImage else { return nil }

        // Scale up so the QR is crisp at the requested size
        let scaleX = size / outputImage.extent.width
        let scaleY = size / outputImage.extent.height
        let scaledImage = outputImage.transformed(by: CGAffineTransform(scaleX: scaleX, y: scaleY))

        // Invert colours (CIFilter gives black-on-transparent; we want black on white)
        let colorInvert = CIFilter(name: "CIColorInvert")!
        colorInvert.setValue(scaledImage, forKey: kCIInputImageKey)
        guard let invertedImage = colorInvert.outputImage else { return nil }

        let maskToAlpha = CIFilter(name: "CIMaskToAlpha")!
        maskToAlpha.setValue(invertedImage, forKey: kCIInputImageKey)
        guard let maskImage = maskToAlpha.outputImage else { return nil }

        // Blend onto white background for the final PNG
        let background = CIImage(color: .white)
            .cropped(to: maskImage.extent)

        let compose = CIFilter(name: "CISourceAtopCompositing")!
        compose.setValue(maskImage, forKey: kCIInputImageKey)
        compose.setValue(background, forKey: kCIInputBackgroundImageKey)
        guard let composedImage = compose.outputImage else { return nil }

        let context = CIContext()
        guard let cgImage = context.createCGImage(composedImage, from: composedImage.extent) else { return nil }
        return UIImage(cgImage: cgImage)
    }
}
