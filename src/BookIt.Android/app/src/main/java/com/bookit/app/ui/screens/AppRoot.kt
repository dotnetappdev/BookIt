package com.bookit.app.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.fragment.app.FragmentActivity
import com.bookit.app.ui.theme.Purple400
import com.bookit.app.ui.theme.Surface800
import com.bookit.app.ui.theme.Surface700
import com.bookit.app.ui.theme.TextPrimary
import com.bookit.app.viewmodels.AuthState
import com.bookit.app.viewmodels.AuthViewModel

/**
 * Root composable — routes between Login, BiometricLock, and the main tab bar
 * based on [AuthViewModel.authState].
 */
@Composable
fun AppRoot(
    activity: FragmentActivity,
    authViewModel: AuthViewModel
) {
    val authState by authViewModel.authState.collectAsState()

    when (authState) {
        AuthState.Loading -> {
            // Splash / loading screen
            Box(
                modifier = Modifier.fillMaxSize().background(Surface800),
                contentAlignment = Alignment.Center
            ) {
                CircularProgressIndicator(color = Purple400)
            }
        }
        AuthState.Unauthenticated -> {
            LoginScreen(authViewModel)
        }
        is AuthState.BiometricLock -> {
            BiometricLockScreen(activity = activity, authViewModel = authViewModel)
        }
        is AuthState.Authenticated -> {
            MainTabBar(authViewModel)
        }
    }
}

// ---------------------------------------------------------------------------
// 5-tab navigation
// ---------------------------------------------------------------------------

private data class TabItem(
    val label: String,
    val icon: ImageVector,
    val selectedIcon: ImageVector = icon
)

private val tabs = listOf(
    TabItem("Home",     Icons.Default.Home,    Icons.Default.Home),
    TabItem("Calendar", Icons.Default.CalendarMonth),
    TabItem("Bookings", Icons.Default.EventNote),
    TabItem("Wallet",   Icons.Default.QrCode),
    TabItem("Profile",  Icons.Default.Person)
)

@Composable
private fun MainTabBar(authViewModel: AuthViewModel) {
    var selectedTab by remember { mutableIntStateOf(0) }

    Scaffold(
        containerColor = Surface800,
        bottomBar = {
            NavigationBar(containerColor = Surface700) {
                tabs.forEachIndexed { index, tab ->
                    NavigationBarItem(
                        selected = selectedTab == index,
                        onClick  = { selectedTab = index },
                        label    = { Text(tab.label) },
                        icon     = {
                            Icon(
                                if (selectedTab == index) tab.selectedIcon else tab.icon,
                                contentDescription = tab.label
                            )
                        },
                        colors = NavigationBarItemDefaults.colors(
                            selectedIconColor   = Purple400,
                            selectedTextColor   = Purple400,
                            unselectedIconColor = TextPrimary.copy(alpha = 0.5f),
                            unselectedTextColor = TextPrimary.copy(alpha = 0.5f),
                            indicatorColor      = Purple400.copy(alpha = 0.15f)
                        )
                    )
                }
            }
        }
    ) { _ ->
        when (selectedTab) {
            0 -> DashboardScreen(authViewModel)
            1 -> CalendarScreen(authViewModel)
            2 -> BookingsScreen(authViewModel)
            3 -> WalletScreen(authViewModel)
            4 -> ProfileScreen(authViewModel)
        }
    }
}
