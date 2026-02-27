package com.bookit.app.services

import android.content.Context
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import com.bookit.app.models.AuthResponse
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json

/**
 * Persists the [AuthResponse] in [EncryptedSharedPreferences] backed by the
 * Android Keystore.  The session survives process death and is cleared on
 * explicit sign-out — matching the iOS KeychainService behaviour.
 */
class SecureStorageService(context: Context) {

    private val masterKey = MasterKey.Builder(context)
        .setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
        .build()

    private val prefs = EncryptedSharedPreferences.create(
        context,
        "bookit_secure_prefs",
        masterKey,
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )

    private val json = Json { ignoreUnknownKeys = true }

    // -------------------------------------------------------------------------
    // Auth session
    // -------------------------------------------------------------------------

    fun saveAuth(auth: AuthResponse) {
        prefs.edit()
            .putString(KEY_AUTH, json.encodeToString(auth))
            .apply()
    }

    fun loadAuth(): AuthResponse? {
        val raw = prefs.getString(KEY_AUTH, null) ?: return null
        return try {
            json.decodeFromString<AuthResponse>(raw)
        } catch (_: Exception) {
            null
        }
    }

    fun clearAuth() {
        prefs.edit().remove(KEY_AUTH).apply()
    }

    companion object {
        private const val KEY_AUTH = "auth_response"
    }
}
