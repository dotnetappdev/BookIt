package com.bookit.app.services

import android.os.Build
import androidx.biometric.BiometricManager
import androidx.biometric.BiometricManager.Authenticators.*
import androidx.biometric.BiometricPrompt
import androidx.core.content.ContextCompat
import androidx.fragment.app.FragmentActivity
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlin.coroutines.resume

/**
 * Wraps [BiometricPrompt] to provide a suspending [authenticate] function.
 *
 * Supports:
 * - Fingerprint / Face unlock (Class 3 biometrics)
 * - Device PIN / Pattern / Password fallback
 *
 * The caller must supply the host [FragmentActivity] because
 * [BiometricPrompt] requires it for lifecycle management.
 */
class BiometricService(private val activity: FragmentActivity) {

    enum class BiometricAvailability { AVAILABLE, NOT_ENROLLED, NOT_AVAILABLE }

    /** Returns whether any biometric (fingerprint or face) is enrolled and available. */
    fun checkBiometricAvailability(): BiometricAvailability {
        val manager = BiometricManager.from(activity)
        return when (manager.canAuthenticate(BIOMETRIC_STRONG)) {
            BiometricManager.BIOMETRIC_SUCCESS          -> BiometricAvailability.AVAILABLE
            BiometricManager.BIOMETRIC_ERROR_NONE_ENROLLED -> BiometricAvailability.NOT_ENROLLED
            else                                        -> BiometricAvailability.NOT_AVAILABLE
        }
    }

    val isBiometricAvailable: Boolean
        get() = checkBiometricAvailability() == BiometricAvailability.AVAILABLE

    /**
     * Shows the biometric prompt.
     * - If biometrics are available → shows biometric prompt with "Use PIN" fallback.
     * - If biometrics are not enrolled/available → shows device credentials only.
     *
     * @return `true` on success, `false` on failure or cancellation.
     */
    suspend fun authenticate(
        title: String = "Verify your identity",
        subtitle: String = "Use biometrics or device PIN to access BookIt",
        negativeButtonText: String = "Cancel"
    ): Boolean = suspendCancellableCoroutine { cont ->
        val executor = ContextCompat.getMainExecutor(activity)

        val callback = object : BiometricPrompt.AuthenticationCallback() {
            override fun onAuthenticationSucceeded(result: BiometricPrompt.AuthenticationResult) {
                if (cont.isActive) cont.resume(true)
            }
            override fun onAuthenticationError(errorCode: Int, errString: CharSequence) {
                if (cont.isActive) cont.resume(false)
            }
            override fun onAuthenticationFailed() {
                // Wrong biometric — don't dismiss the prompt; let the system handle retries.
            }
        }

        val prompt = BiometricPrompt(activity, executor, callback)

        // Choose authenticator set: prefer biometrics + device credential as fallback
        val authenticators = if (isBiometricAvailable) {
            BIOMETRIC_STRONG or DEVICE_CREDENTIAL
        } else {
            DEVICE_CREDENTIAL
        }

        val promptInfo = BiometricPrompt.PromptInfo.Builder()
            .setTitle(title)
            .setSubtitle(subtitle)
            .setAllowedAuthenticators(authenticators)
            .apply {
                // negativeButton is only allowed when DEVICE_CREDENTIAL is NOT in the set
                if (authenticators and DEVICE_CREDENTIAL == 0) {
                    setNegativeButtonText(negativeButtonText)
                }
            }
            .build()

        prompt.authenticate(promptInfo)

        cont.invokeOnCancellation { prompt.cancelAuthentication() }
    }

    /**
     * Shows a PIN / Pattern / Password prompt without biometrics.
     */
    suspend fun authenticateWithDeviceCredential(
        title: String = "Enter your PIN",
        subtitle: String = "Use your device PIN, pattern, or password to access BookIt"
    ): Boolean = suspendCancellableCoroutine { cont ->
        val executor = ContextCompat.getMainExecutor(activity)

        val callback = object : BiometricPrompt.AuthenticationCallback() {
            override fun onAuthenticationSucceeded(result: BiometricPrompt.AuthenticationResult) {
                if (cont.isActive) cont.resume(true)
            }
            override fun onAuthenticationError(errorCode: Int, errString: CharSequence) {
                if (cont.isActive) cont.resume(false)
            }
            override fun onAuthenticationFailed() { /* retries handled by system */ }
        }

        val prompt = BiometricPrompt(activity, executor, callback)
        val promptInfo = BiometricPrompt.PromptInfo.Builder()
            .setTitle(title)
            .setSubtitle(subtitle)
            .setAllowedAuthenticators(DEVICE_CREDENTIAL)
            .build()

        prompt.authenticate(promptInfo)
        cont.invokeOnCancellation { prompt.cancelAuthentication() }
    }
}
