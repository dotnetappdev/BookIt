import SwiftUI

/// Bookings list — upcoming appointments with per-item QR code button.
/// Mirrors `AppointmentsPage.razor`.
struct BookingsView: View {

    @EnvironmentObject var authStore: AuthStore

    @State private var appointments: [AppointmentResponse] = []
    @State private var isLoading    = true
    @State private var tenantName   = "BookIt"
    @State private var selectedAppointment: AppointmentResponse?
    @State private var showQRSheet  = false

    private let lookbackDays = 30
    private let lookaheadDays = 90

    var body: some View {
        NavigationView {
            ZStack {
                Color(hex: "1a1a1a").ignoresSafeArea()

                if isLoading {
                    ProgressView().tint(Color(hex: "a29bfe"))
                } else if appointments.isEmpty {
                    EmptyStateView(
                        icon: "calendar.badge.exclamationmark",
                        title: "No upcoming bookings",
                        subtitle: "Book a service to get started"
                    )
                } else {
                    List {
                        ForEach(appointments.sorted { $0.startTime < $1.startTime }) { apt in
                            BookingListRow(appointment: apt) {
                                selectedAppointment = apt
                                showQRSheet = true
                            }
                            .listRowBackground(Color(hex: "1a1a1a"))
                            .listRowSeparator(.hidden)
                        }
                    }
                    .listStyle(.plain)
                    .scrollContentBackground(.hidden)
                    .refreshable { await load() }
                }
            }
            .navigationTitle("My Bookings")
            .navigationBarTitleDisplayMode(.inline)
            .toolbarColorScheme(.dark, for: .navigationBar)
            .task { await load() }
            .sheet(isPresented: $showQRSheet) {
                if let apt = selectedAppointment {
                    QRSheetView(
                        appointment: apt,
                        businessName: tenantName,
                        membershipNumber: authStore.membershipNumber
                    )
                }
            }
        }
    }

    private func load() async {
        guard !authStore.tenantSlug.isEmpty else { return }
        isLoading = true
        defer { isLoading = false }
        do {
            if let tenant = try? await BookItAPIService.shared.getTenant(slug: authStore.tenantSlug) {
                tenantName = tenant.name
            }
            appointments = try await BookItAPIService.shared.getAppointments(
                tenantSlug: authStore.tenantSlug,
                from: Date().addingTimeInterval(Double(-lookbackDays) * 86_400),
                to: Date().addingTimeInterval(Double(lookaheadDays) * 86_400)
            )
        } catch {
            // silently ignore; list stays empty
        }
    }
}

// MARK: - Booking row card

struct BookingListRow: View {
    let appointment: AppointmentResponse
    let onQRTap: () -> Void

    var body: some View {
        HStack(spacing: 16) {

            // Date block
            VStack(spacing: 2) {
                Text("\(Calendar.current.component(.day, from: appointment.startTime))")
                    .font(.system(size: 22, weight: .heavy))
                    .foregroundColor(.white)
                Text(appointment.startTime.formatted(.dateTime.month(.abbreviated)))
                    .font(.system(size: 10, weight: .semibold))
                    .textCase(.uppercase)
                    .foregroundColor(.white)
            }
            .frame(width: 50, height: 58)
            .background(Color(hex: "6c5ce7"))
            .cornerRadius(10)

            // Details
            VStack(alignment: .leading, spacing: 4) {
                Text(appointment.serviceNames.isEmpty ? "Appointment" : appointment.serviceNames)
                    .font(.system(size: 15, weight: .bold))
                    .foregroundColor(.white)
                    .lineLimit(1)

                Text(timeRange(appointment))
                    .font(.caption)
                    .foregroundColor(Color(hex: "9e9e9e"))

                HStack(spacing: 6) {
                    StatusBadge(status: appointment.statusDisplay, isPending: appointment.isPending, isCancelled: appointment.isCancelled)

                    if appointment.bookingPin != nil {
                        Button(action: onQRTap) {
                            Label("QR", systemImage: "qrcode")
                                .font(.system(size: 11, weight: .semibold))
                                .foregroundColor(Color(hex: "a29bfe"))
                                .padding(.horizontal, 8)
                                .padding(.vertical, 3)
                                .overlay(
                                    RoundedRectangle(cornerRadius: 6)
                                        .stroke(Color(hex: "a29bfe"), lineWidth: 1)
                                )
                        }
                        .buttonStyle(.plain)
                    }
                }
            }

            Spacer()

            // Amount
            Text("£\(appointment.totalAmount as NSDecimalNumber, formatter: currencyFormatter)")
                .font(.system(size: 14, weight: .bold))
                .foregroundColor(.white)
        }
        .padding()
        .background(Color(hex: "2a2a2a"))
        .cornerRadius(12)
        .padding(.horizontal)
        .padding(.vertical, 4)
    }

    private func timeRange(_ apt: AppointmentResponse) -> String {
        let fmt = DateFormatter()
        fmt.dateFormat = "h:mm a"
        let start = fmt.string(from: apt.startTime)
        let end   = fmt.string(from: apt.endTime)
        var result = "\(start) – \(end)"
        if let staff = apt.staffName, !staff.isEmpty { result += " · \(staff)" }
        return result
    }
}

private let currencyFormatter: NumberFormatter = {
    let f = NumberFormatter()
    f.numberStyle = .decimal
    f.minimumFractionDigits = 2
    f.maximumFractionDigits = 2
    return f
}()

// MARK: - Status badge

struct StatusBadge: View {
    let status: String
    let isPending: Bool
    let isCancelled: Bool

    private var badgeColor: Color {
        if isCancelled { return .red }
        if isPending    { return Color(hex: "f59e0b") }
        return Color(hex: "10b981")
    }

    var body: some View {
        Text(status)
            .font(.system(size: 10, weight: .bold))
            .foregroundColor(.white)
            .padding(.horizontal, 8)
            .padding(.vertical, 3)
            .background(badgeColor.opacity(0.85))
            .cornerRadius(6)
    }
}

// MARK: - QR Sheet

struct QRSheetView: View {
    let appointment: AppointmentResponse
    let businessName: String
    let membershipNumber: String?

    @Environment(\.dismiss) private var dismiss

    var body: some View {
        NavigationView {
            ZStack {
                Color(hex: "1a1a1a").ignoresSafeArea()
                AppointmentQRCardView(
                    appointment: appointment,
                    businessName: businessName,
                    membershipNumber: membershipNumber
                )
                .padding()
            }
            .navigationTitle("Booking QR")
            .navigationBarTitleDisplayMode(.inline)
            .toolbarColorScheme(.dark, for: .navigationBar)
            .toolbar {
                ToolbarItem(placement: .cancellationAction) {
                    Button("Close") { dismiss() }
                        .foregroundColor(Color(hex: "a29bfe"))
                }
            }
        }
    }
}
