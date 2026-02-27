import SwiftUI

/// Wallet-style branded QR pass card — mirrors `AppointmentQrCard` Blazor component.
///
/// Shows:
/// - Business logo / name
/// - Customer name + membership number
/// - Service name, date/time, staff
/// - QR code (CoreImage) encoding `BOOKIT:{id}:{pin}:{YYYYMMDDHHmm}:{membership|NONE}`
/// - Booking PIN
struct AppointmentQRCardView: View {

    let appointment: AppointmentResponse
    let businessName: String
    var logoUrl: String?
    var membershipNumber: String?

    var body: some View {
        VStack(spacing: 0) {

            // ── Card header ──
            VStack(spacing: 8) {
                HStack {
                    // Business logo placeholder
                    ZStack {
                        RoundedRectangle(cornerRadius: 10)
                            .fill(Color(hex: "6c5ce7").opacity(0.3))
                            .frame(width: 44, height: 44)
                        Image(systemName: "calendar.circle.fill")
                            .font(.system(size: 26))
                            .foregroundColor(Color(hex: "a29bfe"))
                    }

                    VStack(alignment: .leading, spacing: 2) {
                        Text(businessName)
                            .font(.headline).fontWeight(.heavy)
                            .foregroundColor(.white)
                        Text("Booking Pass")
                            .font(.caption)
                            .foregroundColor(Color(hex: "9e9e9e"))
                    }

                    Spacer()

                    // Membership number badge
                    if let membership = membershipNumber, !membership.isEmpty {
                        VStack(spacing: 2) {
                            Text("MEMBER")
                                .font(.system(size: 8, weight: .bold))
                                .foregroundColor(Color(hex: "9e9e9e"))
                            Text(membership)
                                .font(.system(size: 13, weight: .heavy))
                                .foregroundColor(Color(hex: "a29bfe"))
                        }
                        .padding(.horizontal, 10)
                        .padding(.vertical, 6)
                        .background(Color(hex: "6c5ce7").opacity(0.2))
                        .cornerRadius(8)
                    }
                }

                Divider().background(Color(hex: "3a3a3a"))
            }
            .padding()

            // ── Appointment details ──
            VStack(spacing: 12) {
                InfoRow(icon: "scissors", label: "Service", value: appointment.serviceNames.isEmpty ? "Appointment" : appointment.serviceNames)
                InfoRow(icon: "calendar",  label: "Date",    value: appointment.startTime.formatted(.dateTime.weekday(.wide).day().month(.wide).year()))
                InfoRow(icon: "clock",     label: "Time",    value: "\(appointment.startTime.formatted(.dateTime.hour().minute())) – \(appointment.endTime.formatted(.dateTime.hour().minute()))")

                if let staff = appointment.staffName, !staff.isEmpty {
                    InfoRow(icon: "person.fill", label: "With", value: staff)
                }
            }
            .padding(.horizontal)

            Divider()
                .background(Color(hex: "3a3a3a"))
                .padding(.horizontal)
                .padding(.vertical, 8)

            // ── QR Code ──
            VStack(spacing: 12) {
                QRCodeView(
                    content: appointment.qrContent(membershipNumber: membershipNumber),
                    size: 200
                )
                .cornerRadius(10)
                .padding(8)
                .background(.white)
                .cornerRadius(12)

                // PIN display
                if let pin = appointment.bookingPin, !pin.isEmpty {
                    HStack(spacing: 8) {
                        Image(systemName: "number.square.fill")
                            .foregroundColor(Color(hex: "a29bfe"))
                        Text("PIN: ")
                            .font(.caption)
                            .foregroundColor(Color(hex: "9e9e9e"))
                        Text(pin)
                            .font(.system(size: 20, weight: .heavy, design: .monospaced))
                            .foregroundColor(.white)
                            .tracking(4)
                    }
                    .padding(.vertical, 8)
                    .padding(.horizontal, 16)
                    .background(Color(hex: "3a3a3a"))
                    .cornerRadius(10)
                }

                // Status badge
                StatusBadge(
                    status: appointment.statusDisplay,
                    isPending: appointment.isPending,
                    isCancelled: appointment.isCancelled
                )
            }
            .padding()
        }
        .background(
            LinearGradient(
                colors: [Color(hex: "2a2a2a"), Color(hex: "1e1e1e")],
                startPoint: .top,
                endPoint: .bottom
            )
        )
        .cornerRadius(20)
        .overlay(
            RoundedRectangle(cornerRadius: 20)
                .stroke(Color(hex: "3a3a3a"), lineWidth: 1)
        )
        .shadow(color: Color(hex: "6c5ce7").opacity(0.3), radius: 12, x: 0, y: 4)
    }
}

// MARK: - Info row

private struct InfoRow: View {
    let icon: String
    let label: String
    let value: String

    var body: some View {
        HStack(spacing: 10) {
            Image(systemName: icon)
                .frame(width: 18)
                .foregroundColor(Color(hex: "a29bfe"))
            Text(label)
                .font(.caption)
                .foregroundColor(Color(hex: "9e9e9e"))
                .frame(width: 54, alignment: .leading)
            Text(value)
                .font(.subheadline).fontWeight(.semibold)
                .foregroundColor(.white)
            Spacer()
        }
    }
}
