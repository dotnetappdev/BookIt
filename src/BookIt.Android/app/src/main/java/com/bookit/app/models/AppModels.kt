package com.bookit.app.models

import kotlinx.serialization.Serializable
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

// ---------------------------------------------------------------------------
// Auth
// ---------------------------------------------------------------------------

@Serializable
data class LoginRequest(
    val email: String,
    val password: String,
    val tenantSlug: String? = null
)

@Serializable
data class RegisterRequest(
    val email: String,
    val password: String,
    val firstName: String,
    val lastName: String,
    val membershipNumber: String? = null,
    val tenantSlug: String? = null
)

@Serializable
data class AuthResponse(
    val accessToken: String,
    val refreshToken: String,
    val expiresAt: String,           // ISO-8601 string — parsed on demand
    val userId: String,
    val email: String,
    val fullName: String,
    val role: Int,                   // UserRole enum value
    val tenantId: String,
    val tenantSlug: String,
    val membershipNumber: String? = null
) {
    val initials: String
        get() = fullName.trim().split(" ")
            .take(2)
            .mapNotNull { it.firstOrNull()?.uppercaseChar()?.toString() }
            .joinToString("")

    val roleDisplay: String
        get() = when (role) {
            1 -> "Super Admin"
            2 -> "Admin"
            3 -> "Manager"
            4 -> "Staff"
            5 -> "Customer"
            else -> "User"
        }
}

// ---------------------------------------------------------------------------
// Appointment
// ---------------------------------------------------------------------------

@Serializable
data class AppointmentResponse(
    val id: String,
    val tenantId: String,
    val customerName: String,
    val customerEmail: String,
    val customerPhone: String? = null,
    val startTime: String,           // ISO-8601 string
    val endTime: String,
    val status: Int,
    val paymentStatus: Int,
    val totalAmount: Double,
    val meetingType: Int,
    val meetingLink: String? = null,
    val location: String? = null,
    val confirmationToken: String? = null,
    val bookingPin: String? = null,
    val services: List<ServiceSummary> = emptyList(),
    val staffName: String? = null
) {
    val statusDisplay: String
        get() = when (status) {
            1 -> "Pending"
            2 -> "Confirmed"
            3 -> "Cancelled"
            4 -> "Completed"
            5 -> "No Show"
            6 -> "Rescheduled"
            else -> "Unknown"
        }

    val isPending:   Boolean get() = status == 1
    val isConfirmed: Boolean get() = status == 2
    val isCancelled: Boolean get() = status == 3

    val serviceNames: String get() = services.joinToString(", ") { it.name }

    /** QR content: BOOKIT:{id}:{pin}:{startYYYYMMDDHHmm}:{membership|NONE} */
    fun qrContent(membershipNumber: String?): String {
        val formatter = DateTimeFormatter.ofPattern("yyyyMMddHHmm")
        val dateStr = try {
            LocalDateTime.parse(startTime, DateTimeFormatter.ISO_DATE_TIME).format(formatter)
        } catch (_: Exception) {
            try { LocalDateTime.parse(startTime.take(19)).format(formatter) }
            catch (_: Exception) { "000000000000" }
        }
        val pin = bookingPin.orEmpty()
        val membership = membershipNumber ?: "NONE"
        return "BOOKIT:$id:$pin:$dateStr:$membership"
    }

    fun startDateTime(): LocalDateTime? = try {
        // Handle both "2025-01-15T14:30:00" and "2025-01-15T14:30:00.000Z" / "+00:00" forms
        LocalDateTime.parse(startTime, DateTimeFormatter.ISO_DATE_TIME)
    } catch (_: Exception) {
        try { LocalDateTime.parse(startTime.take(19)) } catch (_: Exception) { null }
    }
}

@Serializable
data class ServiceSummary(
    val id: String,
    val name: String,
    val price: Double,
    val durationMinutes: Int
)

// ---------------------------------------------------------------------------
// Tenant
// ---------------------------------------------------------------------------

@Serializable
data class TenantResponse(
    val id: String,
    val name: String,
    val slug: String,
    val logoUrl: String? = null,
    val address: String? = null,
    val city: String? = null,
    val postCode: String? = null,
    val currency: String? = null,
    val timeZone: String? = null,
    val contactEmail: String? = null,
    val contactPhone: String? = null
)
