package com.bookit.app.viewmodels

import android.app.Application
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.bookit.app.models.AuthResponse
import com.bookit.app.services.BookItApiService
import com.bookit.app.services.SecureStorageService
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

/**
 * Holds authentication state and coordinates sign-in / sign-up / biometric lock.
 *
 * State transitions:
 *  - App cold-starts with a saved session → [AuthState.BiometricLock]
 *  - User unlocks biometrics or enters PIN  → [AuthState.Authenticated]
 *  - No saved session / sign-out            → [AuthState.Unauthenticated]
 */
class AuthViewModel(app: Application) : AndroidViewModel(app) {

    private val storage = SecureStorageService(app)

    private val _authState = MutableStateFlow<AuthState>(AuthState.Loading)
    val authState: StateFlow<AuthState> = _authState.asStateFlow()

    // Convenience accessors
    val authResponse: AuthResponse? get() = (_authState.value as? AuthState.Authenticated)?.auth
    val accessToken: String get()  = authResponse?.accessToken ?: ""
    val tenantSlug: String get()   = authResponse?.tenantSlug ?: ""
    val userName: String get()     = authResponse?.fullName ?: ""
    val userEmail: String get()    = authResponse?.email ?: ""
    val initials: String get()     = authResponse?.initials ?: ""
    val membershipNumber: String?  get() = authResponse?.membershipNumber
    val roleDisplay: String get()  = authResponse?.roleDisplay ?: ""

    init {
        viewModelScope.launch {
            val saved = storage.loadAuth()
            _authState.value = if (saved != null) {
                BookItApiService.setToken(saved.accessToken)
                AuthState.BiometricLock(saved)  // require biometric/PIN before showing app
            } else {
                AuthState.Unauthenticated
            }
        }
    }

    // -------------------------------------------------------------------------
    // Actions
    // -------------------------------------------------------------------------

    /** Called after a successful biometric or PIN challenge. */
    fun unlockBiometric() {
        val locked = _authState.value as? AuthState.BiometricLock ?: return
        _authState.value = AuthState.Authenticated(locked.auth)
    }

    /** Called on a fresh sign-in (no lock needed this session). */
    fun signIn(auth: AuthResponse) {
        storage.saveAuth(auth)
        BookItApiService.setToken(auth.accessToken)
        _authState.value = AuthState.Authenticated(auth)
    }

    fun signOut() {
        storage.clearAuth()
        BookItApiService.clearToken()
        _authState.value = AuthState.Unauthenticated
    }
}

// ---------------------------------------------------------------------------
// Auth state
// ---------------------------------------------------------------------------

sealed interface AuthState {
    data object Loading           : AuthState
    data object Unauthenticated   : AuthState
    data class  BiometricLock(val auth: AuthResponse) : AuthState
    data class  Authenticated(val auth: AuthResponse) : AuthState
}
