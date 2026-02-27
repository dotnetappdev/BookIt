package com.bookit.app.ui.screens

import android.Manifest
import android.content.ContentValues
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Environment
import android.provider.CalendarContract
import android.provider.MediaStore
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
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
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.core.content.ContextCompat
import androidx.core.content.FileProvider
import com.bookit.app.models.AppointmentResponse
import com.bookit.app.services.BookItApiService
import com.bookit.app.ui.components.AppointmentQRCardView
import com.bookit.app.ui.components.generateQrBitmap
import com.bookit.app.ui.theme.*
import com.bookit.app.viewmodels.AuthViewModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.io.File
import java.io.FileOutputStream
import java.time.LocalDateTime
import java.time.ZoneId
import java.time.format.DateTimeFormatter

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun WalletScreen(authViewModel: AuthViewModel) {
    val context = LocalContext.current
    val scope   = rememberCoroutineScope()

    var appointments     by remember { mutableStateOf<List<AppointmentResponse>>(emptyList()) }
    var tenantName       by remember { mutableStateOf("BookIt") }
    var isLoading        by remember { mutableStateOf(true) }
    var actionMessage    by remember { mutableStateOf<Pair<String, Boolean>?>(null) }  // message to isError

    suspend fun load() {
        if (authViewModel.tenantSlug.isEmpty()) return
        isLoading = true
        try {
            runCatching {
                val tenant = BookItApiService.getTenant(authViewModel.tenantSlug)
                tenantName = tenant.name
            }
            appointments = BookItApiService
                .getAppointments(authViewModel.tenantSlug, LocalDateTime.now(),
                    LocalDateTime.now().plusDays(90))
                .filter { !it.isCancelled }
                .sortedBy { it.startDateTime() }
        } catch (_: Exception) { } finally { isLoading = false }
    }

    LaunchedEffect(Unit) { load() }

    val pullState = rememberPullToRefreshState()
    if (pullState.isRefreshing) {
        LaunchedEffect(Unit) { load(); pullState.endRefresh() }
    }

    // Calendar permission launcher
    val calendarPermission = rememberLauncherForActivityResult(
        ActivityResultContracts.RequestMultiplePermissions()
    ) { results ->
        if (results.values.all { it }) {
            appointments.firstOrNull()?.let { apt ->
                scope.launch {
                    val msg = addToCalendar(context, apt, tenantName)
                    actionMessage = msg
                }
            }
        } else {
            actionMessage = "Calendar permission denied." to true
        }
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Wallet", fontWeight = FontWeight.Bold) },
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
            } else {
                LazyColumn(
                    contentPadding = PaddingValues(16.dp),
                    verticalArrangement = Arrangement.spacedBy(16.dp)
                ) {
                    val next = appointments.firstOrNull()

                    if (next != null) {
                        // QR pass card
                        item {
                            AppointmentQRCardView(
                                appointment = next,
                                businessName = tenantName,
                                membershipNumber = authViewModel.membershipNumber
                            )
                        }

                        // Action buttons
                        item {
                            Row(
                                horizontalArrangement = Arrangement.spacedBy(12.dp),
                                modifier = Modifier.fillMaxWidth()
                            ) {
                                // Add to Calendar
                                Button(
                                    onClick = {
                                        val hasRead = ContextCompat.checkSelfPermission(
                                            context, Manifest.permission.READ_CALENDAR) == PackageManager.PERMISSION_GRANTED
                                        val hasWrite = ContextCompat.checkSelfPermission(
                                            context, Manifest.permission.WRITE_CALENDAR) == PackageManager.PERMISSION_GRANTED
                                        if (hasRead && hasWrite) {
                                            scope.launch {
                                                actionMessage = addToCalendar(context, next, tenantName)
                                            }
                                        } else {
                                            calendarPermission.launch(arrayOf(
                                                Manifest.permission.READ_CALENDAR,
                                                Manifest.permission.WRITE_CALENDAR
                                            ))
                                        }
                                    },
                                    modifier = Modifier.weight(1f).height(44.dp),
                                    colors = ButtonDefaults.buttonColors(containerColor = Purple700),
                                    shape = RoundedCornerShape(12.dp)
                                ) {
                                    Icon(Icons.Default.CalendarToday, null, Modifier.size(16.dp))
                                    Spacer(Modifier.width(6.dp))
                                    Text("Add to Calendar", fontSize = 13.sp, fontWeight = FontWeight.Bold)
                                }

                                // Share pass
                                OutlinedButton(
                                    onClick = {
                                        scope.launch {
                                            sharePass(context, next, authViewModel.membershipNumber)
                                        }
                                    },
                                    modifier = Modifier.weight(1f).height(44.dp),
                                    shape = RoundedCornerShape(12.dp),
                                    colors = ButtonDefaults.outlinedButtonColors(contentColor = Purple400)
                                ) {
                                    Icon(Icons.Default.Share, null, Modifier.size(16.dp))
                                    Spacer(Modifier.width(6.dp))
                                    Text("Share Pass", fontSize = 13.sp, fontWeight = FontWeight.SemiBold)
                                }
                            }
                        }

                        // Action result message
                        actionMessage?.let { (msg, isError) ->
                            item {
                                Row(
                                    modifier = Modifier
                                        .fillMaxWidth()
                                        .background(
                                            (if (isError) Color.Red else Color(0xFF10b981)).copy(alpha = 0.12f),
                                            RoundedCornerShape(8.dp)
                                        )
                                        .padding(12.dp),
                                    horizontalArrangement = Arrangement.spacedBy(8.dp),
                                    verticalAlignment = Alignment.CenterVertically
                                ) {
                                    val icon = if (isError) Icons.Default.Warning else Icons.Default.CheckCircle
                                    val color = if (isError) Color(0xFFff6b6b) else Color(0xFF10b981)
                                    Icon(icon, null, tint = color)
                                    Text(msg, color = color, fontSize = 13.sp)
                                }
                            }
                        }

                        // Remaining bookings
                        if (appointments.size > 1) {
                            item {
                                Text(
                                    "${appointments.size - 1} more upcoming booking${if (appointments.size > 2) "s" else ""}",
                                    fontSize = 14.sp, fontWeight = FontWeight.SemiBold, color = TextPrimary
                                )
                            }
                            items(appointments.drop(1).take(3)) { apt ->
                                com.bookit.app.ui.components.AppointmentRowView(apt)
                            }
                        }
                    } else {
                        item {
                            Box(Modifier.fillParentMaxSize(), contentAlignment = Alignment.Center) {
                                EmptyState("No upcoming bookings")
                            }
                        }
                    }
                }
            }
        }
    }
}

// ---------------------------------------------------------------------------
// Calendar integration
// ---------------------------------------------------------------------------

private suspend fun addToCalendar(
    context: Context,
    apt: AppointmentResponse,
    tenantName: String
): Pair<String, Boolean> = withContext(Dispatchers.IO) {
    return@withContext try {
        val startDt = apt.startDateTime()
            ?: return@withContext ("Invalid date." to true)
        val startMillis = startDt
            .atZone(ZoneId.systemDefault()).toInstant().toEpochMilli()
        val endMillis = startDt
            .plusMinutes(apt.services.sumOf { it.durationMinutes }.toLong().coerceAtLeast(30))
            .atZone(ZoneId.systemDefault()).toInstant().toEpochMilli()

        val intent = Intent(Intent.ACTION_INSERT).apply {
            data = CalendarContract.Events.CONTENT_URI
            putExtra(CalendarContract.Events.TITLE, "${apt.serviceNames.ifEmpty { "Appointment" }} @ $tenantName")
            putExtra(CalendarContract.Events.DESCRIPTION, buildString {
                append("Booking with $tenantName")
                apt.bookingPin?.let { append("\nPIN: $it") }
                apt.staffName?.takeIf { it.isNotEmpty() }?.let { append("\nWith: $it") }
            })
            apt.location?.let { putExtra(CalendarContract.Events.EVENT_LOCATION, it) }
            putExtra(CalendarContract.EXTRA_EVENT_BEGIN_TIME, startMillis)
            putExtra(CalendarContract.EXTRA_EVENT_END_TIME, endMillis)
            flags = Intent.FLAG_ACTIVITY_NEW_TASK
        }
        context.startActivity(intent)
        "Calendar app opened — confirm to save event." to false
    } catch (e: Exception) {
        "Could not open calendar: ${e.message}" to true
    }
}

// ---------------------------------------------------------------------------
// Share pass
// ---------------------------------------------------------------------------

private suspend fun sharePass(
    context: Context,
    apt: AppointmentResponse,
    membershipNumber: String?
) = withContext(Dispatchers.IO) {
    val qrContent = apt.qrContent(membershipNumber)
    val bitmap = generateQrBitmap(qrContent, 512) ?: return@withContext
    try {
        val file = File(context.cacheDir, "bookit_pass_${apt.id}.png")
        FileOutputStream(file).use { bitmap.compress(android.graphics.Bitmap.CompressFormat.PNG, 100, it) }
        val uri = FileProvider.getUriForFile(context, "${context.packageName}.fileprovider", file)
        val intent = Intent(Intent.ACTION_SEND).apply {
            type = "image/png"
            putExtra(Intent.EXTRA_STREAM, uri)
            putExtra(Intent.EXTRA_TEXT, "BookIt Pass — $qrContent")
            addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION or Intent.FLAG_ACTIVITY_NEW_TASK)
        }
        context.startActivity(Intent.createChooser(intent, "Share BookIt Pass").apply {
            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
        })
    } catch (_: Exception) { }
}
