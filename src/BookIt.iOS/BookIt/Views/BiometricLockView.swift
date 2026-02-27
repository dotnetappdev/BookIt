import SwiftUI

/// Full-screen lock that appears when the app has a saved session and requires
/// biometric or passcode verification before revealing the main UI.
///
/// Shows:
/// - Face ID / Touch ID icon (adapts to device capability)
/// - Primary action button  — "Unlock with Face ID / Touch ID"
/// - Secondary button       — "Use Passcode" (always available)
struct BiometricLockView: View {

    @EnvironmentObject var authStore: AuthStore

    /// Injected so previews can supply a stub.
    var biometricService: BiometricAuthService

    @State private var isAuthenticating = false
    @State private var errorMessage: String?
    @State private var bounceIcon = false
    @State private var hasAutoTriggered = false

    private var biometricType: BiometricAuthService.BiometricType {
        biometricService.availableBiometricType
    }

    var body: some View {
        ZStack {
            // Background — same dark gradient as the rest of the app
            LinearGradient(
                colors: [Color(hex: "1a1a1a"), Color(hex: "0f0f0f")],
                startPoint: .top,
                endPoint: .bottom
            )
            .ignoresSafeArea()

            VStack(spacing: 32) {

                Spacer()

                // App logo
                VStack(spacing: 12) {
                    ZStack {
                        RoundedRectangle(cornerRadius: 24)
                            .fill(
                                LinearGradient(
                                    colors: [Color(hex: "6c5ce7"), Color(hex: "a29bfe")],
                                    startPoint: .topLeading,
                                    endPoint: .bottomTrailing
                                )
                            )
                            .frame(width: 84, height: 84)
                        Image(systemName: "calendar")
                            .font(.system(size: 40))
                            .foregroundColor(.white)
                    }

                    Text("BookIt")
                        .font(.system(size: 34, weight: .heavy))
                        .foregroundColor(.white)

                    Text("Your account is locked")
                        .font(.subheadline)
                        .foregroundColor(Color(hex: "9e9e9e"))
                }

                // Biometric icon — bounces when triggered
                VStack(spacing: 8) {
                    Image(systemName: biometricIconName)
                        .font(.system(size: 72, weight: .thin))
                        .foregroundColor(Color(hex: "a29bfe"))
                        .symbolEffect(.bounce, value: bounceIcon)
                        .padding(.bottom, 4)

                    Text(biometricLabel)
                        .font(.footnote)
                        .foregroundColor(Color(hex: "9e9e9e"))
                }

                // Error banner
                if let msg = errorMessage {
                    HStack(spacing: 8) {
                        Image(systemName: "exclamationmark.triangle.fill")
                            .foregroundColor(.red)
                        Text(msg)
                            .font(.caption)
                            .foregroundColor(Color(hex: "ff6b6b"))
                        Spacer()
                    }
                    .padding(12)
                    .background(Color.red.opacity(0.15))
                    .cornerRadius(10)
                    .padding(.horizontal, 32)
                }

                // Action buttons
                VStack(spacing: 12) {

                    // Primary — biometric (shown only when available)
                    if biometricType != .none {
                        Button {
                            Task { await attemptAuth(usePasscode: false) }
                        } label: {
                            HStack(spacing: 8) {
                                if isAuthenticating {
                                    ProgressView()
                                        .progressViewStyle(.circular)
                                        .tint(.white)
                                        .scaleEffect(0.8)
                                } else {
                                    Image(systemName: biometricIconName)
                                }
                                Text(biometricButtonTitle)
                                    .font(.system(size: 16, weight: .bold))
                            }
                            .foregroundColor(.white)
                            .frame(maxWidth: .infinity)
                            .frame(height: 52)
                            .background(
                                LinearGradient(
                                    colors: [Color(hex: "6c5ce7"), Color(hex: "a29bfe")],
                                    startPoint: .leading,
                                    endPoint: .trailing
                                )
                            )
                            .cornerRadius(14)
                        }
                        .disabled(isAuthenticating)
                    }

                    // Secondary — always-available passcode option
                    Button {
                        Task { await attemptAuth(usePasscode: true) }
                    } label: {
                        HStack(spacing: 8) {
                            Image(systemName: "lock.fill")
                            Text("Use Passcode")
                                .font(.system(size: 15, weight: .semibold))
                        }
                        .foregroundColor(Color(hex: "a29bfe"))
                        .frame(maxWidth: .infinity)
                        .frame(height: 48)
                        .overlay(
                            RoundedRectangle(cornerRadius: 14)
                                .stroke(Color(hex: "a29bfe").opacity(0.5), lineWidth: 1.5)
                        )
                    }
                    .disabled(isAuthenticating)

                    // Sign-out escape hatch
                    Button {
                        authStore.signOut()
                    } label: {
                        Text("Sign out")
                            .font(.footnote)
                            .foregroundColor(Color(hex: "6a6a6a"))
                    }
                    .padding(.top, 4)
                }
                .padding(.horizontal, 32)

                Spacer()
            }
        }
        .task {
            // Auto-trigger biometric prompt once on first appearance.
            // `.task` is automatically cancelled if the view disappears.
            guard !hasAutoTriggered else { return }
            hasAutoTriggered = true
            await attemptAuth(usePasscode: false)
        }
    }

    // MARK: - Actions

    private func attemptAuth(usePasscode: Bool) async {
        guard !isAuthenticating else { return }
        isAuthenticating = true
        errorMessage = nil
        bounceIcon.toggle()

        let success: Bool
        if usePasscode {
            // Force the passcode policy directly
            success = await biometricService.authenticatePasscodeOnly()
        } else {
            success = await biometricService.authenticate()
        }

        isAuthenticating = false

        if success {
            authStore.unlockBiometric()
        } else {
            errorMessage = usePasscode
                ? "Passcode verification failed. Please try again."
                : "Authentication failed. Tap below to try again."
        }
    }

    // MARK: - Computed helpers

    private var biometricIconName: String {
        switch biometricType {
        case .faceID:  return "faceid"
        case .touchID: return "touchid"
        case .none:    return "lock.fill"
        }
    }

    private var biometricLabel: String {
        switch biometricType {
        case .faceID:  return "Face ID available"
        case .touchID: return "Touch ID available"
        case .none:    return "Enter your passcode to continue"
        }
    }

    private var biometricButtonTitle: String {
        switch biometricType {
        case .faceID:  return "Unlock with Face ID"
        case .touchID: return "Unlock with Touch ID"
        case .none:    return "Unlock"
        }
    }
}
