import Foundation
import LocalAuthentication

/// Wraps `LAContext` to provide Face ID / Touch ID / passcode authentication.
///
/// - `availableBiometricType` reports `.faceID`, `.touchID`, or `.none`
/// - `authenticateWithBiometrics()` first tries biometrics; if that fails
///   (or if biometrics aren't enrolled) it transparently falls through to the
///   device passcode via `.deviceOwnerAuthentication`.
@MainActor
class BiometricAuthService: ObservableObject {

    enum BiometricType {
        case faceID
        case touchID
        case none
    }

    // MARK: - Available biometric type

    var availableBiometricType: BiometricType {
        let ctx = LAContext()
        var error: NSError?
        guard ctx.canEvaluatePolicy(.deviceOwnerAuthenticationWithBiometrics, error: &error) else {
            return .none
        }
        switch ctx.biometryType {
        case .faceID:  return .faceID
        case .touchID: return .touchID
        default:       return .none
        }
    }

    var isBiometricAvailable: Bool {
        availableBiometricType != .none
    }

    // MARK: - Authenticate

    /// Attempts biometric authentication first.  If unavailable or failed,
    /// falls back to the device passcode automatically (via `deviceOwnerAuthentication`).
    ///
    /// - Returns: `true` on success, `false` if the user cancelled or no credential matches.
    func authenticate(reason: String = "Verify your identity to access BookIt") async -> Bool {
        let ctx = LAContext()
        ctx.localizedFallbackTitle = "Use Passcode"
        ctx.localizedCancelTitle   = "Cancel"

        // Prefer biometrics; LAContext will offer "Use Passcode" as the fallback button.
        // If the device has no biometrics enrolled at all, we drop to passcode-only policy.
        let policy: LAPolicy = isBiometricAvailable
            ? .deviceOwnerAuthenticationWithBiometrics
            : .deviceOwnerAuthentication

        do {
            return try await ctx.evaluatePolicy(policy, localizedReason: reason)
        } catch {
            // User cancelled, authentication failed, or biometrics not enrolled.
            // `.deviceOwnerAuthenticationWithBiometrics` already shows "Use Passcode"
            // as the system fallback button — no manual second prompt needed.
            return false
        }
    }

    // MARK: - Passcode-only authentication

    /// Authenticates using the device passcode only (no biometric prompt).
    func authenticatePasscodeOnly(reason: String = "Enter your passcode to access BookIt") async -> Bool {
        return await authenticateWithPasscode(reason: reason)
    }

    // MARK: - Passcode fallback

    private func authenticateWithPasscode(reason: String) async -> Bool {
        let ctx = LAContext()
        ctx.localizedCancelTitle = "Cancel"
        do {
            return try await ctx.evaluatePolicy(.deviceOwnerAuthentication, localizedReason: reason)
        } catch {
            return false
        }
    }
}
