package com.bookit.app.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.material3.pulltorefresh.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.bookit.app.models.AppointmentResponse
import com.bookit.app.services.BookItApiService
import com.bookit.app.ui.components.AppointmentRowView
import com.bookit.app.ui.theme.*
import com.bookit.app.viewmodels.AuthViewModel
import kotlinx.coroutines.launch
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun DashboardScreen(authViewModel: AuthViewModel) {
    val scope = rememberCoroutineScope()
    var todayList    by remember { mutableStateOf<List<AppointmentResponse>>(emptyList()) }
    var upcomingList by remember { mutableStateOf<List<AppointmentResponse>>(emptyList()) }
    var isLoading    by remember { mutableStateOf(true) }
    var error        by remember { mutableStateOf<String?>(null) }

    val firstName = authViewModel.userName.split(" ").firstOrNull() ?: "there"
    val greeting  = when (LocalDateTime.now().hour) {
        in 0..11  -> "morning"
        in 12..16 -> "afternoon"
        else      -> "evening"
    }

    suspend fun load() {
        if (authViewModel.tenantSlug.isEmpty()) return
        isLoading = true; error = null
        try {
            val now   = LocalDateTime.now().withHour(0).withMinute(0).withSecond(0)
            val limit = now.plusDays(14)
            val all   = BookItApiService.getAppointments(authViewModel.tenantSlug, now, limit)
            todayList    = all.filter { it.startDateTime()?.toLocalDate() == now.toLocalDate() }
            upcomingList = all.filter { (it.startDateTime() ?: now) > LocalDateTime.now() &&
                                        it.startDateTime()?.toLocalDate() != now.toLocalDate() }
        } catch (e: Exception) {
            error = e.message
        } finally { isLoading = false }
    }

    LaunchedEffect(Unit) { load() }

    val pullState = rememberPullToRefreshState()
    if (pullState.isRefreshing) {
        LaunchedEffect(Unit) { load(); pullState.endRefresh() }
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Dashboard", fontWeight = FontWeight.Bold) },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = Surface800,
                    titleContentColor = TextPrimary
                )
            )
        },
        containerColor = Surface800
    ) { padding ->
        PullToRefreshBox(
            isRefreshing = pullState.isRefreshing,
            onRefresh = { scope.launch { load(); pullState.endRefresh() } },
            modifier = Modifier.padding(padding)
        ) {
            if (isLoading) {
                Box(Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    CircularProgressIndicator(color = Purple400)
                }
            } else {
                LazyColumn(
                    modifier = Modifier.fillMaxSize(),
                    contentPadding = PaddingValues(16.dp),
                    verticalArrangement = Arrangement.spacedBy(12.dp)
                ) {
                    // Greeting
                    item {
                        Text(
                            "Good $greeting, $firstName 👋",
                            fontSize = 22.sp, fontWeight = FontWeight.Bold, color = TextPrimary
                        )
                        Text(
                            LocalDateTime.now().format(DateTimeFormatter.ofPattern("EEEE, d MMMM yyyy")),
                            fontSize = 13.sp, color = TextMuted
                        )
                        Spacer(Modifier.height(4.dp))
                    }

                    // Stat cards grid (2 columns via Row)
                    item {
                        Row(
                            horizontalArrangement = Arrangement.spacedBy(12.dp),
                            modifier = Modifier.fillMaxWidth()
                        ) {
                            StatCard("Today",    todayList.size.toString(),   Icons.Default.Today,       Purple700, Modifier.weight(1f))
                            StatCard("Upcoming", upcomingList.size.toString(), Icons.Default.Schedule,   Color(0xFF3b82f6), Modifier.weight(1f))
                        }
                        Spacer(Modifier.height(12.dp))
                        Row(
                            horizontalArrangement = Arrangement.spacedBy(12.dp),
                            modifier = Modifier.fillMaxWidth()
                        ) {
                            val confirmed = (todayList + upcomingList).count { it.isConfirmed }
                            val pending   = (todayList + upcomingList).count { it.isPending  }
                            StatCard("Confirmed", confirmed.toString(), Icons.Default.CheckCircle, Color(0xFF10b981), Modifier.weight(1f))
                            StatCard("Pending",   pending.toString(),   Icons.Default.HourglassEmpty, Amber500, Modifier.weight(1f))
                        }
                    }

                    // Today
                    if (todayList.isNotEmpty()) {
                        item { SectionHeader("Today's Schedule") }
                        items(todayList) { AppointmentRowView(it) }
                    }

                    // Upcoming
                    if (upcomingList.isNotEmpty()) {
                        item { SectionHeader("Upcoming") }
                        items(upcomingList.take(5)) { AppointmentRowView(it) }
                    }

                    // Empty state
                    if (todayList.isEmpty() && upcomingList.isEmpty()) {
                        item { EmptyState("No upcoming appointments") }
                    }

                    error?.let { item { ErrorBanner(it) } }
                }
            }
        }
    }
}

// ---------------------------------------------------------------------------
// Shared sub-composables
// ---------------------------------------------------------------------------

@Composable
fun StatCard(
    label: String,
    value: String,
    icon: androidx.compose.ui.graphics.vector.ImageVector,
    color: Color,
    modifier: Modifier = Modifier
) {
    Card(
        modifier = modifier,
        colors = CardDefaults.cardColors(containerColor = Surface700),
        shape = RoundedCornerShape(14.dp)
    ) {
        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(8.dp)
        ) {
            Icon(icon, contentDescription = null, tint = color, modifier = Modifier.size(22.dp))
            Text(value, fontSize = 28.sp, fontWeight = FontWeight.ExtraBold, color = TextPrimary)
            Text(label, fontSize = 12.sp, color = TextMuted)
        }
    }
}

@Composable
fun SectionHeader(title: String) {
    Text(
        title,
        fontSize = 15.sp, fontWeight = FontWeight.SemiBold, color = TextPrimary,
        modifier = Modifier.padding(vertical = 4.dp)
    )
}

@Composable
fun EmptyState(message: String) {
    Box(
        modifier = Modifier.fillMaxWidth().padding(40.dp),
        contentAlignment = Alignment.Center
    ) {
        Text(message, color = TextMuted, fontSize = 14.sp)
    }
}
