package com.bookit.app

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.lifecycle.viewmodel.compose.viewModel
import com.bookit.app.ui.theme.BookItTheme
import com.bookit.app.ui.screens.AppRoot
import com.bookit.app.viewmodels.AuthViewModel

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            BookItTheme {
                val authViewModel: AuthViewModel = viewModel()
                AppRoot(
                    activity = this,
                    authViewModel = authViewModel
                )
            }
        }
    }
}
