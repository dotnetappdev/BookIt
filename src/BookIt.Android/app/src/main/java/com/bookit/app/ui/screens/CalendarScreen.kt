package com.bookit.app.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material.icons.automirrored.filled.ArrowForward
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.bookit.app.models.AppointmentResponse
import com.bookit.app.services.BookItApiService
import com.bookit.app.ui.components.AppointmentRowView
import com.bookit.app.ui.theme.*
import com.bookit.app.viewmodels.AuthViewModel
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.YearMonth
import java.time.format.DateTimeFormatter
import java.time.format.TextStyle
import java.util.Locale

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun CalendarScreen(authViewModel: AuthViewModel) {
    var currentMonth   by remember { mutableStateOf(YearMonth.now()) }
    var selectedDate   by remember { mutableStateOf<LocalDate?>(LocalDate.now()) }
    var appointments   by remember { mutableStateOf<List<AppointmentResponse>>(emptyList()) }
    var isLoading      by remember { mutableStateOf(true) }

    // Load appointments for ±1 month window around current month
    LaunchedEffect(currentMonth) {
        if (authViewModel.tenantSlug.isEmpty()) return@LaunchedEffect
        isLoading = true
        try {
            val from = currentMonth.atDay(1).atStartOfDay().minusDays(1)
            val to   = currentMonth.atEndOfMonth().atTime(23, 59)
            appointments = BookItApiService.getAppointments(authViewModel.tenantSlug, from, to)
        } catch (_: Exception) { /* show empty */ }
        finally { isLoading = false }
    }

    // Days in the current month that have bookings
    val bookedDays = appointments
        .mapNotNull { it.startDateTime()?.toLocalDate() }
        .toSet()

    val dayAppointments = appointments
        .filter { it.startDateTime()?.toLocalDate() == selectedDate }
        .sortedBy { it.startDateTime() }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Calendar", fontWeight = FontWeight.Bold) },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = Surface800, titleContentColor = TextPrimary)
            )
        },
        containerColor = Surface800
    ) { padding ->
        Column(
            modifier = Modifier
                .padding(padding)
                .fillMaxSize()
                .padding(16.dp)
        ) {
            // Month navigation
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                IconButton(onClick = { currentMonth = currentMonth.minusMonths(1) }) {
                    Icon(Icons.AutoMirrored.Filled.ArrowBack, null, tint = Purple400)
                }
                Text(
                    currentMonth.format(DateTimeFormatter.ofPattern("MMMM yyyy")),
                    fontSize = 18.sp, fontWeight = FontWeight.Bold, color = TextPrimary
                )
                IconButton(onClick = { currentMonth = currentMonth.plusMonths(1) }) {
                    Icon(Icons.AutoMirrored.Filled.ArrowForward, null, tint = Purple400)
                }
            }

            Spacer(Modifier.height(8.dp))

            // Day-of-week headers (localised)
            Row(Modifier.fillMaxWidth()) {
                java.time.DayOfWeek.values().forEach { dow ->
                    Text(
                        dow.getDisplayName(java.time.format.TextStyle.SHORT, java.util.Locale.getDefault()),
                        modifier = Modifier.weight(1f),
                        textAlign = TextAlign.Center,
                        fontSize = 11.sp, color = TextMuted, fontWeight = FontWeight.Medium
                    )
                }
            }

            Spacer(Modifier.height(4.dp))

            // Month grid
            MonthGrid(
                yearMonth = currentMonth,
                selectedDate = selectedDate,
                bookedDays = bookedDays,
                onDaySelected = { selectedDate = it }
            )

            Spacer(Modifier.height(16.dp))

            // Appointments for selected day
            Text(
                selectedDate?.format(DateTimeFormatter.ofPattern("EEEE, d MMMM")) ?: "Select a day",
                fontSize = 15.sp, fontWeight = FontWeight.SemiBold, color = TextPrimary
            )
            Spacer(Modifier.height(8.dp))

            if (isLoading) {
                Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
                    CircularProgressIndicator(color = Purple400)
                }
            } else if (dayAppointments.isEmpty()) {
                EmptyState("No bookings on this day")
            } else {
                LazyColumn(verticalArrangement = Arrangement.spacedBy(8.dp)) {
                    items(dayAppointments) { AppointmentRowView(it) }
                }
            }
        }
    }
}

@Composable
private fun MonthGrid(
    yearMonth: YearMonth,
    selectedDate: LocalDate?,
    bookedDays: Set<LocalDate>,
    onDaySelected: (LocalDate) -> Unit
) {
    val firstDay   = yearMonth.atDay(1)
    // Monday = 1 … Sunday = 7
    val startOffset = (firstDay.dayOfWeek.value - 1)
    val daysInMonth = yearMonth.lengthOfMonth()

    val cells = startOffset + daysInMonth
    val rows  = (cells + 6) / 7

    Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
        for (row in 0 until rows) {
            Row(Modifier.fillMaxWidth()) {
                for (col in 0 until 7) {
                    val index = row * 7 + col
                    val dayNum = index - startOffset + 1
                    if (dayNum < 1 || dayNum > daysInMonth) {
                        Spacer(Modifier.weight(1f).aspectRatio(1f))
                    } else {
                        val date     = yearMonth.atDay(dayNum)
                        val isToday  = date == LocalDate.now()
                        val isSelected = date == selectedDate
                        val hasBooking = date in bookedDays

                        Box(
                            modifier = Modifier
                                .weight(1f)
                                .aspectRatio(1f)
                                .padding(2.dp)
                                .clip(CircleShape)
                                .background(
                                    when {
                                        isSelected -> Purple700
                                        isToday    -> Purple400.copy(alpha = 0.25f)
                                        else       -> Color.Transparent
                                    }
                                )
                                .clickable { onDaySelected(date) },
                            contentAlignment = Alignment.Center
                        ) {
                            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                                Text(
                                    dayNum.toString(),
                                    fontSize = 13.sp,
                                    fontWeight = if (isToday || isSelected) FontWeight.Bold else FontWeight.Normal,
                                    color = when {
                                        isSelected -> Color.White
                                        isToday    -> Purple400
                                        else       -> TextPrimary
                                    }
                                )
                                if (hasBooking) {
                                    Box(
                                        Modifier.size(4.dp).clip(CircleShape)
                                            .background(if (isSelected) Color.White else Purple400)
                                    )
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
