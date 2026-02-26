# BookIt

> **The all-in-one booking CRM for service businesses** — barbers, salons, spas, gyms and recruitment agencies. Accept appointments 24/7, manage your team, and get paid online.

---

## Screenshots

### Public Front End (Blazor + MudBlazor)

**Home page** — hero, stats bar, features grid, "How it works", CTA

![BookIt Home Page](https://github.com/user-attachments/assets/5ecaa36f-551a-49a8-9c00-0218bdbc006e)

**Pricing page** — 4-tier plans (Free / Starter / Pro / Enterprise), monthly/annual toggle

![BookIt Pricing Page](https://github.com/user-attachments/assets/b5c3f532-d7eb-4bf6-8010-9c5dfe501047)

**Login page** — glassmorphism card on dark gradient, show/hide password

![BookIt Login](https://github.com/user-attachments/assets/e8e8dda8-879e-4bd2-8c82-fdbf8177f1a0)

### Admin Back End (Blazor + MudBlazor)

**Admin Dashboard** — dark sidebar, stat cards, today's schedule, quick actions

![BookIt Admin Dashboard](https://github.com/user-attachments/assets/88cc84d7-b714-4af7-add2-4d553073a2db)

---

## Projects

| Project | Type | Description |
|---|---|---|
| `BookIt.API` | ASP.NET Core Web API | REST API — auth, appointments, tenants, payments |
| `BookIt.Core` | Class Library | Entities, DTOs, enums, interfaces, feature flags |
| `BookIt.Infrastructure` | Class Library | EF Core, repositories, background services |
| `BookIt.Web` | ASP.NET Core MVC | Original Razor/MVC front end with dark-mode SCSS theme |
| `BookIt.Blazor` | Blazor Server | Second front end — MudBlazor 9, dark/light mode |
| `BookIt.UI.Shared` | Razor Class Library | Shared MudBlazor components for Blazor + MAUI |
| `BookIt.Payments.Stripe` | Class Library | `IStripeProvider` — Stripe Payment Intents v2 |
| `BookIt.Payments.PayPal` | Class Library | `IPayPalProvider` — PayPal Orders v2 |
| `BookIt.Payments.ApplePay` | Class Library | `IApplePayProvider` — Apple Pay via Stripe (isolated reusable DLL) |
| `BookIt.Subscriptions.RevenueCat` | Class Library | `IRevenueCatProvider` — RevenueCat subscription management (isolated reusable DLL) |
| `BookIt.Tests` | xUnit | Unit tests (24 passing) |

---

## Features

### Booking & Calendar
- Public booking page per tenant (e.g. `/demo-barber/book`)
- Full month/week calendar with availability management
- Multi-staff support with individual schedules
- Automatic confirmation emails

### Payments
- Stripe (Payment Intents v2) via `BookIt.Payments.Stripe`
- PayPal (Orders v2) via `BookIt.Payments.PayPal`
- **Apple Pay** via Stripe, isolated in `BookIt.Payments.ApplePay` — reusable class library
- Require full payment or deposit at booking time
- Payment status tracking (Unpaid / Paid / Partial / Refunded)

### Subscriptions & RevenueCat
- Four subscription tiers: **Free** · **Starter £19/mo** · **Pro £49/mo** · **Enterprise £129/mo**
- Monthly / annual billing toggle (20% saving)
- **RevenueCat** subscription management via `BookIt.Subscriptions.RevenueCat` — reusable class library
  - Entitlement-based plan resolution (maps RevenueCat products to `SubscriptionPlan`)
  - Super-admin-only configuration panel: set RevenueCat API key, entitlement ID, and per-tier prices/product IDs
- Feature flags map plan to capability:
  ```csharp
  FeatureFlags.CanUseOnlinePayments(SubscriptionPlan.Free);   // false
  FeatureFlags.CanUseAiAssistant(SubscriptionPlan.Pro);       // true
  FeatureFlags.MaxStaff(SubscriptionPlan.Starter);            // 5
  ```

### Admin Portal
- Dark sidebar with grouped navigation (Main / Management / Account)
- Stat cards with 10 px padding and colour-coded accent bars
- Today's schedule with status dots and coloured chips
- Quick-action panel
- Profile dropdown (Dashboard, Settings, Subscription, Sign Out)
- Dark / light mode toggle

### Super Admin Console (`/super-admin`)
- Tenant management (list, search, delete, copy Tenant ID)
- **RevenueCat Configuration tab** — visible only to `SuperAdmin` role:
  - Platform API key and entitlement identifier
  - Per-tier pricing (monthly & annual) and RevenueCat product IDs for all four plans

### Blazor Front End
| Page | Route |
|---|---|
| Home | `/` |
| Login | `/login` |
| Pricing | `/pricing` |
| Admin Dashboard | `/{slug}/admin` |
| Admin Calendar | `/{slug}/admin/calendar` |
| Admin Services | `/{slug}/admin/services` |
| Admin Forms | `/{slug}/admin/forms` |
| Admin Interviews | `/{slug}/admin/interviews` |
| Admin Settings | `/{slug}/admin/settings` |
| Admin Subscriptions | `/{slug}/admin/subscriptions` |

### MAUI-Ready Shared Library (`BookIt.UI.Shared`)
All UI logic is in a Razor Class Library so the same components work in Blazor Server, WASM and **.NET MAUI**:

```csharp
// In your MAUI app's MauiProgram.cs
builder.Services.AddBookItUI("https://api.bookit.app");
```

Components available:
- `AdminLayout` — full dark sidebar + topbar + profile dropdown
- `DashboardView`, `ServicesView`, `SettingsView`, `SubscriptionsView`
- `LoginView`, `PricingView`
- `BookItApiService` — stateless HTTP client (no `IHttpContextAccessor`)
- `BookItAuthState` — scoped auth state with initials helper
- `BookItTheme` — custom MudBlazor theme (brand colours, light + dark palettes)

---

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server / LocalDB
- Stripe API keys (optional)
- PayPal client credentials (optional)

### Run the API
```bash
cd src/BookIt.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=BookIt;Trusted_Connection=True;"
dotnet user-secrets set "Jwt:Key" "your-256-bit-secret"
dotnet run
```

### Run the Blazor Front End
```bash
cd src/BookIt.Blazor
dotnet run
# → https://localhost:5001
```

### Run the MVC Front End
```bash
cd src/BookIt.Web
dotnet run
# → https://localhost:5002
```

### Run Tests
```bash
dotnet test
```

---

## Dark Mode

Both front ends support dark and light mode:

- **Blazor** — `MudThemeProvider` with `IsDarkMode` toggle in both `MainLayout` and `AdminLayout`; preference persists to `localStorage` via JS interop
- **MVC** — `[data-theme="dark"]` CSS variable system; moon/sun toggle in public navbar and admin topbar; preference persisted to `localStorage` with no flash on load

---

## Payment Providers

### Stripe (`BookIt.Payments.Stripe`)
```csharp
// Register
builder.Services.AddStripePayments();

// Use
var result = await _stripeProvider.CreatePaymentIntentAsync(secretKey, amount, currency, metadata);
// result.PaymentIntentId, result.ClientSecret
```

### Apple Pay via Stripe (`BookIt.Payments.ApplePay`)
Apple Pay on the web is processed through Stripe — this library creates a PaymentIntent that
the Stripe.js Payment Request Button presents as an Apple Pay sheet on supported devices.

```csharp
// Register (requires AddStripePayments() to be registered first)
builder.Services.AddStripePayments();
builder.Services.AddApplePayPayments();

// Use
var result = await _applePayProvider.CreateApplePayIntentAsync(stripeSecretKey, amount, currency);
// result.PaymentIntentId, result.ClientSecret  →  pass ClientSecret to Stripe.js
```

### PayPal (`BookIt.Payments.PayPal`)
```csharp
// Register
builder.Services.AddPayPalPayments();

// Use
var orderId = await _paypalProvider.CreateOrderAsync(clientId, clientSecret, amount, currency, ref, desc);
```

### RevenueCat (`BookIt.Subscriptions.RevenueCat`)
```csharp
// Register
builder.Services.AddRevenueCat();

// Use — resolve the current plan from RevenueCat entitlements
var plan = await _revenueCatProvider.GetEntitlementPlanAsync(apiKey, appUserId);

// Get all offerings (used to display pricing tiers with RevenueCat product IDs)
var tiers = await _revenueCatProvider.GetOfferingsAsync(apiKey);
```

> **Super-admin only**: The RevenueCat API key, entitlement identifier, and per-tier prices can be
> configured in the **Super Admin Console** at `/super-admin` → **RevenueCat Config** tab.
> This section is only rendered for users with `UserRole.SuperAdmin`.

---

## Database Migrations

See [docs/EF-Migrations.md](docs/EF-Migrations.md) for full migration instructions.

```bash
cd src/BookIt.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../BookIt.API
dotnet ef database update --startup-project ../BookIt.API
```

---

## Architecture

```
BookIt.API          ← HTTP entry point, controllers, middleware
    ↓
BookIt.Infrastructure  ← EF Core, repos, services (delegates payments to providers)
    ↓
BookIt.Core         ← Entities, DTOs, interfaces, enums, FeatureFlags
    ↓
BookIt.Payments.Stripe        ← IStripeProvider (isolated class library)
BookIt.Payments.PayPal        ← IPayPalProvider (isolated class library)
BookIt.Payments.ApplePay      ← IApplePayProvider (isolated — delegates to Stripe)
BookIt.Subscriptions.RevenueCat ← IRevenueCatProvider (isolated class library)

BookIt.Blazor       ← Blazor Server front end (consumes BookIt.UI.Shared)
BookIt.Web          ← ASP.NET Core MVC front end
BookIt.UI.Shared    ← Razor Class Library (Blazor + MAUI compatible)
```

---

© 2026 BookIt
