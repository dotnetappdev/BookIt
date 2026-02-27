package com.bookit.app.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.bookit.app.services.ApiException
import com.bookit.app.services.BookItApiService
import com.bookit.app.ui.theme.*
import com.bookit.app.viewmodels.AuthViewModel
import kotlinx.coroutines.launch

@Composable
fun LoginScreen(authViewModel: AuthViewModel) {
    var selectedTab by remember { mutableIntStateOf(0) }   // 0 = Sign In, 1 = Sign Up
    val scrollState = rememberScrollState()

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(Surface800)
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .verticalScroll(scrollState)
                .padding(24.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Spacer(Modifier.height(56.dp))

            // Logo
            Box(
                modifier = Modifier
                    .size(80.dp)
                    .background(
                        Brush.linearGradient(listOf(Purple700, Purple400)),
                        RoundedCornerShape(20.dp)
                    ),
                contentAlignment = Alignment.Center
            ) {
                Icon(Icons.Default.CalendarMonth, contentDescription = null,
                    tint = Color.White, modifier = Modifier.size(40.dp))
            }

            Spacer(Modifier.height(16.dp))
            Text("BookIt", fontSize = 32.sp, fontWeight = FontWeight.ExtraBold, color = TextPrimary)
            Text("Sign in to manage your bookings", fontSize = 14.sp, color = TextMuted)

            Spacer(Modifier.height(28.dp))

            // Tab row
            TabRow(
                selectedTabIndex = selectedTab,
                containerColor = Surface700,
                contentColor = Purple400,
                modifier = Modifier.fillMaxWidth()
            ) {
                Tab(selected = selectedTab == 0, onClick = { selectedTab = 0 },
                    text = { Text("Sign In") })
                Tab(selected = selectedTab == 1, onClick = { selectedTab = 1 },
                    text = { Text("Sign Up") })
            }

            Spacer(Modifier.height(20.dp))

            if (selectedTab == 0) SignInForm(authViewModel)
            else SignUpForm(authViewModel)

            Spacer(Modifier.height(40.dp))
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Sign In form
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun SignInForm(authViewModel: AuthViewModel) {
    val scope = rememberCoroutineScope()
    var email    by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var showPassword by remember { mutableStateOf(false) }
    var isLoading by remember { mutableStateOf(false) }
    var error: String? by remember { mutableStateOf(null) }

    FormCard {
        BookItTextField(
            value = email,
            onValueChange = { email = it },
            label = "Email",
            icon = Icons.Default.Email,
            keyboardType = KeyboardType.Email
        )

        BookItTextField(
            value = password,
            onValueChange = { password = it },
            label = "Password",
            icon = Icons.Default.Lock,
            isPassword = !showPassword,
            trailingIcon = {
                IconButton(onClick = { showPassword = !showPassword }) {
                    Icon(
                        if (showPassword) Icons.Default.VisibilityOff else Icons.Default.Visibility,
                        contentDescription = null, tint = TextMuted
                    )
                }
            }
        )

        error?.let { ErrorBanner(it) }

        BookItButton(label = "Sign In", isLoading = isLoading) {
            scope.launch {
                isLoading = true; error = null
                try {
                    val auth = BookItApiService.login(email.trim(), password)
                    authViewModel.signIn(auth)
                } catch (e: ApiException) {
                    error = e.message
                } catch (_: Exception) {
                    error = "Unable to connect. Please check your internet connection."
                } finally { isLoading = false }
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Sign Up form
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun SignUpForm(authViewModel: AuthViewModel) {
    val scope = rememberCoroutineScope()
    var firstName  by remember { mutableStateOf("") }
    var lastName   by remember { mutableStateOf("") }
    var email      by remember { mutableStateOf("") }
    var password   by remember { mutableStateOf("") }
    var membership by remember { mutableStateOf("") }
    var isLoading  by remember { mutableStateOf(false) }
    var error: String? by remember { mutableStateOf(null) }

    FormCard {
        Row(horizontalArrangement = Arrangement.spacedBy(12.dp)) {
            BookItTextField(
                value = firstName, onValueChange = { firstName = it },
                label = "First name", icon = Icons.Default.Person,
                modifier = Modifier.weight(1f)
            )
            BookItTextField(
                value = lastName, onValueChange = { lastName = it },
                label = "Last name", icon = Icons.Default.Person,
                modifier = Modifier.weight(1f)
            )
        }
        BookItTextField(
            value = email, onValueChange = { email = it },
            label = "Email", icon = Icons.Default.Email,
            keyboardType = KeyboardType.Email
        )
        BookItTextField(
            value = password, onValueChange = { password = it },
            label = "Password", icon = Icons.Default.Lock, isPassword = true
        )
        BookItTextField(
            value = membership, onValueChange = { membership = it },
            label = "Membership number (optional)", icon = Icons.Default.CreditCard
        )

        error?.let { ErrorBanner(it) }

        BookItButton(label = "Create Account", isLoading = isLoading) {
            if (firstName.isBlank() || lastName.isBlank()) {
                error = "First and last name are required."; return@BookItButton
            }
            scope.launch {
                isLoading = true; error = null
                try {
                    val auth = BookItApiService.register(
                        email.trim(), password, firstName, lastName,
                        membership.takeIf { it.isNotBlank() }
                    )
                    authViewModel.signIn(auth)
                } catch (e: ApiException) {
                    error = e.message
                } catch (_: Exception) {
                    error = "Unable to connect. Please check your internet connection."
                } finally { isLoading = false }
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Reusable form components
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun FormCard(content: @Composable ColumnScope.() -> Unit) {
    Card(
        modifier = Modifier.fillMaxWidth(),
        colors = CardDefaults.cardColors(containerColor = Surface700),
        shape = RoundedCornerShape(16.dp)
    ) {
        Column(
            modifier = Modifier.padding(20.dp),
            verticalArrangement = Arrangement.spacedBy(14.dp),
            content = content
        )
    }
}

@Composable
fun BookItTextField(
    value: String,
    onValueChange: (String) -> Unit,
    label: String,
    icon: ImageVector,
    modifier: Modifier = Modifier,
    keyboardType: KeyboardType = KeyboardType.Text,
    isPassword: Boolean = false,
    trailingIcon: @Composable (() -> Unit)? = null
) {
    OutlinedTextField(
        value = value,
        onValueChange = onValueChange,
        label = { Text(label, color = TextMuted) },
        leadingIcon = { Icon(icon, contentDescription = null, tint = TextMuted) },
        trailingIcon = trailingIcon,
        modifier = modifier.fillMaxWidth(),
        keyboardOptions = KeyboardOptions(keyboardType = keyboardType),
        visualTransformation = if (isPassword) PasswordVisualTransformation() else VisualTransformation.None,
        singleLine = true,
        colors = OutlinedTextFieldDefaults.colors(
            focusedTextColor   = TextPrimary,
            unfocusedTextColor = TextPrimary,
            focusedBorderColor = Purple400,
            unfocusedBorderColor = Surface500,
            cursorColor        = Purple400
        )
    )
}

@Composable
fun BookItButton(
    label: String,
    isLoading: Boolean,
    onClick: () -> Unit
) {
    Button(
        onClick = onClick,
        enabled = !isLoading,
        modifier = Modifier.fillMaxWidth().height(50.dp),
        colors = ButtonDefaults.buttonColors(containerColor = Purple700),
        shape = RoundedCornerShape(12.dp)
    ) {
        if (isLoading) {
            CircularProgressIndicator(modifier = Modifier.size(20.dp), color = Color.White, strokeWidth = 2.dp)
            Spacer(Modifier.width(8.dp))
        }
        Text(label, fontWeight = FontWeight.Bold, fontSize = 16.sp)
    }
}

@Composable
fun ErrorBanner(message: String) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .background(Color.Red.copy(alpha = 0.12f), RoundedCornerShape(8.dp))
            .padding(12.dp),
        horizontalArrangement = Arrangement.spacedBy(8.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(Icons.Default.Warning, contentDescription = null, tint = Color(0xFFff6b6b))
        Text(message, color = Color(0xFFff6b6b), fontSize = 13.sp)
    }
}
