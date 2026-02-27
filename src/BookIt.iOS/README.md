# BookIt iOS — SwiftUI App

A dedicated native iOS application for **BookIt**, built with SwiftUI. Provides the same features as the `.NET MAUI` Blazor Hybrid app but uses native Swift UI components.

---

## Requirements

| Tool | Version |
|------|---------|
| Xcode | 15.3 or later |
| iOS Deployment Target | 16.0+ |
| Swift | 5.9+ |

---

## Getting Started

1. **Open the project in Xcode**
   ```bash
   open src/BookIt.iOS/BookIt.xcodeproj
   ```

2. **Set your development team** — Select the `BookIt` target → Signing & Capabilities → Team.

3. **Run** on a simulator or physical device (⌘R).

---

## Features

All five tabs mirror the MAUI app:

| Tab | Screen | Description |
|-----|--------|-------------|
| 🏠 | Dashboard | Stat cards (today / upcoming / confirmed / pending) + today's schedule |
| 📅 | Calendar | Month calendar with booking dots; tap a day to see its appointments |
| 📋 | Bookings | Chronological list of upcoming appointments with per-item QR button |
| 🔲 | Wallet | Branded QR pass card for the next appointment — **Add to Calendar** (EventKit) and **Share Pass** (native share sheet) |
| 👤 | Profile | User info, role, membership number, sign-out |

### Authentication
- **Sign In** with email + password
- **Sign Up** with first/last name, email, password, optional membership number
- Session persisted in the iOS **Keychain** (stays logged in between launches)

### QR Wallet
- Pass card shows: business name, service, date/time, staff, membership number
- QR content format: `BOOKIT:{id}:{pin}:{YYYYMMDDHHmm}:{membershipNumber|NONE}`
- **Add to Calendar** — saves an `EKEvent` to the user's default iOS calendar (requires calendar permission)
- **Share Pass** — generates a QR image via CoreImage and opens the native share sheet

---

## Project Structure

```
BookIt.iOS/
├── BookIt.xcodeproj/
│   └── project.pbxproj          # Xcode project
└── BookIt/
    ├── BookItApp.swift           # @main entry point
    ├── ContentView.swift         # Tab bar / login gate
    ├── Info.plist
    ├── Assets.xcassets/
    ├── Models/
    │   └── AppModels.swift       # AuthResponse, AppointmentResponse, etc.
    ├── Services/
    │   ├── BookItAPIService.swift # async/await HTTP client (actor)
    │   ├── AuthStore.swift        # @MainActor observable auth state
    │   └── KeychainService.swift  # Secure token persistence
    ├── Views/
    │   ├── LoginView.swift
    │   ├── DashboardView.swift
    │   ├── CalendarView.swift
    │   ├── BookingsView.swift
    │   ├── WalletView.swift
    │   └── ProfileView.swift
    └── Components/
        ├── QRCodeView.swift           # CoreImage QR generator (no external deps)
        ├── AppointmentQRCardView.swift # Wallet-style branded card
        └── AppointmentRowView.swift   # Compact appointment row
```

---

## API

The app connects to `https://api.bookit.app` (same backend as the MAUI and Blazor apps).

To point at a local development server, edit `baseURL` in `Services/BookItAPIService.swift`:

```swift
private let baseURL = URL(string: "http://localhost:5000")!
```

---

## No External Dependencies

All functionality uses only Apple frameworks:

- **SwiftUI** — UI
- **Foundation / URLSession** — networking
- **CoreImage** — QR code generation
- **EventKit** — calendar integration
- **Security** — Keychain storage
- **UIKit** — `UIActivityViewController` for share sheet
