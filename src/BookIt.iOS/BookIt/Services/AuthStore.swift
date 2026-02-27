import Foundation
import SwiftUI

/// Observable auth state — mirrors `BookItAuthState` from the C# shared library.
/// Persists the token in the Keychain so the user stays logged in between launches.
@MainActor
class AuthStore: ObservableObject {

    @Published var isAuthenticated = false
    @Published var authResponse: AuthResponse?

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
            Task {
                await BookItAPIService.shared.setToken(saved.accessToken)
            }
        }
    }

    // MARK: - Auth actions

    func signIn(_ auth: AuthResponse) {
        authResponse = auth
        isAuthenticated = true
        keychain.saveAuth(auth)
        Task {
            await BookItAPIService.shared.setToken(auth.accessToken)
        }
    }

    func signOut() {
        authResponse = nil
        isAuthenticated = false
        keychain.clearAuth()
        Task {
            await BookItAPIService.shared.clearToken()
        }
    }
}
