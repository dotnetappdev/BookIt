package com.bookit.app.ui.screens

import androidx.compose.animation.core.*
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Fingerprint
import androidx.compose.material.icons.filled.Lock
import androidx.compose.material.icons.filled.FaceRetouchingNatural
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.scale
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.fragment.app.FragmentActivity
import com.bookit.app.services.BiometricService
import com.bookit.app.ui.theme.*
import com.bookit.app.viewmodels.AuthViewModel
import kotlinx.coroutines.launch

/**
 * Full-screen biometric / PIN lock shown at every cold launch when a saved
 * session is found.  Mirrors [BiometricLockView.swift] from the iOS app.
 *
 * - Primary button: fingerprint / face (adapts to device capability)
 * - Secondary button: "Use PIN / Password" — always visible
 * - Escape hatch: "Sign out"
 */
@Composable
fun BiometricLockScreen(
    activity: FragmentActivity,
    authViewModel: AuthViewModel
) {
    val biometricService = remember { BiometricService(activity) }
    val isAvailable = biometricService.isBiometricAvailable

    val scope = rememberCoroutineScope()
    var isAuthenticating by remember { mutableStateOf(false) }
    var errorMessage by remember { mutableStateOf<String?>(null) }

    // Pulse animation for the biometric icon
    val infiniteTransition = rememberInfiniteTransition(label = "pulse")
    val scale by infiniteTransition.animateFloat(
        initialValue = 0.95f,
        targetValue  = 1.05f,
        animationSpec = infiniteRepeatable(
            animation = tween(1000, easing = FastOutSlowInEasing),
            repeatMode = RepeatMode.Reverse
        ),
        label = "iconScale"
    )

    // Auto-trigger biometrics on first composition
    LaunchedEffect(Unit) {
        attemptBiometric(biometricService, authViewModel) { error -> errorMessage = error }
    }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(
                Brush.verticalGradient(listOf(Color(0xFF1a1a1a), Color(0xFF0f0f0f)))
            )
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(32.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.SpaceBetween
        ) {
            Spacer(Modifier.height(40.dp))

            // App logo + title
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(12.dp)
            ) {
                Box(
                    modifier = Modifier
                        .size(88.dp)
                        .background(
                            Brush.linearGradient(listOf(Purple700, Purple400)),
                            RoundedCornerShape(24.dp)
                        ),
                    contentAlignment = Alignment.Center
                ) {
                    Icon(Icons.Default.Lock, contentDescription = null,
                        tint = Color.White, modifier = Modifier.size(44.dp))
                }
                Text("BookIt", fontSize = 34.sp, fontWeight = FontWeight.ExtraBold, color = TextPrimary)
                Text("Your account is locked", fontSize = 14.sp, color = TextMuted)
            }

            // Biometric icon (animated pulse)
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                Icon(
                    imageVector = if (isAvailable) Icons.Default.Fingerprint
                                  else Icons.Default.FaceRetouchingNatural,
                    contentDescription = null,
                    tint = Purple400,
                    modifier = Modifier.size(80.dp).scale(scale)
                )
                Text(
                    text = if (isAvailable) "Biometrics available" else "Use device PIN to unlock",
                    fontSize = 12.sp, color = TextMuted
                )
            }

            // Error banner
            errorMessage?.let { msg ->
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .background(Color.Red.copy(alpha = 0.12f), RoundedCornerShape(10.dp))
                        .padding(12.dp),
                    horizontalArrangement = Arrangement.spacedBy(8.dp),
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Icon(Icons.Default.Lock, contentDescription = null, tint = Color(0xFFff6b6b))
                    Text(msg, color = Color(0xFFff6b6b), fontSize = 13.sp)
                }
            }

            // Action buttons
            Column(
                modifier = Modifier.fillMaxWidth(),
                verticalArrangement = Arrangement.spacedBy(12.dp),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                // Primary — biometric (shown when available)
                if (isAvailable) {
                    Button(
                        onClick = {
                            scope.launch {
                                isAuthenticating = true
                                errorMessage = null
                                attemptBiometric(biometricService, authViewModel) { e -> errorMessage = e }
                                isAuthenticating = false
                            }
                        },
                        enabled = !isAuthenticating,
                        modifier = Modifier.fillMaxWidth().height(52.dp),
                        colors = ButtonDefaults.buttonColors(containerColor = Purple700),
                        shape = RoundedCornerShape(14.dp)
                    ) {
                        if (isAuthenticating) {
                            CircularProgressIndicator(Modifier.size(20.dp), color = Color.White, strokeWidth = 2.dp)
                            Spacer(Modifier.width(8.dp))
                        } else {
                            Icon(Icons.Default.Fingerprint, contentDescription = null,
                                modifier = Modifier.size(20.dp))
                            Spacer(Modifier.width(8.dp))
                        }
                        Text("Unlock with Biometrics", fontWeight = FontWeight.Bold, fontSize = 16.sp)
                    }
                }

                // Secondary — PIN / Password
                OutlinedButton(
                    onClick = {
                        scope.launch {
                            isAuthenticating = true
                            errorMessage = null
                            val ok = biometricService.authenticateWithDeviceCredential()
                            if (ok) authViewModel.unlockBiometric()
                            else errorMessage = "PIN verification failed. Please try again."
                            isAuthenticating = false
                        }
                    },
                    enabled = !isAuthenticating,
                    modifier = Modifier.fillMaxWidth().height(48.dp),
                    border = androidx.compose.foundation.BorderStroke(1.5.dp, Purple400),
                    shape = RoundedCornerShape(14.dp),
                    colors = ButtonDefaults.outlinedButtonColors(contentColor = Purple400)
                ) {
                    Icon(Icons.Default.Lock, contentDescription = null,
                        modifier = Modifier.size(16.dp))
                    Spacer(Modifier.width(8.dp))
                    Text("Use PIN / Password", fontWeight = FontWeight.SemiBold, fontSize = 15.sp)
                }

                // Sign-out escape hatch
                TextButton(onClick = { authViewModel.signOut() }) {
                    Text("Sign out", color = TextFaint, fontSize = 13.sp)
                }
            }

            Spacer(Modifier.height(24.dp))
        }
    }
}

private suspend fun attemptBiometric(
    service: BiometricService,
    authViewModel: AuthViewModel,
    onError: (String) -> Unit
) {
    val ok = service.authenticate()
    if (ok) authViewModel.unlockBiometric()
    else onError("Authentication failed. Tap below to try again.")
}
