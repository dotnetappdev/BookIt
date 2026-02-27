import SwiftUI
import EventKit

/// QR Wallet — branded pass card with Add to Calendar and Share Pass actions.
/// Mirrors `QrWalletPage.razor` and `WalletPassService`.
struct WalletView: View {

    @EnvironmentObject var authStore: AuthStore

    @State private var appointments: [AppointmentResponse] = []
    @State private var nextAppointment: AppointmentResponse?
    @State private var tenantName  = "BookIt"
    @State private var logoUrl: String?
    @State private var isLoading   = true
    @State private var actionBusy  = false
    @State private var actionMessage: String?
    @State private var actionIsError = false
    @State private var showShareSheet = false
    @State private var shareItems: [Any] = []

    private let lookaheadDays = 90

    var body: some View {
        NavigationView {
            ZStack {
                Color(hex: "1a1a1a").ignoresSafeArea()

                if isLoading {
                    ProgressView().tint(Color(hex: "a29bfe"))
                } else {
                    ScrollView {
                        VStack(spacing: 20) {

                            if let next = nextAppointment {

                                // QR pass card
                                AppointmentQRCardView(
                                    appointment: next,
                                    businessName: tenantName,
                                    logoUrl: logoUrl,
                                    membershipNumber: authStore.membershipNumber
                                )
                                .padding(.horizontal)

                                // Action buttons
                                HStack(spacing: 12) {
                                    WalletActionButton(
                                        title: "Add to Calendar",
                                        icon: "calendar.badge.plus",
                                        isBusy: actionBusy
                                    ) {
                                        await addToCalendar(next)
                                    }
                                    WalletActionButton(
                                        title: "Share Pass",
                                        icon: "square.and.arrow.up",
                                        isBusy: actionBusy,
                                        isOutlined: true
                                    ) {
                                        await sharePass(next)
                                    }
                                }
                                .padding(.horizontal)

                                // Action result message
                                if let msg = actionMessage {
                                    HStack(spacing: 8) {
                                        Image(systemName: actionIsError ? "exclamationmark.triangle.fill" : "checkmark.circle.fill")
                                            .foregroundColor(actionIsError ? .red : Color(hex: "10b981"))
                                        Text(msg)
                                            .font(.caption)
                                            .foregroundColor(actionIsError ? Color(hex: "ff6b6b") : Color(hex: "10b981"))
                                        Spacer()
                                    }
                                    .padding(12)
                                    .background((actionIsError ? Color.red : Color(hex: "10b981")).opacity(0.12))
                                    .cornerRadius(8)
                                    .padding(.horizontal)
                                }

                                // Additional upcoming bookings
                                if appointments.count > 1 {
                                    SectionHeader(title: "\(appointments.count - 1) more upcoming booking\(appointments.count > 2 ? "s" : "")")

                                    ForEach(appointments.dropFirst().prefix(3)) { apt in
                                        UpcomingMiniRow(
                                            appointment: apt,
                                            onCalendarTap: { Task { await addToCalendar(apt) } }
                                        )
                                        .padding(.horizontal)
                                    }
                                }

                            } else {
                                EmptyStateView(
                                    icon: "wallet.pass",
                                    title: "No upcoming bookings",
                                    subtitle: "Your next appointment QR pass will appear here"
                                )
                            }

                            Spacer(minLength: 30)
                        }
                        .padding(.top)
                    }
                    .refreshable { await load() }
                }
            }
            .navigationTitle("Wallet")
            .navigationBarTitleDisplayMode(.inline)
            .toolbarColorScheme(.dark, for: .navigationBar)
            .task { await load() }
            .sheet(isPresented: $showShareSheet) {
                ShareSheet(items: shareItems)
            }
        }
    }

    // MARK: - Load

    private func load() async {
        guard !authStore.tenantSlug.isEmpty else { return }
        isLoading = true
        defer { isLoading = false }
        do {
            if let tenant = try? await BookItAPIService.shared.getTenant(slug: authStore.tenantSlug) {
                tenantName = tenant.name
                logoUrl    = tenant.logoUrl
            }
            let all = try await BookItAPIService.shared.getAppointments(
                tenantSlug: authStore.tenantSlug,
                from: Date(),
                to: Date().addingTimeInterval(Double(lookaheadDays) * 86_400)
            )
            appointments = all
                .filter { !$0.isCancelled }
                .sorted { $0.startTime < $1.startTime }
            nextAppointment = appointments.first
        } catch {
            // silently; empty state shown
        }
    }

    // MARK: - Add to Calendar (EventKit)

    private func addToCalendar(_ apt: AppointmentResponse) async {
        actionBusy = true
        actionMessage = nil
        defer { actionBusy = false }

        let store = EKEventStore()
        do {
            if #available(iOS 17.0, *) {
                try await store.requestWriteOnlyAccessToEvents()
            } else {
                try await store.requestAccess(to: .event)
            }

            let event = EKEvent(eventStore: store)
            event.title     = apt.serviceNames.isEmpty ? "Appointment @ \(tenantName)" : "\(apt.serviceNames) @ \(tenantName)"
            event.startDate = apt.startTime
            event.endDate   = apt.endTime
            event.calendar  = store.defaultCalendarForNewEvents

            var notes = "Booking with \(tenantName)"
            if let pin = apt.bookingPin         { notes += "\nPIN: \(pin)" }
            if let mem = authStore.membershipNumber { notes += "\nMembership No: \(mem)" }
            if let staff = apt.staffName, !staff.isEmpty { notes += "\nWith: \(staff)" }
            event.notes = notes

            if let location = apt.location { event.location = location }

            try store.save(event, span: .thisEvent)

            actionMessage = "Calendar event saved — check your Calendar app."
            actionIsError = false
        } catch {
            actionMessage = "Could not add to calendar: \(error.localizedDescription)"
            actionIsError = true
        }
    }

    // MARK: - Share Pass

    private func sharePass(_ apt: AppointmentResponse) async {
        actionBusy = true
        defer { actionBusy = false }

        let qrContent = apt.qrContent(membershipNumber: authStore.membershipNumber)
        guard let qrImage = QRCodeGenerator.generateUIImage(from: qrContent, size: 300) else {
            actionMessage = "Could not generate QR code."
            actionIsError = true
            return
        }

        shareItems = [qrImage, "BookIt Pass – \(tenantName)"]
        showShareSheet = true
    }
}

// MARK: - Wallet action button

struct WalletActionButton: View {
    let title: String
    let icon: String
    let isBusy: Bool
    var isOutlined = false
    let action: () async -> Void

    var body: some View {
        Button {
            Task { await action() }
        } label: {
            HStack(spacing: 6) {
                Image(systemName: icon)
                Text(title)
                    .font(.system(size: 14, weight: .bold))
            }
            .foregroundColor(isOutlined ? Color(hex: "a29bfe") : .white)
            .frame(maxWidth: .infinity)
            .frame(height: 44)
            .background(
                isOutlined
                    ? Color.clear
                    : LinearGradient(colors: [Color(hex: "6c5ce7"), Color(hex: "a29bfe")],
                                     startPoint: .leading, endPoint: .trailing)
            )
            .overlay(
                RoundedRectangle(cornerRadius: 12)
                    .stroke(isOutlined ? Color(hex: "a29bfe") : Color.clear, lineWidth: 1.5)
            )
            .cornerRadius(12)
        }
        .disabled(isBusy)
    }
}

// MARK: - Upcoming mini row

struct UpcomingMiniRow: View {
    let appointment: AppointmentResponse
    let onCalendarTap: () -> Void

    var body: some View {
        HStack {
            VStack(alignment: .leading, spacing: 3) {
                Text(appointment.serviceNames.isEmpty ? "Appointment" : appointment.serviceNames)
                    .font(.system(size: 14, weight: .semibold))
                    .foregroundColor(.white)
                Text("\(appointment.startTime.formatted(.dateTime.weekday(.abbreviated).day().month(.abbreviated))) · \(appointment.startTime.formatted(.dateTime.hour().minute()))")
                    .font(.caption)
                    .foregroundColor(Color(hex: "9e9e9e"))
            }
            Spacer()
            StatusBadge(
                status: appointment.statusDisplay,
                isPending: appointment.isPending,
                isCancelled: appointment.isCancelled
            )
            Button(action: onCalendarTap) {
                Image(systemName: "calendar.badge.plus")
                    .foregroundColor(Color(hex: "a29bfe"))
            }
            .buttonStyle(.plain)
        }
        .padding()
        .background(Color(hex: "2a2a2a"))
        .cornerRadius(12)
    }
}

// MARK: - UIActivityViewController wrapper

struct ShareSheet: UIViewControllerRepresentable {
    let items: [Any]

    func makeUIViewController(context: Context) -> UIActivityViewController {
        UIActivityViewController(activityItems: items, applicationActivities: nil)
    }

    func updateUIViewController(_ uiViewController: UIActivityViewController, context: Context) {}
}
