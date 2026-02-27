import Foundation

// MARK: - Auth Models

struct LoginRequest: Encodable {
    let email: String
    let password: String
    let tenantSlug: String?
}

struct RegisterRequest: Encodable {
    let email: String
    let password: String
    let firstName: String
    let lastName: String
    let membershipNumber: String?
    let tenantSlug: String?
}

struct AuthResponse: Codable {
    let accessToken: String
    let refreshToken: String
    let expiresAt: Date
    let userId: UUID
    let email: String
    let fullName: String
    let role: Int          // UserRole enum value
    let tenantId: UUID
    let tenantSlug: String
    let membershipNumber: String?

    var initials: String {
        let parts = fullName.split(separator: " ")
        return parts.prefix(2).compactMap { $0.first.map(String.init) }.joined().uppercased()
    }

    var roleDisplay: String {
        switch role {
        case 1: return "Super Admin"
        case 2: return "Admin"
        case 3: return "Manager"
        case 4: return "Staff"
        case 5: return "Customer"
        default: return "User"
        }
    }
}

// MARK: - Appointment Models

struct AppointmentResponse: Codable, Identifiable {
    let id: UUID
    let tenantId: UUID
    let customerName: String
    let customerEmail: String
    let customerPhone: String?
    let startTime: Date
    let endTime: Date
    let status: Int         // AppointmentStatus enum value
    let paymentStatus: Int
    let totalAmount: Decimal
    let meetingType: Int
    let meetingLink: String?
    let location: String?
    let confirmationToken: String?
    let bookingPin: String?
    let services: [ServiceSummary]
    let staffName: String?

    var statusDisplay: String {
        switch status {
        case 1: return "Pending"
        case 2: return "Confirmed"
        case 3: return "Cancelled"
        case 4: return "Completed"
        case 5: return "No Show"
        case 6: return "Rescheduled"
        default: return "Unknown"
        }
    }

    var isPending: Bool { status == 1 }
    var isConfirmed: Bool { status == 2 }
    var isCancelled: Bool { status == 3 }

    var serviceNames: String {
        services.map(\.name).joined(separator: ", ")
    }

    /// QR content format: BOOKIT:{id}:{pin}:{startYYYYMMDDHHmm}:{membershipNumber|NONE}
    func qrContent(membershipNumber: String?) -> String {
        let formatter = DateFormatter()
        formatter.dateFormat = "yyyyMMddHHmm"
        let dateStr = formatter.string(from: startTime)
        let pin = bookingPin ?? ""
        let membership = membershipNumber ?? "NONE"
        return "BOOKIT:\(id):\(pin):\(dateStr):\(membership)"
    }
}

struct ServiceSummary: Codable, Identifiable {
    let id: UUID
    let name: String
    let price: Decimal
    let durationMinutes: Int
}

// MARK: - Tenant Model

struct TenantResponse: Codable {
    let id: UUID
    let name: String
    let slug: String
    let logoUrl: String?
    let address: String?
    let city: String?
    let postCode: String?
    let currency: String?
    let timeZone: String?
    let contactEmail: String?
    let contactPhone: String?
}

// MARK: - Dashboard Stats

struct DashboardStats {
    let todayCount: Int
    let upcomingCount: Int
    let pendingCount: Int
    let confirmedCount: Int
}
