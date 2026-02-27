import SwiftUI

@main
struct BookItApp: App {

    @StateObject private var authStore = AuthStore()

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environmentObject(authStore)
                .preferredColorScheme(.dark)
        }
    }
}
