import Foundation
import SwiftUI

/// Observable auth state — mirrors `BookItAuthState` from the C# shared library.
/// Persists the token in the Keychain so the user stays logged in between launches.
/// When a saved session exists the app requires biometric or passcode verification
/// before revealing the main UI (`requiresBiometricUnlock = true`).
@MainActor
class AuthStore: ObservableObject {

    @Published var isAuthenticated = false
    @Published var authResponse: AuthResponse?
    /// `true` when the user has a saved session but has not yet passed
    /// the biometric / passcode lock screen this launch.
    @Published var requiresBiometricUnlock = false

    private let keychain = KeychainService()

    // MARK: - Convenience accessors

    var accessToken: String  { authResponse?.accessToken ?? "" }
    var tenantSlug: String   { authResponse?.tenantSlug ?? "" }
    var userName: String     { authResponse?.fullName ?? "" }
    var userEmail: String    { authResponse?.email ?? "" }
    var initials: String     { authResponse?.initials ?? "" }
    var membershipNumber: String? { authResponse?.membershipNumber }
    var roleDisplay: String  { authResponse?.roleDisplay ?? "" }

    // MARK: - Init (restore saved session)

    init() {
        if let saved = keychain.loadAuth() {
            authResponse = saved
            isAuthenticated = true
            // Require biometric / passcode verification on every cold launch
            requiresBiometricUnlock = true
            Task {
                await BookItAPIService.shared.setToken(saved.accessToken)
            }
        }
    }

    // MARK: - Auth actions

    func signIn(_ auth: AuthResponse) {
        authResponse = auth
        isAuthenticated = true
        requiresBiometricUnlock = false   // fresh login — no lock needed this session
        keychain.saveAuth(auth)
        Task {
            await BookItAPIService.shared.setToken(auth.accessToken)
        }
    }

    /// Called by `BiometricLockView` once the user has passed Face ID / passcode.
    func unlockBiometric() {
        requiresBiometricUnlock = false
    }

    func signOut() {
        authResponse = nil
        isAuthenticated = false
        requiresBiometricUnlock = false
        keychain.clearAuth()
        Task {
            await BookItAPIService.shared.clearToken()
        }
    }
}
