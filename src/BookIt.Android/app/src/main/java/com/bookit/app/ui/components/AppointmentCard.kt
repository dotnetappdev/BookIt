package com.bookit.app.ui.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.bookit.app.models.AppointmentResponse
import com.bookit.app.ui.theme.*
import java.time.format.DateTimeFormatter

/**
 * Branded QR pass card — mirrors `AppointmentQRCardView.swift`.
 */
@Composable
fun AppointmentQRCardView(
    appointment: AppointmentResponse,
    businessName: String,
    membershipNumber: String?,
    modifier: Modifier = Modifier
) {
    Box(
        modifier = modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(20.dp))
            .background(
                Brush.verticalGradient(
                    colors = listOf(Color(0xFF2d2d3e), Color(0xFF1a1a2e))
                )
            )
            .padding(24.dp)
    ) {
        Column(verticalArrangement = Arrangement.spacedBy(20.dp)) {

            // Header
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Column {
                    Text(
                        text = businessName,
                        fontSize = 18.sp,
                        fontWeight = FontWeight.Bold,
                        color = TextPrimary
                    )
                    Text(
                        text = "Booking Pass",
                        fontSize = 12.sp,
                        color = Purple400
                    )
                }
                StatusBadge(
                    status = appointment.statusDisplay,
                    isPending = appointment.isPending,
                    isCancelled = appointment.isCancelled
                )
            }

            // Service + time
            Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
                Text(
                    text = appointment.serviceNames.ifEmpty { "Appointment" },
                    fontSize = 16.sp,
                    fontWeight = FontWeight.SemiBold,
                    color = TextPrimary
                )
                appointment.startDateTime()?.let { dt ->
                    val fmt = DateTimeFormatter.ofPattern("EEE d MMM yyyy · HH:mm")
                    Text(
                        text = dt.format(fmt),
                        fontSize = 13.sp,
                        color = TextMuted
                    )
                }
                appointment.staffName?.takeIf { it.isNotEmpty() }?.let {
                    Text(text = "With $it", fontSize = 13.sp, color = TextMuted)
                }
            }

            // QR code centred
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .clip(RoundedCornerShape(16.dp))
                    .background(Color.White)
                    .padding(16.dp),
                contentAlignment = Alignment.Center
            ) {
                QRCodeView(
                    content = appointment.qrContent(membershipNumber),
                    size = 180.dp
                )
            }

            // PIN + membership footer
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                appointment.bookingPin?.let {
                    Column {
                        Text("PIN", fontSize = 10.sp, color = TextFaint)
                        Text(it, fontSize = 16.sp, fontWeight = FontWeight.Bold, color = Purple400)
                    }
                }
                membershipNumber?.let {
                    Column(horizontalAlignment = Alignment.End) {
                        Text("Membership", fontSize = 10.sp, color = TextFaint)
                        Text(it, fontSize = 14.sp, fontWeight = FontWeight.SemiBold, color = TextPrimary)
                    }
                }
            }
        }
    }
}

// ---------------------------------------------------------------------------
// Reusable status badge
// ---------------------------------------------------------------------------

@Composable
fun StatusBadge(
    status: String,
    isPending: Boolean,
    isCancelled: Boolean,
    modifier: Modifier = Modifier
) {
    val (bg, fg) = when {
        isCancelled -> Color(0xFFff6b6b).copy(alpha = 0.2f) to Color(0xFFff6b6b)
        isPending   -> Amber500.copy(alpha = 0.2f) to Amber500
        else        -> Green500.copy(alpha = 0.2f) to Green500
    }
    Box(
        modifier = modifier
            .clip(RoundedCornerShape(20.dp))
            .background(bg)
            .padding(horizontal = 10.dp, vertical = 4.dp)
    ) {
        Text(text = status, fontSize = 11.sp, fontWeight = FontWeight.SemiBold, color = fg)
    }
}

// ---------------------------------------------------------------------------
// Appointment row (used in Bookings and Dashboard lists)
// ---------------------------------------------------------------------------

@Composable
fun AppointmentRowView(
    appointment: AppointmentResponse,
    modifier: Modifier = Modifier
) {
    Row(
        modifier = modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(12.dp))
            .background(Surface700)
            .padding(16.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        // Time column
        appointment.startDateTime()?.let { dt ->
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                modifier = Modifier.width(48.dp)
            ) {
                Text(
                    dt.format(DateTimeFormatter.ofPattern("HH:mm")),
                    fontSize = 14.sp, fontWeight = FontWeight.Bold, color = Purple400
                )
                Text(
                    dt.format(DateTimeFormatter.ofPattern("MMM d")),
                    fontSize = 10.sp, color = TextMuted
                )
            }
        }

        // Details
        Column(modifier = Modifier.weight(1f), verticalArrangement = Arrangement.spacedBy(2.dp)) {
            Text(
                text = appointment.serviceNames.ifEmpty { "Appointment" },
                fontSize = 14.sp, fontWeight = FontWeight.SemiBold, color = TextPrimary
            )
            Text(
                text = appointment.customerName,
                fontSize = 12.sp, color = TextMuted
            )
            appointment.staffName?.takeIf { it.isNotEmpty() }?.let {
                Text("With $it", fontSize = 11.sp, color = TextFaint)
            }
        }

        StatusBadge(
            status = appointment.statusDisplay,
            isPending = appointment.isPending,
            isCancelled = appointment.isCancelled
        )
    }
}
