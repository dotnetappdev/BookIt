import SwiftUI

/// Root view — shows Login when unauthenticated, biometric lock when the session
/// needs verification, or the main tab bar when fully authenticated.
struct ContentView: View {

    @EnvironmentObject var authStore: AuthStore

    /// Shared biometric service — created once and passed down.
    @StateObject private var biometricService = BiometricAuthService()

    var body: some View {
        if authStore.isAuthenticated {
            if authStore.requiresBiometricUnlock {
                BiometricLockView(biometricService: biometricService)
            } else {
                MainTabView()
            }
        } else {
            LoginView()
        }
    }
}

/// Five-tab bottom navigation matching the MAUI app.
struct MainTabView: View {

    var body: some View {
        TabView {
            DashboardView()
                .tabItem {
                    Label("Home", systemImage: "house.fill")
                }

            CalendarView()
                .tabItem {
                    Label("Calendar", systemImage: "calendar")
                }

            BookingsView()
                .tabItem {
                    Label("Bookings", systemImage: "note.text")
                }

            WalletView()
                .tabItem {
                    Label("Wallet", systemImage: "qrcode")
                }

            ProfileView()
                .tabItem {
                    Label("Profile", systemImage: "person.fill")
                }
        }
        .accentColor(Color("AccentColor"))
    }
}
