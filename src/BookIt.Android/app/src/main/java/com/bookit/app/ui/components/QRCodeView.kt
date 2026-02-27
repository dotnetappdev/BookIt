package com.bookit.app.ui.components

import android.graphics.Bitmap
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.size
import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import com.bookit.app.ui.theme.Surface600
import com.google.zxing.BarcodeFormat
import com.google.zxing.EncodeHintType
import com.google.zxing.qrcode.QRCodeWriter
import android.graphics.Color as AndroidColor

/**
 * Renders a QR code for [content] using the ZXing Core library.
 * Produces a black-on-white bitmap — matching the iOS QRCodeView.
 */
@Composable
fun QRCodeView(
    content: String,
    size: Dp = 220.dp,
    modifier: Modifier = Modifier
) {
    val bitmap = remember(content) { generateQrBitmap(content, 512) }

    if (bitmap != null) {
        Image(
            bitmap = bitmap.asImageBitmap(),
            contentDescription = "QR Code",
            modifier = modifier.size(size)
        )
    } else {
        Box(
            modifier = modifier
                .size(size)
                .background(Surface600),
            contentAlignment = Alignment.Center
        ) { /* fallback blank box */ }
    }
}

/**
 * Generates a [Bitmap] containing the QR code for [content] at [pixelSize]×[pixelSize].
 */
fun generateQrBitmap(content: String, pixelSize: Int = 512): Bitmap? {
    return try {
        val hints = mapOf(EncodeHintType.MARGIN to 1)
        val writer = QRCodeWriter()
        val matrix = writer.encode(content, BarcodeFormat.QR_CODE, pixelSize, pixelSize, hints)
        val bitmap = Bitmap.createBitmap(pixelSize, pixelSize, Bitmap.Config.ARGB_8888)
        val pixels = IntArray(pixelSize * pixelSize) { i ->
            val x = i % pixelSize
            val y = i / pixelSize
            if (matrix[x, y]) AndroidColor.BLACK else AndroidColor.WHITE
        }
        bitmap.setPixels(pixels, 0, pixelSize, 0, 0, pixelSize, pixelSize)
        bitmap
    } catch (_: Exception) {
        null
    }
}
