package com.bookit.app.ui.screens

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.QrCode
import androidx.compose.material3.*
import androidx.compose.material3.pulltorefresh.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import com.bookit.app.models.AppointmentResponse
import com.bookit.app.services.BookItApiService
import com.bookit.app.ui.components.AppointmentQRCardView
import com.bookit.app.ui.components.AppointmentRowView
import com.bookit.app.ui.theme.*
import com.bookit.app.viewmodels.AuthViewModel
import kotlinx.coroutines.launch
import java.time.LocalDateTime

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun BookingsScreen(authViewModel: AuthViewModel) {
    val scope = rememberCoroutineScope()
    var appointments by remember { mutableStateOf<List<AppointmentResponse>>(emptyList()) }
    var isLoading    by remember { mutableStateOf(true) }
    var selectedQR   by remember { mutableStateOf<AppointmentResponse?>(null) }

    suspend fun load() {
        if (authViewModel.tenantSlug.isEmpty()) return
        isLoading = true
        try {
            val from = LocalDateTime.now().minusDays(1)
            val to   = LocalDateTime.now().plusDays(90)
            appointments = BookItApiService
                .getAppointments(authViewModel.tenantSlug, from, to)
                .sortedBy { it.startDateTime() }
        } catch (_: Exception) { } finally { isLoading = false }
    }

    LaunchedEffect(Unit) { load() }

    val pullState = rememberPullToRefreshState()
    if (pullState.isRefreshing) {
        LaunchedEffect(Unit) { load(); pullState.endRefresh() }
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Bookings", fontWeight = FontWeight.Bold) },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = Surface800, titleContentColor = TextPrimary)
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
            } else if (appointments.isEmpty()) {
                Box(Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    EmptyState("No bookings found")
                }
            } else {
                LazyColumn(
                    contentPadding = PaddingValues(16.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    items(appointments) { apt ->
                        Row(
                            verticalAlignment = Alignment.CenterVertically,
                            modifier = Modifier.fillMaxWidth()
                        ) {
                            AppointmentRowView(apt, modifier = Modifier.weight(1f))
                            Spacer(Modifier.width(8.dp))
                            IconButton(onClick = { selectedQR = apt }) {
                                Icon(Icons.Default.QrCode, contentDescription = "View QR",
                                    tint = Purple400)
                            }
                        }
                    }
                }
            }
        }
    }

    // QR modal
    selectedQR?.let { apt ->
        Dialog(onDismissRequest = { selectedQR = null }) {
            Surface(
                shape = androidx.compose.foundation.shape.RoundedCornerShape(20.dp),
                color = Surface700
            ) {
                Box(Modifier.padding(8.dp)) {
                    AppointmentQRCardView(
                        appointment = apt,
                        businessName = "BookIt",
                        membershipNumber = authViewModel.membershipNumber
                    )
                }
            }
        }
    }
}
