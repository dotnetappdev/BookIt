package com.bookit.app.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.bookit.app.ui.theme.*
import com.bookit.app.viewmodels.AuthViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ProfileScreen(authViewModel: AuthViewModel) {
    var showSignOutDialog by remember { mutableStateOf(false) }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Profile", fontWeight = FontWeight.Bold) },
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
                .verticalScroll(rememberScrollState())
                .padding(20.dp),
            verticalArrangement = Arrangement.spacedBy(20.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {

            // Avatar
            Box(
                modifier = Modifier
                    .size(88.dp)
                    .clip(CircleShape)
                    .background(Brush.linearGradient(listOf(Purple700, Purple400))),
                contentAlignment = Alignment.Center
            ) {
                Text(
                    authViewModel.initials.ifEmpty { "?" },
                    fontSize = 32.sp, fontWeight = FontWeight.ExtraBold, color = Color.White
                )
            }

            // Name + email
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                Text(authViewModel.userName, fontSize = 22.sp,
                    fontWeight = FontWeight.Bold, color = TextPrimary)
                Text(authViewModel.userEmail, fontSize = 14.sp, color = TextMuted)
            }

            // Role badge
            Box(
                modifier = Modifier
                    .clip(RoundedCornerShape(20.dp))
                    .background(Purple700.copy(alpha = 0.25f))
                    .padding(horizontal = 16.dp, vertical = 6.dp)
            ) {
                Text(authViewModel.roleDisplay, fontSize = 13.sp,
                    fontWeight = FontWeight.SemiBold, color = Purple400)
            }

            // Info card
            Card(
                modifier = Modifier.fillMaxWidth(),
                colors = CardDefaults.cardColors(containerColor = Surface700),
                shape = RoundedCornerShape(14.dp)
            ) {
                Column(Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                    ProfileInfoRow(Icons.Default.Email, "Email", authViewModel.userEmail)
                    ProfileInfoRow(Icons.Default.Business, "Organisation", authViewModel.tenantSlug)
                    authViewModel.membershipNumber?.let { mem ->
                        ProfileInfoRow(Icons.Default.CreditCard, "Membership No.", mem)
                    }
                }
            }

            Spacer(Modifier.height(8.dp))

            // Sign out button
            Button(
                onClick = { showSignOutDialog = true },
                modifier = Modifier.fillMaxWidth().height(50.dp),
                colors = ButtonDefaults.buttonColors(containerColor = Color(0xFFef4444)),
                shape = RoundedCornerShape(12.dp)
            ) {
                Icon(Icons.Default.Logout, contentDescription = null, modifier = Modifier.size(18.dp))
                Spacer(Modifier.width(8.dp))
                Text("Sign Out", fontWeight = FontWeight.Bold, fontSize = 16.sp)
            }
        }
    }

    // Confirmation dialog
    if (showSignOutDialog) {
        AlertDialog(
            onDismissRequest = { showSignOutDialog = false },
            title   = { Text("Sign Out", color = TextPrimary) },
            text    = { Text("Are you sure you want to sign out?", color = TextMuted) },
            confirmButton = {
                TextButton(onClick = {
                    showSignOutDialog = false
                    authViewModel.signOut()
                }) {
                    Text("Sign Out", color = Color(0xFFef4444), fontWeight = FontWeight.Bold)
                }
            },
            dismissButton = {
                TextButton(onClick = { showSignOutDialog = false }) {
                    Text("Cancel", color = Purple400)
                }
            },
            containerColor = Surface700
        )
    }
}

@Composable
private fun ProfileInfoRow(
    icon: androidx.compose.ui.graphics.vector.ImageVector,
    label: String,
    value: String
) {
    Row(
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        Icon(icon, contentDescription = null, tint = Purple400, modifier = Modifier.size(20.dp))
        Column {
            Text(label, fontSize = 11.sp, color = TextFaint)
            Text(value, fontSize = 14.sp, color = TextPrimary, fontWeight = FontWeight.Medium)
        }
    }
}
