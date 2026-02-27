import SwiftUI

/// A compact appointment row used on the Dashboard and Calendar screens.
struct AppointmentRowView: View {

    let appointment: AppointmentResponse

    private let timeFormatter: DateFormatter = {
        let f = DateFormatter()
        f.dateFormat = "h:mm a"
        return f
    }()

    var body: some View {
        HStack(spacing: 14) {

            // Time column
            VStack(spacing: 2) {
                Text(timeFormatter.string(from: appointment.startTime))
                    .font(.system(size: 13, weight: .bold))
                    .foregroundColor(Color(hex: "a29bfe"))
                Text(timeFormatter.string(from: appointment.endTime))
                    .font(.system(size: 11))
                    .foregroundColor(Color(hex: "6a6a6a"))
            }
            .frame(width: 60, alignment: .center)

            // Accent line
            Rectangle()
                .fill(accentColor)
                .frame(width: 3)
                .cornerRadius(2)

            // Details
            VStack(alignment: .leading, spacing: 3) {
                Text(appointment.serviceNames.isEmpty ? "Appointment" : appointment.serviceNames)
                    .font(.system(size: 14, weight: .semibold))
                    .foregroundColor(.white)
                    .lineLimit(1)

                if !appointment.customerName.isEmpty {
                    Text(appointment.customerName)
                        .font(.caption)
                        .foregroundColor(Color(hex: "9e9e9e"))
                }

                if let staff = appointment.staffName, !staff.isEmpty {
                    Text("With \(staff)")
                        .font(.caption)
                        .foregroundColor(Color(hex: "6a6a6a"))
                }
            }

            Spacer()

            StatusBadge(
                status: appointment.statusDisplay,
                isPending: appointment.isPending,
                isCancelled: appointment.isCancelled
            )
        }
        .padding()
        .background(Color(hex: "2a2a2a"))
        .cornerRadius(12)
    }

    private var accentColor: Color {
        if appointment.isCancelled { return .red }
        if appointment.isPending   { return Color(hex: "f59e0b") }
        return Color(hex: "10b981")
    }
}
