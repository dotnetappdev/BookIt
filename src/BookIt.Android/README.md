# BookIt Android

Native Android application for BookIt, built with **Kotlin + Jetpack Compose** targeting Android 15 (API 35), min API 26 (Android 8.0).  Mirrors all features of the BookIt iOS and MAUI apps.

## Requirements

| Tool | Version |
|------|---------|
| Android Studio | Hedgehog 2023.1.1+ |
| Kotlin | 2.1.0 |
| AGP | 8.7.3 |
| Compile SDK | 35 (Android 15) |
| Min SDK | 26 (Android 8.0) |

## Opening the project

1. Open **Android Studio**
2. **File → Open** → select `src/BookIt.Android/`
3. Let Gradle sync complete
4. Run on a device or emulator (API 26+)

## Project structure

```
app/src/main/java/com/bookit/app/
├── BookItApplication.kt          Application class
├── MainActivity.kt               Single Activity host
├── models/
│   └── AppModels.kt              Kotlin data classes matching C# DTOs
├── services/
│   ├── BookItApiService.kt       HTTP client (coroutines + HttpURLConnection)
│   ├── SecureStorageService.kt   EncryptedSharedPreferences (Keystore-backed)
│   └── BiometricService.kt       BiometricPrompt wrapper
├── viewmodels/
│   └── AuthViewModel.kt          Auth state + biometric lock logic
└── ui/
    ├── theme/                    Material 3 dark theme (Color, Theme, Type)
    ├── components/
    │   ├── QRCodeView.kt         ZXing QR bitmap generator
    │   └── AppointmentCard.kt    Shared cards and status badge
    └── screens/
        ├── AppRoot.kt            Navigation root (Login / Lock / Tabs)
        ├── LoginScreen.kt        Sign In / Sign Up tabs
        ├── BiometricLockScreen.kt Fingerprint / Face / PIN lock
        ├── DashboardScreen.kt    Stat cards + today's schedule
        ├── CalendarScreen.kt     Month grid with booking dots
        ├── BookingsScreen.kt     Chronological list + QR modal
        ├── WalletScreen.kt       QR pass card + Calendar + Share
        └── ProfileScreen.kt      User info + sign-out
```

## Features

### 5-tab navigation

| Tab | Key behaviour |
|-----|---------------|
| **Home** | Stat cards (Today / Upcoming / Confirmed / Pending) + today's schedule; pull-to-refresh |
| **Calendar** | Month grid with booking dot indicators; tap a day to see appointments |
| **Bookings** | Chronological list with status badges; per-item QR modal |
| **Wallet** | Branded QR pass card; **Add to Calendar** (CalendarContract) + **Share Pass** (FileProvider + Intent.ACTION_SEND) |
| **Profile** | Avatar initials, role badge, user info, sign-out confirmation |

### Biometric / PIN lock

On every cold launch where a saved session exists in `EncryptedSharedPreferences`, the app presents `BiometricLockScreen`:

- **"Unlock with Biometrics"** — uses `BiometricPrompt` with `BIOMETRIC_STRONG | DEVICE_CREDENTIAL`; the system automatically offers "Use PIN" as a fallback inside the prompt sheet
- **"Use PIN / Password"** — calls `DEVICE_CREDENTIAL` directly
- **"Sign out"** — clears the session and returns to login

### Security

| Mechanism | Implementation |
|-----------|----------------|
| Token storage | `EncryptedSharedPreferences` (AES-256-GCM, Android Keystore) |
| Biometric auth | `androidx.biometric:biometric:1.1.0` — `BiometricPrompt` with `BIOMETRIC_STRONG` |
| QR generation | ZXing Core 3.5.3 — no UI dependency, runs on background thread |
| Network | HTTPS only; `network_security_config.xml` disables cleartext for `api.bookit.app` |

### QR format

Matches iOS and MAUI exactly:
```
BOOKIT:{id}:{pin}:{startYYYYMMDDHHmm}:{membershipNumber|NONE}
```
