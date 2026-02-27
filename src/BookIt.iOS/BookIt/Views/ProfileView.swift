import SwiftUI

/// User profile — info card, preferences, sign-out.
/// Mirrors `ProfilePage.razor`.
struct ProfileView: View {

    @EnvironmentObject var authStore: AuthStore
    @State private var showSignOutAlert = false

    var body: some View {
        NavigationView {
            ZStack {
                Color(hex: "1a1a1a").ignoresSafeArea()

                ScrollView {
                    VStack(spacing: 20) {

                        // ── Avatar & name ──
                        VStack(spacing: 10) {
                            ZStack {
                                Circle()
                                    .fill(
                                        LinearGradient(
                                            colors: [Color(hex: "6c5ce7"), Color(hex: "a29bfe")],
                                            startPoint: .topLeading,
                                            endPoint: .bottomTrailing
                                        )
                                    )
                                    .frame(width: 80, height: 80)
                                Text(authStore.initials)
                                    .font(.system(size: 28, weight: .heavy))
                                    .foregroundColor(.white)
                            }

                            Text(authStore.userName)
                                .font(.title3).fontWeight(.bold).foregroundColor(.white)

                            Text(authStore.userEmail)
                                .font(.subheadline).foregroundColor(Color(hex: "9e9e9e"))

                            if !authStore.roleDisplay.isEmpty {
                                Text(authStore.roleDisplay)
                                    .font(.caption).fontWeight(.semibold)
                                    .foregroundColor(Color(hex: "a29bfe"))
                                    .padding(.horizontal, 12).padding(.vertical, 4)
                                    .background(Color(hex: "6c5ce7").opacity(0.2))
                                    .cornerRadius(8)
                            }
                        }
                        .padding(.top, 20)

                        Divider().background(Color(hex: "3a3a3a")).padding(.horizontal)

                        // ── Info card ──
                        ProfileInfoCard(rows: [
                            ProfileInfoRow(label: "Organisation", value: authStore.tenantSlug),
                            ProfileInfoRow(label: "Role", value: authStore.roleDisplay),
                            authStore.membershipNumber.map {
                                ProfileInfoRow(label: "Membership No.", value: $0)
                            },
                        ].compactMap { $0 })

                        // ── Preferences card ──
                        ProfileMenuCard(items: [
                            ProfileMenuItem(icon: "bell.fill",         title: "Notification preferences"),
                            ProfileMenuItem(icon: "lock.fill",         title: "Change password"),
                            ProfileMenuItem(icon: "trash.fill",        title: "Delete account", isDestructive: true),
                        ])

                        // ── Sign out ──
                        Button {
                            showSignOutAlert = true
                        } label: {
                            HStack {
                                Image(systemName: "arrow.right.square.fill")
                                Text("Sign Out")
                                    .fontWeight(.bold)
                            }
                            .foregroundColor(.red)
                            .frame(maxWidth: .infinity)
                            .frame(height: 48)
                            .overlay(
                                RoundedRectangle(cornerRadius: 12)
                                    .stroke(Color.red.opacity(0.6), lineWidth: 1.5)
                            )
                        }
                        .padding(.horizontal)
                        .alert("Sign Out", isPresented: $showSignOutAlert) {
                            Button("Sign Out", role: .destructive) { authStore.signOut() }
                            Button("Cancel", role: .cancel) {}
                        } message: {
                            Text("Are you sure you want to sign out?")
                        }

                        Spacer(minLength: 30)
                    }
                }
            }
            .navigationTitle("Profile")
            .navigationBarTitleDisplayMode(.inline)
            .toolbarColorScheme(.dark, for: .navigationBar)
        }
    }
}

// MARK: - Info card

struct ProfileInfoRow {
    let label: String
    let value: String
}

struct ProfileInfoCard: View {
    let rows: [ProfileInfoRow]

    var body: some View {
        VStack(spacing: 0) {
            ForEach(Array(rows.enumerated()), id: \.offset) { index, row in
                HStack {
                    Text(row.label)
                        .font(.subheadline)
                        .foregroundColor(Color(hex: "9e9e9e"))
                    Spacer()
                    Text(row.value)
                        .font(.subheadline).fontWeight(.semibold)
                        .foregroundColor(.white)
                }
                .padding(.horizontal, 16)
                .padding(.vertical, 12)

                if index < rows.count - 1 {
                    Divider().background(Color(hex: "3a3a3a")).padding(.horizontal, 16)
                }
            }
        }
        .background(Color(hex: "2a2a2a"))
        .cornerRadius(14)
        .padding(.horizontal)
    }
}

// MARK: - Menu card

struct ProfileMenuItem {
    let icon: String
    let title: String
    var isDestructive = false
}

struct ProfileMenuCard: View {
    let items: [ProfileMenuItem]

    var body: some View {
        VStack(spacing: 0) {
            ForEach(Array(items.enumerated()), id: \.offset) { index, item in
                Button {
                    // placeholder — implement individual settings flows
                } label: {
                    HStack(spacing: 14) {
                        Image(systemName: item.icon)
                            .frame(width: 20)
                            .foregroundColor(item.isDestructive ? .red : Color(hex: "a29bfe"))
                        Text(item.title)
                            .foregroundColor(item.isDestructive ? .red : .white)
                        Spacer()
                        Image(systemName: "chevron.right")
                            .font(.caption)
                            .foregroundColor(Color(hex: "6a6a6a"))
                    }
                    .padding(.horizontal, 16)
                    .padding(.vertical, 14)
                }

                if index < items.count - 1 {
                    Divider().background(Color(hex: "3a3a3a")).padding(.horizontal, 16)
                }
            }
        }
        .background(Color(hex: "2a2a2a"))
        .cornerRadius(14)
        .padding(.horizontal)
    }
}
