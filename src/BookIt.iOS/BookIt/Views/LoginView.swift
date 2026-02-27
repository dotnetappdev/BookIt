import SwiftUI

/// Sign-in / Sign-up screen — mirrors `LoginPage.razor`.
struct LoginView: View {

    @EnvironmentObject var authStore: AuthStore

    @State private var selectedTab = 0          // 0 = Sign In, 1 = Sign Up

    // Sign In
    @State private var loginEmail    = ""
    @State private var loginPassword = ""
    @State private var showPassword  = false
    @State private var loginError: String?

    // Sign Up
    @State private var regFirstName       = ""
    @State private var regLastName        = ""
    @State private var regEmail           = ""
    @State private var regPassword        = ""
    @State private var regMembership      = ""
    @State private var regError: String?

    @State private var isLoading = false

    var body: some View {
        ZStack {
            Color(hex: "1a1a1a").ignoresSafeArea()

            ScrollView {
                VStack(spacing: 24) {

                    // ── Logo + title ──
                    VStack(spacing: 12) {
                        ZStack {
                            RoundedRectangle(cornerRadius: 20)
                                .fill(
                                    LinearGradient(
                                        colors: [Color(hex: "6c5ce7"), Color(hex: "a29bfe")],
                                        startPoint: .topLeading,
                                        endPoint: .bottomTrailing
                                    )
                                )
                                .frame(width: 72, height: 72)
                            Image(systemName: "calendar")
                                .font(.system(size: 36))
                                .foregroundColor(.white)
                        }

                        Text("BookIt")
                            .font(.system(size: 32, weight: .heavy))
                            .foregroundColor(.white)

                        Text("Sign in to manage your bookings")
                            .font(.subheadline)
                            .foregroundColor(Color(hex: "9e9e9e"))
                    }
                    .padding(.top, 48)

                    // ── Tab picker ──
                    Picker("", selection: $selectedTab) {
                        Text("Sign In").tag(0)
                        Text("Sign Up").tag(1)
                    }
                    .pickerStyle(.segmented)
                    .padding(.horizontal)

                    // ── Form card ──
                    VStack(spacing: 16) {
                        if selectedTab == 0 {
                            signInForm
                        } else {
                            signUpForm
                        }
                    }
                    .padding(24)
                    .background(Color(hex: "2a2a2a"))
                    .cornerRadius(16)
                    .padding(.horizontal)

                    Spacer(minLength: 40)
                }
            }
        }
    }

    // MARK: - Sign In

    private var signInForm: some View {
        VStack(spacing: 16) {
            BookItTextField(
                icon: "envelope.fill",
                placeholder: "Email",
                text: $loginEmail,
                keyboardType: .emailAddress,
                autocapitalization: .never
            )

            ZStack(alignment: .trailing) {
                BookItTextField(
                    icon: "lock.fill",
                    placeholder: "Password",
                    text: $loginPassword,
                    isSecure: !showPassword
                )
                Button {
                    showPassword.toggle()
                } label: {
                    Image(systemName: showPassword ? "eye.slash.fill" : "eye.fill")
                        .foregroundColor(Color(hex: "9e9e9e"))
                        .padding(.trailing, 16)
                }
            }

            if let error = loginError {
                ErrorBanner(message: error)
            }

            BookItPrimaryButton(title: "Sign In", isLoading: isLoading) {
                await performSignIn()
            }
        }
    }

    // MARK: - Sign Up

    private var signUpForm: some View {
        VStack(spacing: 16) {
            HStack(spacing: 12) {
                BookItTextField(icon: "person.fill", placeholder: "First name", text: $regFirstName)
                BookItTextField(icon: "person.fill", placeholder: "Last name",  text: $regLastName)
            }

            BookItTextField(
                icon: "envelope.fill",
                placeholder: "Email",
                text: $regEmail,
                keyboardType: .emailAddress,
                autocapitalization: .never
            )

            BookItTextField(
                icon: "lock.fill",
                placeholder: "Password",
                text: $regPassword,
                isSecure: true
            )

            BookItTextField(
                icon: "creditcard.fill",
                placeholder: "Membership number (optional)",
                text: $regMembership
            )

            if let error = regError {
                ErrorBanner(message: error)
            }

            BookItPrimaryButton(title: "Create Account", isLoading: isLoading) {
                await performSignUp()
            }
        }
    }

    // MARK: - Actions

    private func performSignIn() async {
        guard !isLoading else { return }
        isLoading = true
        loginError = nil
        defer { isLoading = false }

        do {
            let auth = try await BookItAPIService.shared.login(
                email: loginEmail.trimmingCharacters(in: .whitespaces),
                password: loginPassword
            )
            authStore.signIn(auth)
        } catch let error as APIError {
            loginError = error.errorDescription
        } catch {
            loginError = "Unable to connect. Please check your internet connection."
        }
    }

    private func performSignUp() async {
        guard !isLoading else { return }
        guard !regFirstName.isEmpty, !regLastName.isEmpty else {
            regError = "First and last name are required."
            return
        }
        isLoading = true
        regError = nil
        defer { isLoading = false }

        do {
            let auth = try await BookItAPIService.shared.register(
                email: regEmail.trimmingCharacters(in: .whitespaces),
                password: regPassword,
                firstName: regFirstName,
                lastName: regLastName,
                membershipNumber: regMembership.isEmpty ? nil : regMembership
            )
            authStore.signIn(auth)
        } catch let error as APIError {
            regError = error.errorDescription
        } catch {
            regError = "Unable to connect. Please check your internet connection."
        }
    }
}

// MARK: - Shared Form Components

struct BookItTextField: View {
    let icon: String
    let placeholder: String
    @Binding var text: String
    var keyboardType: UIKeyboardType    = .default
    var autocapitalization: TextInputAutocapitalization = .sentences
    var isSecure = false

    var body: some View {
        HStack(spacing: 12) {
            Image(systemName: icon)
                .foregroundColor(Color(hex: "9e9e9e"))
                .frame(width: 20)
            if isSecure {
                SecureField(placeholder, text: $text)
                    .foregroundColor(.white)
                    .autocorrectionDisabled()
            } else {
                TextField(placeholder, text: $text)
                    .keyboardType(keyboardType)
                    .textInputAutocapitalization(autocapitalization)
                    .autocorrectionDisabled()
                    .foregroundColor(.white)
            }
        }
        .padding()
        .background(Color(hex: "3a3a3a"))
        .cornerRadius(10)
        .overlay(
            RoundedRectangle(cornerRadius: 10)
                .stroke(Color(hex: "4a4a4a"), lineWidth: 1)
        )
    }
}

struct BookItPrimaryButton: View {
    let title: String
    let isLoading: Bool
    let action: () async -> Void

    var body: some View {
        Button {
            Task { await action() }
        } label: {
            HStack {
                if isLoading {
                    ProgressView()
                        .progressViewStyle(.circular)
                        .tint(.white)
                        .scaleEffect(0.8)
                }
                Text(title)
                    .font(.system(size: 16, weight: .bold))
                    .foregroundColor(.white)
            }
            .frame(maxWidth: .infinity)
            .frame(height: 48)
            .background(
                LinearGradient(
                    colors: [Color(hex: "6c5ce7"), Color(hex: "a29bfe")],
                    startPoint: .leading,
                    endPoint: .trailing
                )
            )
            .cornerRadius(12)
        }
        .disabled(isLoading)
    }
}

struct ErrorBanner: View {
    let message: String

    var body: some View {
        HStack(spacing: 8) {
            Image(systemName: "exclamationmark.triangle.fill")
                .foregroundColor(.red)
            Text(message)
                .font(.caption)
                .foregroundColor(Color(hex: "ff6b6b"))
            Spacer()
        }
        .padding(12)
        .background(Color.red.opacity(0.15))
        .cornerRadius(8)
    }
}

// MARK: - Color helper

extension Color {
    init(hex: String) {
        let hex = hex.trimmingCharacters(in: CharacterSet.alphanumerics.inverted)
        var int: UInt64 = 0
        Scanner(string: hex).scanHexInt64(&int)
        let a, r, g, b: UInt64
        switch hex.count {
        case 3:
            (a, r, g, b) = (255, (int >> 8) * 17, (int >> 4 & 0xF) * 17, (int & 0xF) * 17)
        case 6:
            (a, r, g, b) = (255, int >> 16, int >> 8 & 0xFF, int & 0xFF)
        case 8:
            (a, r, g, b) = (int >> 24, int >> 16 & 0xFF, int >> 8 & 0xFF, int & 0xFF)
        default:
            (a, r, g, b) = (255, 0, 0, 0)
        }
        self.init(
            .sRGB,
            red:   Double(r) / 255,
            green: Double(g) / 255,
            blue:  Double(b) / 255,
            opacity: Double(a) / 255
        )
    }
}
