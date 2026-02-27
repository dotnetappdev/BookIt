package com.bookit.app.ui.theme

import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.runtime.Composable

private val DarkColors = darkColorScheme(
    primary        = Purple700,
    onPrimary      = TextPrimary,
    primaryContainer = Purple400,
    secondary      = Blue500,
    background     = Surface800,
    surface        = Surface700,
    onBackground   = TextPrimary,
    onSurface      = TextPrimary,
    error          = Red400
)

@Composable
fun BookItTheme(content: @Composable () -> Unit) {
    MaterialTheme(
        colorScheme = DarkColors,
        typography  = BookItTypography,
        content     = content
    )
}
