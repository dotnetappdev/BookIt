import SwiftUI

/// Root view — shows Login when unauthenticated, main tab bar when authenticated.
struct ContentView: View {

    @EnvironmentObject var authStore: AuthStore

    var body: some View {
        if authStore.isAuthenticated {
            MainTabView()
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
