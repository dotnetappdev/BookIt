import SwiftUI

/// Dashboard — stat cards + today's and upcoming appointments.
/// Mirrors `DashboardPage.razor` and `DashboardView` shared component.
struct DashboardView: View {

    @EnvironmentObject var authStore: AuthStore

    @State private var todayAppointments: [AppointmentResponse]    = []
    @State private var upcomingAppointments: [AppointmentResponse] = []
    @State private var isLoading = true
    @State private var error: String?

    private let upcomingDays = 14

    private var stats: [StatCard] {
        [
            StatCard(value: "\(todayAppointments.count)",    label: "Today",    icon: "calendar.day.timeline.left", color: Color(hex: "6c5ce7")),
            StatCard(value: "\(upcomingAppointments.count)", label: "Upcoming", icon: "clock.fill",                color: Color(hex: "3b82f6")),
            StatCard(value: "\(confirmedCount)",             label: "Confirmed",icon: "checkmark.seal.fill",       color: Color(hex: "10b981")),
            StatCard(value: "\(pendingCount)",               label: "Pending",  icon: "hourglass",                 color: Color(hex: "f59e0b")),
        ]
    }

    private var confirmedCount: Int { (todayAppointments + upcomingAppointments).filter { $0.isConfirmed }.count }
    private var pendingCount:   Int { (todayAppointments + upcomingAppointments).filter { $0.isPending  }.count }

    var body: some View {
        NavigationView {
            ZStack {
                Color(hex: "1a1a1a").ignoresSafeArea()

                if isLoading {
                    ProgressView()
                        .tint(Color(hex: "a29bfe"))
                } else {
                    ScrollView {
                        VStack(alignment: .leading, spacing: 20) {

                            // Greeting
                            VStack(alignment: .leading, spacing: 4) {
                                Text("Good \(greeting), \(authStore.userName.components(separatedBy: " ").first ?? "there") 👋")
                                    .font(.title2).fontWeight(.bold).foregroundColor(.white)
                                Text(Date(), style: .date)
                                    .font(.subheadline).foregroundColor(Color(hex: "9e9e9e"))
                            }
                            .padding(.horizontal)

                            // Stat cards grid
                            LazyVGrid(
                                columns: [GridItem(.flexible()), GridItem(.flexible())],
                                spacing: 12
                            ) {
                                ForEach(stats) { card in
                                    StatCardView(card: card)
                                }
                            }
                            .padding(.horizontal)

                            // Today's schedule
                            if !todayAppointments.isEmpty {
                                SectionHeader(title: "Today's Schedule")
                                ForEach(todayAppointments) { apt in
                                    AppointmentRowView(appointment: apt)
                                        .padding(.horizontal)
                                }
                            }

                            // Upcoming
                            if !upcomingAppointments.isEmpty {
                                SectionHeader(title: "Upcoming")
                                ForEach(upcomingAppointments.prefix(5)) { apt in
                                    AppointmentRowView(appointment: apt)
                                        .padding(.horizontal)
                                }
                            }

                            if todayAppointments.isEmpty && upcomingAppointments.isEmpty {
                                EmptyStateView(
                                    icon: "calendar.badge.exclamationmark",
                                    title: "No appointments",
                                    subtitle: "Your upcoming bookings will appear here"
                                )
                            }

                            Spacer(minLength: 30)
                        }
                        .padding(.top)
                    }
                    .refreshable { await load() }
                }
            }
            .navigationTitle("Dashboard")
            .navigationBarTitleDisplayMode(.inline)
            .toolbarColorScheme(.dark, for: .navigationBar)
            .task { await load() }
        }
    }

    // MARK: - Helpers

    private var greeting: String {
        let hour = Calendar.current.component(.hour, from: Date())
        if hour < 12 { return "morning" }
        if hour < 17 { return "afternoon" }
        return "evening"
    }

    private func load() async {
        guard !authStore.tenantSlug.isEmpty else { return }
        isLoading = true
        defer { isLoading = false }
        do {
            let today = Calendar.current.startOfDay(for: Date())
            let all = try await BookItAPIService.shared.getAppointments(
                tenantSlug: authStore.tenantSlug,
                from: today,
                to: today.addingTimeInterval(Double(upcomingDays) * 86_400)
            )
            todayAppointments    = all.filter { Calendar.current.isDateInToday($0.startTime) }
            upcomingAppointments = all.filter { $0.startTime > Date() && !Calendar.current.isDateInToday($0.startTime) }
        } catch {
            self.error = error.localizedDescription
        }
    }
}

// MARK: - Stat Card

struct StatCard: Identifiable {
    let id = UUID()
    let value: String
    let label: String
    let icon: String
    let color: Color
}

struct StatCardView: View {
    let card: StatCard

    var body: some View {
        VStack(alignment: .leading, spacing: 12) {
            HStack {
                Image(systemName: card.icon)
                    .font(.title3)
                    .foregroundColor(card.color)
                Spacer()
            }
            Text(card.value)
                .font(.system(size: 28, weight: .heavy))
                .foregroundColor(.white)
            Text(card.label)
                .font(.caption)
                .foregroundColor(Color(hex: "9e9e9e"))
        }
        .padding()
        .background(Color(hex: "2a2a2a"))
        .cornerRadius(14)
    }
}

// MARK: - Reusable subviews

struct SectionHeader: View {
    let title: String
    var body: some View {
        Text(title)
            .font(.headline).fontWeight(.semibold)
            .foregroundColor(.white)
            .padding(.horizontal)
            .padding(.top, 4)
    }
}

struct EmptyStateView: View {
    let icon: String
    let title: String
    let subtitle: String

    var body: some View {
        VStack(spacing: 12) {
            Image(systemName: icon)
                .font(.system(size: 52))
                .foregroundColor(Color(hex: "4a4a4a"))
            Text(title)
                .font(.headline).fontWeight(.semibold)
                .foregroundColor(Color(hex: "9e9e9e"))
            Text(subtitle)
                .font(.caption)
                .foregroundColor(Color(hex: "6a6a6a"))
                .multilineTextAlignment(.center)
        }
        .frame(maxWidth: .infinity)
        .padding(40)
    }
}
