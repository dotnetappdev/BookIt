# BookIt

> **The all-in-one booking CRM for service businesses** — barbers, salons, spas, gyms and recruitment agencies. Accept appointments 24/7, manage your team, and get paid online.

---

## Screenshots

### Public Front End (Blazor + MudBlazor)

**Home page** — hero, stats bar, features grid, "How it works", CTA

![BookIt Home Page](https://github.com/user-attachments/assets/5ecaa36f-551a-49a8-9c00-0218bdbc006e)

**Pricing page** — 4-tier plans (Free / Starter / Pro / Enterprise), monthly/annual toggle with Apple Pay badge on Starter+

![BookIt Pricing Page](https://github.com/user-attachments/assets/b5c3f532-d7eb-4bf6-8010-9c5dfe501047)

**Login page** — glassmorphism card on dark gradient, show/hide password

![BookIt Login](https://github.com/user-attachments/assets/e8e8dda8-879e-4bd2-8c82-fdbf8177f1a0)

### Admin Back End (Blazor + MudBlazor)

**Admin Dashboard** — dark sidebar, stat cards, today's schedule, quick actions

![BookIt Admin Dashboard](https://github.com/user-attachments/assets/88cc84d7-b714-4af7-add2-4d553073a2db)

### Admin Panel — MVC Back End (new screens)

**Admin sidebar** now includes: Services · **Staff** · **Classes** · **Customers** · Forms · Interviews — all available for every business type.

#### Staff Management

Add, edit, delete staff members and assign them to services. Works for any profession (barber, gym instructor, physio, consultant, etc.).

![Admin Staff Page](https://github.com/user-attachments/assets/4bcb0bd6-c0b4-4949-ace5-f29098400170)

**Add / Edit Staff modal** — two-column grid with icons on all contact fields, scrollable on mobile:

```
┌──────────────────────────────────────────────────────┐
│ ✦  Add Staff Member                              [×] │
├──────────────────────────────────────────────────────┤
│ First Name *       │ Last Name *                     │
│ [Jane            ] │ [Smith           ]               │
│                                                      │
│ Email                  │ Phone                       │
│ [✉ jane@salon.com   ]  │ [✆ 07700 900 123 ]          │
│                                                      │
│ Photo URL                                            │
│ [🖼 https://example.com/photo.jpg               ]    │
│                                                      │
│ Bio                                                  │
│ [Senior stylist with 10+ years experience…     ]    │
│                                                      │
│ Sort Order  │ ☑ Active                               │
│ [ 0       ] │                                        │
│                                          [Cancel] [Save Staff Member] │
└──────────────────────────────────────────────────────┘
```

**Assign Services modal** — tick the services each staff member can perform:

```
┌────────────────────────────────────┐
│ ☰  Assign Services             [×] │
├────────────────────────────────────┤
│ Select services that Jane Smith    │
│ can perform:                       │
│  ☑ Haircut         (30 min · £25) │
│  ☑ Beard Trim      (15 min · £15) │
│  ☐ Colour          (90 min · £65) │
│  ☑ Blow Dry        (30 min · £20) │
│              [Cancel] [Save Assignments] │
└────────────────────────────────────┘
```

#### Classes & Group Sessions

Schedule group sessions with date, time, capacity, price, and **multiple instructors**. Any number of staff can be assigned as instructors per session.

![Admin Classes Page](https://github.com/user-attachments/assets/e36dc566-8372-4b17-923d-842b6e111a4e)

#### Customers CRM

Full CRUD for customer profiles. Profiles are created automatically on every booking and can be managed manually by admins.

![Admin Customers Page](https://github.com/user-attachments/assets/d760ca5c-f5fe-4333-828f-cb3809d1e3fe)

**Add / Edit Customer modal** — fields organised into labelled sections, contact fields with icons, scrollable:

```
┌──────────────────────────────────────────────────────┐
│ ✦  Add Customer                                  [×] │
├──────────────────────────────────────────────────────┤
│ ── PERSONAL DETAILS ─────────────────────────────── │
│ First Name *        │ Last Name                      │
│ [Jane             ] │ [Doe              ]             │
│ Gender              │ Membership Number               │
│ [— Not specified —] │ [MBR-001          ]             │
│                                                      │
│ ── CONTACT INFORMATION ──────────────────────────── │
│ Email *                        │ Phone               │
│ [✉ jane@example.com         ]  │ [✆ 07700 900 123 ]  │
│ Mobile                                               │
│ [📱 07700 900 456          ]                         │
│                                                      │
│ ── ADDRESS ──────────────────────────────────────── │
│ Address                                              │
│ [123 High Street                                 ]   │
│ City           │ Post Code  │ Country                │
│ [London      ] │ [SW1A 1AA] │ [UK          ]         │
│                                                      │
│ ── NOTES & TAGS ─────────────────────────────────── │
│ Tags                                                 │
│ [VIP, New Client                              ]      │
│ ☑ Marketing Opt-in    ☑ SMS Opt-in                  │
│ Notes                                                │
│ [Prefers afternoon appointments…              ]      │
│                                       [Cancel] [Save Customer] │
└──────────────────────────────────────────────────────┘
```

#### Services Management

Manage service offerings with price, duration, buffer time and booking link. The **Edit** modal is fully wired — pre-populates all fields including buffer time, and includes an inline **Delete** action.

```
┌──────────────────────────────────────────────────────┐
│ Services                            [+ Add Service]  │
│ Manage your service offerings                        │
├──────────┬──────────┬──────────┬────────────────────┤
│ ┌──────┐ │ ┌──────┐ │ ┌──────┐ │                   │
│ │ ✂    │ │ │ 🧴   │ │ │ ✂    │ │                   │
│ │      │ │ │      │ │ │      │ │                   │
│ │Haircut│ │ │Colour│ │ │Beard │ │                   │
│ │[cuts] │ │ │      │ │ │Trim  │ │                   │
│ │ £25  │ │ │ £65  │ │ │ £15  │ │                   │
│ │30 min│ │ │90 min│ │ │15 min│ │                   │
│ │[Online]│ │ │[Online]│ │ │[Online]│               │
│ ├──────┤ │ ├──────┤ │ ├──────┤ │                   │
│ │ ✏ Edit│ │ │ ✏ Edit│ │ │ ✏ Edit│ │                │
│ └──────┘ │ └──────┘ │ └──────┘ │                   │
└──────────┴──────────┴──────────┴────────────────────┘
```

**Add / Edit Service modal** — consistent `admin-form-grid` layout with icons, slug auto-generation, buffer time and delete:

```
┌──────────────────────────────────────────────────────┐
│ ✏  Edit Service                                  [×] │
├──────────────────────────────────────────────────────┤
│ Service Name *                  │ Price (£) *        │
│ [Haircut                      ] │ [25.00    ]        │
│                                                      │
│ URL Slug (leave blank to keep current)               │
│ /demo-barber/book/ [haircut                       ]  │
│                                                      │
│ Duration (minutes) *    │ Buffer Time (minutes)      │
│ [30 minutes          ▼] │ [5                   ]     │
│                          (cleanup time between appts)│
│                                                      │
│ Description                                          │
│ [Classic scissor & clipper cut, finished with…  ]   │
│                                                      │
│ Image URL                                            │
│ [🖼 https://example.com/haircut.jpg             ]    │
│                                                      │
│ ☑ Allow Online Booking                               │
│                                                      │
│ [🗑 Delete]               [Cancel] [Save Changes]   │
└──────────────────────────────────────────────────────┘
```

#### Booking Forms (admin + mobile)

Build and manage booking forms. The form builder is fully mobile-responsive (collapses to single column on screens < 992 px).

![Admin Booking Forms Page](https://github.com/user-attachments/assets/ed361fb4-2b42-4b41-9f94-78355ee4e361)

### Admin Panel — New Screens (Blazor Dark Mode)

#### Customers — Data Grid with Membership Number

```
┌─────────────────────────────────────────────────────────────┐
│ Customers                               [+ Add Customer]    │
│ All customers for this business                             │
├─────────────────────────────────────────────────────────────┤
│ 🔍 [Search by name, email or phone…]                       │
├────────┬──────────────┬────────────┬───────┬───────┬───────┤
│Customer│    Phone     │Membership  │ City  │Bookings│Spent  │
├────────┼──────────────┼────────────┼───────┼────────┼───────┤
│ JD     │07700 900 123 │MBR-001     │London │  [5]   │£320   │
│Jane Doe│jane@ex.com   │            │       │        │       │
├────────┼──────────────┼────────────┼───────┼────────┼───────┤
│ JS     │07700 900 456 │GYM-2024    │Bristol│  [2]   │£85    │
└────────┴──────────────┴────────────┴───────┴────────┴───────┘
```

Add/Edit dialog includes a **Membership Number** field for gym / club / loyalty schemes.

#### Booking Forms — Full CRUD Data Grid

```
┌─────────────────────────────────────────────────────────────┐
│ Booking Forms                           [+ New Form]        │
├──────────────┬────────┬─────────┬───────────────────────── │
│ Form Name    │ Fields │ Default │ Actions                   │
├──────────────┼────────┼─────────┼───────────────────────── │
│ Default Form │  [8]   │[Default]│ ⚙ Settings  🔨 Builder 🗑 │
│ Intake Form  │  [5]   │         │ ⚙ Settings  🔨 Builder 🗑 │
│ Consultation │  [12]  │         │ ⚙ Settings  🔨 Builder 🗑 │
└──────────────┴────────┴─────────┴───────────────────────── │
```

**⚙ Settings dialog** for inline rename/metadata editing without navigating to the builder:
```
┌──────────────────────────────────────────┐
│  Edit Form Settings                      │
├──────────────────────────────────────────┤
│  Form Name      [Default Booking Form   ]│
│  Description    [Standard intake…      ]│
│  Welcome Msg    [Welcome! Please fill…  ]│
│  Confirm Msg    [Thank you! We'll see…  ]│
│  ● Collect phone      ● Collect notes   │
│  ● Set as default form                  │
│             [Cancel]  [Save Changes]    │
└──────────────────────────────────────────┘
```

#### Booking Form (Dark Mode)

```
┌─────────────────────────────────────────────────────────────┐
│  ◀ Back                     Book Appointment           🌙   │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────┐   │
│  │  💈  My Salon · 123 High Street, London             │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  Services          ✓ Haircut £25    Beard Trim £15          │
│                                                             │
│  Select a date                                              │
│  ┌─────────────────────────────────────────────────────┐   │
│  │    March 2026                          ‹  ›          │   │
│  │  Mo  Tu  We  Th  Fr  Sa  Su                          │   │
│  │                         [4]  5   6   7   8           │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  Available times:  [9:00] [10:00] [11:00] [14:00] [15:00]  │
│                                                             │
│  Your details                                               │
│  Full name [Jane Smith    ]  Email [jane@example.com    ]   │
│  Phone     [07700 900 123 ]  Notes [Any special requests]   │
│                                                             │
│         [ Book Appointment →                  £25.00 ]      │
└─────────────────────────────────────────────────────────────┘
```

Toggle the moon/sun icon (top-right) to switch light/dark — saved to `localStorage`.

#### Interviews — Add Slot with Video Conference

```
┌──────────────────────────────────────────────┐
│  Add Interview Slot                          │
├──────────────────────────────────────────────┤
│  Date [04/03/2026]    Time [10:00 am]        │
│  Duration [60 min]                           │
│  Interviewer Name [Jane Smith              ] │
│  Location [Head Office, Room 3A            ] │
│                                              │
│  ──── 📹 Video Conference ─────────────      │
│  Provider [Microsoft Teams ▼]               │
│  (Teams / Zoom / Google Meet / Webex / ...)  │
│  Meeting URL   [https://teams.microsoft.com] │
│  Meeting ID    [123 456 7890               ] │
│  Password/PIN  [abc123                     ] │
│  Host URL      [https://teams.…/host       ] │
│  Dial-In       [+44 20 1234 5678           ] │
│                                              │
│                  [Cancel]  [Create Slot]     │
└──────────────────────────────────────────────┘
```

---

### MAUI Mobile App Screens

#### Login / Sign Up (Light + Dark)

```
┌─────────────────────────────────────────┐
│           📅  BookIt                    │
│   Sign in to manage your bookings       │
│                                         │
│   [ Sign In ]  [ Sign Up ]              │
│                                         │
│  Email    [jane@example.com          ]  │
│  Password [•••••••••••••            ] 👁 │
│  Org code [my-salon                  ]  │
│                                         │
│  ┌─────────────────────────────────┐   │
│  │          Sign In →              │   │
│  └─────────────────────────────────┘   │
└─────────────────────────────────────────┘
```

Sign Up tab adds: First name · Last name · **Membership number (optional)**.

#### Dashboard Tab

```
┌─────────────────────────────────────────┐
│  Dashboard                        [JD]  │
│  ┌────────┐  ┌─────────┐  ┌─────────┐  │
│  │Today  3│  │Week  14 │  │Rev £420 │  │
│  └────────┘  └─────────┘  └─────────┘  │
│  Today's Schedule                       │
│  🟣 10:00  Jane Doe — Haircut           │
│  🟡 11:30  Bob S. — Beard Trim          │
│  🟢 14:00  Alice J. — Colour            │
├────────┬────────┬────────┬──────┬──────┤
│Dashboard│Calendar│Bookings│Wallet│Profile│
└────────┴────────┴────────┴──────┴──────┘
```

#### My Bookings Tab

```
┌─────────────────────────────────────────┐
│  My Bookings                            │
│  ┌───────────────────────────────────┐  │
│  │ ┌────┐  Haircut + Beard Trim     │  │
│  │ │ 4  │  10:00 am – 11:30 am     │  │
│  │ │MAR │  · Jane Smith             │  │
│  │ └────┘  [Confirmed ✓]  [QR]     │  │
│  │                        £40.00    │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

#### QR Wallet Tab

```
┌─────────────────────────────────────────┐
│  Wallet · Your upcoming booking pass    │
│                                         │
│  ┌─────────────────────────────────┐   │
│  │ ┌────┐  My Salon               │   │  ← branded header
│  │ │ B  │  Booking Confirmation   │   │
│  │ └────┘                         │   │
│  │─────────────────────────────────│   │
│  │  Jane Doe                       │   │
│  │  Wed, 4 March 2026              │   │
│  │  10:00 am – 11:00 am            │   │
│  │  Haircut, Beard Trim            │   │
│  │                                 │   │
│  │  ┌─────────────────────────┐    │   │
│  │  │  ▓▓▓▓ ░░░ ▓▓▓▓ ░░░░    │    │   │
│  │  │  ░░░░ ▓▓▓ ░░░░ ▓▓▓▓    │    │   │  ← QR code
│  │  │  ▓▓▓▓ ░░░ ▓▓▓▓ ░░░░    │    │   │
│  │  └─────────────────────────┘    │   │
│  │                                 │   │
│  │  BOOKING PIN:    4 8 2 9        │   │
│  │  MEMBERSHIP NO:  MBR-001        │   │  ← membership number
│  │─────────────────────────────────│   │
│  │  Generated 4 Mar 2026 · BookIt  │   │
│  └─────────────────────────────────┘   │
│                                         │
│  ┌──────────────────┐ ┌─────────────┐  │
│  │ 📅 Add to Calendar│ │ ↗ Share Pass│  │  ← wallet actions
│  └──────────────────┘ └─────────────┘  │
│                                         │
│  2 more upcoming bookings               │
│  Thu 5 Mar · 2:00 pm  Haircut  [Confirmed] [📅] │
│  Mon 9 Mar · 10:00 am Colour   [Pending ]  [📅] │
├────────┬────────┬────────┬──────┬──────┤
│Dashboard│Calendar│Bookings│Wallet│Profile│
└────────┴────────┴────────┴──────┴──────┘
```

**QR code data**: `BOOKIT:{appointmentId}:{pin}:{startYYYYMMDDHHmm}:{membershipNumber|NONE}`

**📅 Add to Calendar** → generates ICS event → opens iOS Calendar / Google Calendar
**↗ Share Pass** → generates PNG QR → native share sheet (AirDrop / Messages / Save to Photos)

#### Profile Tab

```
┌─────────────────────────────────────────┐
│            ┌────┐                       │
│            │ JD │  Jane Doe             │
│            └────┘  jane@example.com     │
│                    [Customer]           │
│  ─────────────────────────────────      │
│  Organisation     my-salon              │
│  Role             Customer              │
│  ─────────────────────────────────      │
│  🔔 Notification preferences            │
│  🔒 Change password                     │
│  🗑 Delete account                       │
│                                         │
│  [ Sign Out ]                           │
└─────────────────────────────────────────┘
```

---

### Admin Settings — Notifications

The Settings page (`/{slug}/admin/settings`) now includes three new sections:

**SMS Notifications** — enable/disable, choose ClickSend or Twilio, enter credentials (API keys masked):

```
┌───────────────────────────────────────────────────┐
│ 💬  SMS Notifications                             │
├───────────────────────────────────────────────────┤
│  ● Enable SMS notifications                       │
│  Provider:  [ClickSend ▼]                         │
│  Username   [______________________]              │
│  API Key    [•••••••••••••••••••••] (masked)      │
│  From       [+447700900000]                       │
└───────────────────────────────────────────────────┘
```

**Email Notifications (SendGrid)** — enable/disable, SendGrid API key, from address:

```
┌───────────────────────────────────────────────────┐
│ ✉️  Email Notifications (SendGrid)                │
├───────────────────────────────────────────────────┤
│  ● Enable booking confirmation & reminder emails  │
│  SendGrid API Key  [SG.•••••••••••] (masked)      │
│  From Email        [noreply@yourdomain.com]        │
│  From Name         [Your Business Name]            │
└───────────────────────────────────────────────────┘
```

**Reminder Alerts** — iOS Calendar-style multi-select chip UI with independent email/SMS toggles and a custom offset input:

```
┌───────────────────────────────────────────────────┐
│ 🔔  Reminder Alerts                               │
├───────────────────────────────────────────────────┤
│  Choose when to send reminders before each        │
│  appointment. Multiple alerts — like iOS Calendar │
│                                                   │
│  [✓] Email reminders    [ ] SMS reminders         │
│                                                   │
│  [5 min] [10 min] [15 min] [30 min] [1 hour]      │
│  [2 hours] [3 hours] [6 hours] [12 hours]         │
│  [🔔 1 day ✓] [2 days] [1 week]                  │
│                                                   │
│  Custom:  [3] [Weeks ▼] [+ Add]                  │
│           ↳ adds "3 weeks" chip (removable)       │
└───────────────────────────────────────────────────┘
```

**Settings layout** — card sections (Business Profile, Branding, Payment Settings, Booking Settings, AI Assistant) are fully dark-mode compatible; section headers use the theme surface colour instead of hard-coded white:

```
┌ Light mode ──────────────────────────────────────┐  ┌ Dark mode ───────────────────────────────────────┐
│ 🏢  Business Profile                             │  │ 🏢  Business Profile                             │
│  Business Name   [Elite Hair Studio            ] │  │  Business Name   [Elite Hair Studio            ] │
│  Business Type   [Salon               ▼]         │  │  Business Type   [Salon               ▼]         │
├──────────────────────────────────────────────────┤  ├──────────────────────────────────────────────────┤
│ 🎨  Branding & Appearance                        │  │ 🎨  Branding & Appearance                        │
│  Primary Colour  [🟣] [#6c5ce7        ]          │  │  Primary Colour  [🟣] [#6c5ce7        ]          │
└──────────────────────────────────────────────────┘  └──────────────────────────────────────────────────┘
 ↑ surface white bg                                     ↑ surface dark bg — no white flash
```

### Super Admin Console (`/super-admin`) — new tab

**RevenueCat Config tab** (visible to `SuperAdmin` only):
- Set the RevenueCat platform API key and entitlement identifier
- Configure monthly/annual prices and RevenueCat product IDs per tier (Free → Enterprise)
- Tenant management: search, copy Tenant ID, delete

---

## Quick Start — Demo Data

All seed data is installed **automatically at API startup** via EF Core `MigrateAsync()`.  
No manual scripts needed — just run the API.

### Demo Accounts

| Role | Email | Password |
|------|-------|----------|
| SuperAdmin | superadmin@bookit.app | `SuperAdmin123!` |
| TenantAdmin | admin@demo-barber.com | `Admin123!` |
| Manager | manager@demo-barber.com | `Manager123!` |
| Staff (John) | staff@demo-barber.com | `Staff123!` |
| Staff (James) | james@elitehair.com | `Staff123!` |
| Staff (Emma) | emma@elitehair.com | `Staff123!` |
| Staff (Oliver) | oliver@urbanstyle.com | `Staff123!` |
| Customer | customer@example.com | `Customer123!` |

> Tenant slug: **demo-barber**  
> Full account list with GUIDs, CSV data, and appointment details → **[SEEDS.md](SEEDS.md)**

### Role Capabilities

| Role | Can do |
|------|--------|
| SuperAdmin | Full platform access — all tenants |
| TenantAdmin | Full access within their tenant |
| Manager | Manage staff / services (TenantId hidden) |
| Staff | View own appointments, cancel with reason, view customers |
| Customer | Book appointments, view own bookings |

---

## Projects

| Project | Type | Description |
|---|---|---|
| `BookIt.API` | ASP.NET Core Web API | REST API — auth, appointments, tenants, customers, webhooks |
| `BookIt.Core` | Class Library | Entities, DTOs, enums, interfaces, feature flags |
| `BookIt.Infrastructure` | Class Library | EF Core, repositories, background services |
| `BookIt.Web` | ASP.NET Core MVC | Original Razor/MVC front end with dark-mode SCSS theme |
| `BookIt.Blazor` | Blazor Server | Admin + public front end — MudBlazor 9, dark/light mode |
| `BookIt.Maui` | .NET MAUI Blazor Hybrid | iOS / Android / macOS / Windows mobile app |
| `BookIt.UI.Shared` | Razor Class Library | Shared MudBlazor components for Blazor + MAUI |
| `BookIt.Payments.Stripe` | Class Library | `IStripeProvider` — Stripe Payment Intents v2 |
| `BookIt.Payments.PayPal` | Class Library | `IPayPalProvider` — PayPal Orders v2 |
| `BookIt.Payments.ApplePay` | Class Library | `IApplePayProvider` — Apple Pay via Stripe (isolated reusable DLL) |
| `BookIt.Subscriptions.RevenueCat` | Class Library | `IRevenueCatProvider` — RevenueCat subscription management (isolated reusable DLL) |
| `BookIt.Notifications.Sms` | Class Library | `ISmsProvider` — ClickSend & Twilio SMS (isolated reusable DLL) |
| `BookIt.Notifications.Email` | Class Library | `IEmailNotificationService` — SendGrid booking confirmations & reminders (isolated reusable DLL) |
| `BookIt.Tests` | xUnit | Unit tests (31 passing) |

---

## Features

### Booking & Calendar
- Public booking page per tenant (e.g. `/demo-barber/book`)
- Full month/week calendar with availability management
- Multi-staff support with individual schedules
- **Staff picker on booking page** — customers choose "Any" or a preferred staff member; filters available slots
- Automatic booking confirmation emails (SendGrid)

### Approve / Decline Bookings
- Admin can **Approve** or **Decline** any pending appointment from the Calendar or Dashboard
- On approval: sends a branded `BookingApproved` HTML email (SendGrid) + fires `appointment.approved` webhook
- On decline: sends a branded `BookingDeclined` HTML email + fires `appointment.declined` webhook
- API: `POST /api/tenants/{slug}/appointments/{id}/approve` and `.../decline`

### Staff Management (all business types)
- Full CRUD for staff members: name, email, phone, photo URL, bio, sort order, active flag
- **Assign staff to services** — each staff member can be linked to specific services they perform
- Works for every business type: barber, salon, gym, physio, spa, recruitment, hotel, etc.
- API: `GET/POST/PUT/DELETE /api/tenants/{slug}/staff` + `PUT /staff/{id}/services`
- Admin page at `/{slug}/admin/Staff` with card layout and three modals (Add / Edit / Assign Services)
- Add / Edit modals use two-column `admin-form-grid` with Bootstrap icons on contact fields (✉ email, ✆ phone, 🖼 photo URL)

### Classes & Group Sessions (all business types)
- Schedule group classes/sessions that multiple customers can book into
- **Multiple instructors per session** — select any number of active staff as instructors
- Fields: name, linked service, description, date, start time, duration, max capacity, price, location/room
- Capacity indicator shows `booked / max` with red badge when full
- Status tracking: Scheduled / In Progress / Completed / Cancelled / Full
- API: `GET/POST/PUT/DELETE /api/tenants/{slug}/class-sessions`
- Admin page at `/{slug}/admin/Classes` — searchable table, Add / Edit / Cancel modals
- See [docs/Staff-and-Classes.md](docs/Staff-and-Classes.md) for full details

### Customer CRM
- Dedicated `Customer` entity + table with full contact info and **Membership Number**
- **Auto-created on every booking** — first booking creates the profile; repeat bookings update contact details, increment `TotalBookings`, and refresh `LastVisit`
- **Booking form pre-fill** — returning customers who type their email get their name and phone pre-filled ("Welcome back!" hint)
- Admin CRUD page at `/{slug}/admin/Customers` — searchable table, full Add / Edit / Delete modals
- Fields: name, email, phone, mobile, address, gender, membership number, tags, notes, marketing/SMS opt-ins
- Add / Edit modals organised into four labelled sections (Personal Details · Contact Information · Address · Notes & Tags) with Bootstrap icons on contact fields
- Public lookup endpoint: `GET /api/tenants/{slug}/customers/lookup?email=X`
- Full REST API: `GET/POST/PUT/DELETE /api/tenants/{slug}/customers`
- Webhook events: `customer.created`, `customer.updated`, `customer.deleted`

### Webhooks
- `Webhook` + `WebhookDelivery` entities (soft-delete, per-tenant, EF-stored)
- HMAC-SHA256 signed JSON envelope to all matching active endpoints
- Events: `appointment.created` · `appointment.cancelled` · `appointment.approved` · `appointment.declined` · `customer.created` · `customer.updated` · `customer.deleted`
- Full CRUD + `/deliveries` history

### Booking Forms
- Visual Form Builder with field toolbox (Text, Email, Phone, Number, Date, Dropdown, Radio, Checkboxes, File Upload, Rating, Signature, Heading, Paragraph, Services & Prices)
- Forms data grid with **Settings ⚙ / Builder 🔨 / Delete 🗑** per form
- **Settings dialog** — inline rename, description, welcome/confirmation messages, toggles, default flag
- Fully **mobile-responsive** — builder collapses to single column on screens < 992 px
- Full CRUD: `GET/POST/PUT/DELETE /api/tenants/{slug}/booking-forms`

### Interviews (Recruitment Module)
- Create interview slots with staff assignment
- **Video conference integration** — Teams / Zoom / Google Meet / Webex / GoTo / Jitsi / Whereby / Other
- Stores: meeting ID, password, join URL, host URL, dial-in — all included in confirmation email
- Candidate invitation flow via unique token link

### Notifications
- **SMS** — ClickSend & Twilio providers, both in `BookIt.Notifications.Sms`; provider selected per tenant in Settings
- **Email** — SendGrid booking confirmations, reminders, and cancellations via `BookIt.Notifications.Email`
- **Reminder alerts** — iOS-calendar-style multi-selection (5 min · 10 min · 15 min · 30 min · 1 h · 2 h · … · 1 day · 2 days · 1 week)
- Enable email reminders and/or SMS reminders independently per tenant
- Scheduled via **Hangfire** background job manager (InMemory by default, SQLite/SQL Server in production)

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
- Dark sidebar with grouped navigation: **Main Menu** (Dashboard, Calendar) · **Management** (Services, **Staff**, **Classes**, **Customers**, Forms, Interviews, Booking Page) · **Configuration** (Settings, Subscription)
- Stat cards with colour-coded accent bars
- Today's schedule with status dots and coloured chips
- Quick-action panel
- Profile dropdown (Dashboard, Settings, Subscription, Sign Out)
- Dark / light mode toggle
- Fully mobile-responsive (collapsible sidebar overlay on small screens)

### Super Admin Console (`/super-admin`)
- Tenant management (list, search, delete, copy Tenant ID)
- **RevenueCat Configuration tab** — visible only to `SuperAdmin` role:
  - Platform API key and entitlement identifier
  - Per-tier pricing (monthly & annual) and RevenueCat product IDs for all four plans

### Blazor Front End Pages
| Page | Route |
|---|---|
| Home | `/` |
| Login | `/login` |
| Pricing | `/pricing` |
| Book Appointment | `/{slug}/book` |
| Admin Dashboard | `/{slug}/admin` |
| Admin Calendar | `/{slug}/admin/calendar` |
| Admin Services | `/{slug}/admin/services` |
| Admin Customers | `/{slug}/admin/customers` |
| Admin Forms | `/{slug}/admin/forms` |
| Admin Form Builder | `/{slug}/admin/forms/builder?formId={id}` |
| Admin Interviews | `/{slug}/admin/interviews` |
| Admin Email Templates | `/{slug}/admin/email-templates` |
| Admin Settings | `/{slug}/admin/settings` |
| Admin Subscriptions | `/{slug}/admin/subscriptions` |
| Super Admin | `/super-admin` |

### MVC Admin Pages (`BookIt.Web`)
| Page | Route |
|---|---|
| Admin Dashboard | `/{slug}/admin` |
| Admin Calendar | `/{slug}/admin/Calendar` |
| Admin Services | `/{slug}/admin/Services` |
| **Admin Staff** | `/{slug}/admin/Staff` |
| **Admin Classes** | `/{slug}/admin/Classes` |
| **Admin Customers** | `/{slug}/admin/Customers` |
| Admin Booking Forms | `/{slug}/admin/Forms` |
| Admin Form Builder | `/{slug}/admin/FormBuilder?formId={id}` |
| Admin Interviews | `/{slug}/admin/Interviews` |
| Admin Settings | `/{slug}/admin/Settings` |
| Admin Subscriptions | `/{slug}/admin/Subscriptions` |

### MAUI Mobile App (`BookIt.Maui`)
Cross-platform Blazor Hybrid app sharing `BookIt.UI.Shared` components.

**5-tab bottom navigation:**
| Tab | Route | Description |
|---|---|---|
| Dashboard | `/dashboard` | Stat cards + today's schedule |
| Calendar | `/calendar` | Month calendar + slot availability |
| Bookings | `/appointments` | Upcoming bookings with per-item QR button |
| Wallet | `/wallet` | Branded pass card with QR, calendar export, share |
| Profile | `/profile` | User info, sign out |

**QR Wallet features:**
- Branded wallet card (business logo, name, date/time, services, PIN)
- **Membership number** shown on card and encoded in QR data
- QR data: `BOOKIT:{id}:{pin}:{startYYYYMMDDHHmm}:{membershipNumber|NONE}`
- **📅 Add to Calendar** — generates ICS calendar event (iOS Calendar / Google Calendar)
- **↗ Share Pass** — generates PNG QR image and opens native share sheet

**Sign up** — optional Membership Number field, stored on user account and returned in all auth responses.

**Offline**: `MauiSyncService` caches data to on-device SQLite (sqlite-net-pcl). Tokens in OS `SecureStorage`.

### MAUI-Ready Shared Library (`BookIt.UI.Shared`)
All UI logic is in a Razor Class Library so the same components work in Blazor Server, WASM and **.NET MAUI**:

```csharp
// In your MAUI app's MauiProgram.cs
builder.Services.AddBookItUI("https://api.bookit.app");
```

Components available:
- `AdminLayout` — full dark sidebar + topbar + profile dropdown
- `DashboardView`, `ServicesView`, `SettingsView`, `SubscriptionsView`
- `AppointmentQrCard` — wallet-style branded QR pass card (membership number, PIN, QR encoded with date/time)
- `ChatModerationView` — admin AI chat settings panel (system prompt, blocked phrases, flagged message review)
- `LoginView`, `PricingView`
- `BookItApiService` — stateless HTTP client (no `IHttpContextAccessor`)
- `BookItAuthState` — scoped auth state with initials + `MembershipNumber`
- `BookItTheme` — custom MudBlazor theme (brand colours, light + dark palettes)

---

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server / LocalDB (or SQLite — used automatically in development)
- Stripe API keys (optional — for online payments)
- PayPal client credentials (optional)
- SendGrid API key (optional — for booking confirmation and reminder emails)
- ClickSend or Twilio credentials (optional — for SMS notifications)
- RevenueCat API key (optional — for subscription entitlement resolution)

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

### Build the MAUI App (requires .NET MAUI workload)
```bash
# Install MAUI workload (once)
dotnet workload install maui

# Android
cd src/BookIt.Maui
dotnet build -f net10.0-android

# iOS (macOS only)
dotnet build -f net10.0-ios

# Windows
dotnet build -f net10.0-windows10.0.19041.0
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

### SMS Notifications (`BookIt.Notifications.Sms`)
```csharp
// Register both providers + factory
builder.Services.AddSmsNotifications();

// Use — factory selects ClickSend or Twilio based on tenant config
var provider = _smsFactory.Get(tenant.SmsProvider.ToString());
var result = await provider.SendAsync(toPhone, message, credentialString);
// ClickSend credential: "USERNAME:API_KEY"
// Twilio credential:    "ACCOUNT_SID:AUTH_TOKEN:FROM_NUMBER"
```

### Email Notifications (`BookIt.Notifications.Email`)
```csharp
// Register SendGrid email service
builder.Services.AddSendGridEmail();

// Use
await _emailService.SendBookingConfirmationAsync(apiKey, fromEmail, fromName,
    toEmail, customerName, businessName, serviceName, start, end, location, meetingLink, pin);

await _emailService.SendAppointmentReminderAsync(apiKey, fromEmail, fromName,
    toEmail, customerName, businessName, serviceName, start, minutesBefore, location, meetingLink);
```

### Reminder Scheduling (Hangfire)
```csharp
// Registered automatically via AddInfrastructure()
// Schedule reminders when an appointment is created:
_reminderScheduler.ScheduleReminders(appointmentId, tenantId, startTime, alertMinutes);

// Cancel reminders when cancelled/rescheduled:
_reminderScheduler.CancelReminders(appointmentId);
```

**Reminder alert options** (iOS Calendar-style, configurable per tenant):
`5 min · 10 min · 15 min · 30 min · 1 h · 2 h · 3 h · 6 h · 12 h · 1 day · 2 days · 1 week`

### MAUI Wallet Pass (`BookIt.Maui.Services.WalletPassService`)
```csharp
// Add to iOS Calendar / Google Calendar (ICS format)
await _walletPassService.AddToCalendarAsync(appointment, businessName, membershipNumber);

// Native share sheet — shares QR code as PNG image
await _walletPassService.ShareQrPassAsync(appointment, businessName, qrDataUri);

// Generate ICS string directly
var ics = _walletPassService.GenerateIcs(appointment, businessName, membershipNumber);
```

> **Note on Native Wallet Passes**: Full Apple PKPass (`.pkpass`) and Google Wallet JWT passes
> require platform developer certificates (Apple Developer Program / Google Pay & Wallet Console)
> and server-side signing. `WalletPassService` uses **ICS calendar events** as a
> cross-platform alternative that works without additional credentials.

---

## Database Migrations

See [docs/EF-Migrations.md](docs/EF-Migrations.md) for full migration instructions.

See [docs/Notifications.md](docs/Notifications.md) for SMS, SendGrid email, and Hangfire reminder scheduler setup.

See [docs/Staff-and-Classes.md](docs/Staff-and-Classes.md) for staff management, classes module, and customer pre-fill setup.

```bash
cd src/BookIt.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../BookIt.API
dotnet ef database update --startup-project ../BookIt.API
```

---

## Architecture

```
BookIt.API             ← HTTP entry point, controllers, middleware
    ↓
BookIt.Infrastructure  ← EF Core, repos, services (delegates to providers)
    ↓
BookIt.Core            ← Entities, DTOs, interfaces, enums, FeatureFlags
    ↓
BookIt.Payments.Stripe          ← IStripeProvider (isolated class library)
BookIt.Payments.PayPal          ← IPayPalProvider (isolated class library)
BookIt.Payments.ApplePay        ← IApplePayProvider (isolated — delegates to Stripe)
BookIt.Subscriptions.RevenueCat ← IRevenueCatProvider (isolated class library)
BookIt.Notifications.Sms        ← ISmsProvider / ClickSendSmsProvider / TwilioSmsProvider
BookIt.Notifications.Email      ← IEmailNotificationService / SendGridEmailService

BookIt.Blazor       ← Blazor Server front end (consumes BookIt.UI.Shared)
BookIt.Maui         ← .NET MAUI Blazor Hybrid (iOS/Android/macOS/Windows)
BookIt.Web          ← ASP.NET Core MVC front end
BookIt.UI.Shared    ← Razor Class Library (shared Blazor + MAUI components)
    ↑ AppointmentQrCard  (membership QR, wallet-style pass card)
    ↑ ChatModerationView (AI chat admin panel)
    ↑ BookItAuthState    (stores MembershipNumber from auth response)
    ↑ BookItApiService   (full REST client for API)
```

---

© 2026 BookIt
