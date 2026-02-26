# BookIt — Notifications (SMS, Email & Reminders)

This document covers the SMS notifications, email notifications, and appointment reminder
scheduler added to BookIt. All notification providers are packaged as **isolated, reusable
class library DLLs** that can be consumed by any .NET app.

---

## Table of Contents

1. [Overview](#overview)
2. [Architecture](#architecture)
3. [SMS Notifications (`BookIt.Notifications.Sms`)](#sms-notifications)
   - [ClickSend](#clicksend-provider)
   - [Twilio](#twilio-provider)
   - [Provider Factory](#provider-factory)
   - [DI Registration](#sms-di-registration)
4. [Email Notifications (`BookIt.Notifications.Email`)](#email-notifications)
   - [SendGrid Service](#sendgrid-service)
   - [Email Templates](#email-templates)
   - [DI Registration](#email-di-registration)
5. [Reminder Scheduler (Hangfire)](#reminder-scheduler)
   - [Alert Configuration (iOS Calendar-style)](#alert-configuration)
   - [AppointmentReminderJob](#appointmentreminderjob)
   - [Cancellation Handling](#cancellation-handling)
6. [Tenant Settings UI](#tenant-settings-ui)
   - [SMS Notifications Section](#sms-notifications-section)
   - [Email Notifications Section](#email-notifications-section)
   - [Reminder Alerts Section](#reminder-alerts-section)
7. [Database Fields](#database-fields)
8. [Getting Started — Quick Setup](#getting-started)
9. [Production Hangfire Storage](#production-hangfire-storage)

---

## Overview

| Feature | Library | Provider(s) |
|---|---|---|
| SMS notifications | `BookIt.Notifications.Sms` | ClickSend, Twilio |
| Booking confirmation email | `BookIt.Notifications.Email` | SendGrid |
| Appointment reminder email | `BookIt.Notifications.Email` | SendGrid |
| Booking cancellation email | `BookIt.Notifications.Email` | SendGrid |
| Reminder job scheduling | Hangfire (in Infrastructure) | InMemory (dev), SQL Server (prod) |

Credentials for each provider are configured **per tenant** in the admin Settings page,
meaning one BookIt installation can support multiple tenants each with their own SMS and
email providers.

---

## Architecture

```
BookIt.Infrastructure
    ├── HangfireReminderScheduler   (IReminderScheduler)
    ├── AppointmentReminderJob      (Hangfire background job)
    │       ├── IEmailNotificationService  ─→ BookIt.Notifications.Email
    │       └── SmsProviderFactory         ─→ BookIt.Notifications.Sms
    └── DependencyInjection         (registers everything)

BookIt.Notifications.Sms
    ├── ISmsProvider
    ├── ClickSendSmsProvider
    ├── TwilioSmsProvider
    └── SmsProviderFactory

BookIt.Notifications.Email
    ├── IEmailNotificationService
    ├── SendGridEmailService
    └── EmailTemplates (internal HTML generator)
```

Data flow for a new booking:

```
1. Appointment created  →  AppointmentService
2.                      →  IReminderScheduler.ScheduleReminders(...)
3. Hangfire enqueues delayed jobs for each alert offset (e.g., 1 day before, 1 hour before)
4. Job fires at scheduled time  →  AppointmentReminderJob.SendAsync(...)
5.                              →  SendGrid reminder email (if EnableEmailReminders)
6.                              →  ClickSend / Twilio SMS (if EnableSmsReminders)
```

---

## SMS Notifications

### ClickSend Provider

`ClickSendSmsProvider` calls the [ClickSend REST API v3](https://developers.clicksend.com/docs/rest/v3/).

Tenant credentials required:

| Field in Settings | Tenant property | Description |
|---|---|---|
| ClickSend Username | `ClickSendUsername` | Your ClickSend account username |
| ClickSend API Key | `ClickSendApiKey` | Found in the ClickSend dashboard → API Credentials |
| From Number / Name | `ClickSendFromNumber` | Sender ID or E.164 number (e.g. `+447700900000`) |

The provider constructs Basic Auth from `USERNAME:API_KEY` at runtime — credentials are
**never stored in a single string** to avoid accidental logging.

### Twilio Provider

`TwilioSmsProvider` calls the [Twilio Messages API](https://www.twilio.com/docs/sms/api).

Tenant credentials required:

| Field in Settings | Tenant property | Description |
|---|---|---|
| Twilio Account SID | `TwilioAccountSid` | Found in the Twilio Console dashboard |
| Twilio Auth Token | `TwilioAuthToken` | Found in the Twilio Console dashboard |
| Twilio From Number | `TwilioFromNumber` | A Twilio-verified E.164 number (e.g. `+441234567890`) |

### Provider Factory

`SmsProviderFactory` selects the right provider at runtime based on the tenant's configured
`SmsProvider` enum value:

```csharp
public enum SmsProvider
{
    None      = 0,
    ClickSend = 1,
    Twilio    = 2
}
```

```csharp
// Usage inside AppointmentReminderJob
var provider = _smsFactory.Get(tenant.SmsProvider.ToString());
// Returns ClickSendSmsProvider by default, TwilioSmsProvider for "twilio"
```

### SMS DI Registration

```csharp
// Program.cs / DependencyInjection.cs
builder.Services.AddSmsNotifications();
// Registers: ClickSendSmsProvider, TwilioSmsProvider, SmsProviderFactory
```

---

## Email Notifications

### SendGrid Service

`SendGridEmailService` wraps the official [SendGrid .NET library](https://github.com/sendgrid/sendgrid-csharp)
and exposes three methods via `IEmailNotificationService`:

| Method | Triggered when |
|---|---|
| `SendBookingConfirmationAsync` | Appointment created |
| `SendAppointmentReminderAsync` | Hangfire reminder job fires |
| `SendBookingCancellationAsync` | Appointment cancelled |

All methods accept credentials per-call so a single DI-registered instance serves all tenants.

Tenant configuration required:

| Field in Settings | Tenant property | Description |
|---|---|---|
| SendGrid API Key | `SendGridApiKey` | Create at [app.sendgrid.com](https://app.sendgrid.com) → API Keys |
| From Email | `SendGridFromEmail` | Must be a verified sender in SendGrid |
| From Name | `SendGridFromName` | Display name shown to recipients |

### Email Templates

All templates are responsive HTML with the BookIt brand gradient header. They include:

**Booking Confirmation**
- Service name, date, time range
- Location or meeting link
- Booking PIN (6-character alphanumeric, displayed in a prominent box)
- Business branding

**Appointment Reminder**
- Natural-language "in X minutes / hours / tomorrow / in N days" phrasing
- Appointment details in a coloured callout card
- Direct meeting link (if virtual)

**Booking Cancellation**
- Cancelled appointment details
- Cancellation reason (if provided)

### Email DI Registration

```csharp
builder.Services.AddSendGridEmail();
// Registers: IEmailNotificationService → SendGridEmailService
```

---

## Reminder Scheduler

### Alert Configuration

Each tenant configures reminder alerts just like iOS Calendar — selecting one or more preset
alert times from a chip UI in Settings, with an option to add a fully custom offset:

| Preset | Minutes before |
|---|---|
| 5 min | 5 |
| 10 min | 10 |
| 15 min | 15 |
| 30 min | 30 |
| 1 hour | 60 |
| 2 hours | 120 |
| 3 hours | 180 |
| 6 hours | 360 |
| 12 hours | 720 |
| 1 day | 1 440 |
| 2 days | 2 880 |
| 1 week | 10 080 |
| **Custom** | Any value in hours, days, or weeks |

The **Custom** option lets you type any number and choose a unit (Hours / Days / Weeks).
For example, entering `3` and selecting `weeks` adds a reminder 3 weeks before the appointment.

Stored in `Tenant.ReminderAlerts` as a comma-separated string of minutes (e.g. `"60,1440"`).

### AppointmentReminderJob

When a reminder job fires, `AppointmentReminderJob.SendAsync(...)`:

1. Re-loads the appointment (including tenant config) from the database
2. **Skips silently** if the appointment is not found or is cancelled
3. Sends a SendGrid reminder email (if `EnableEmailReminders && SendGridApiKey != null`)
4. Sends an SMS reminder (if `EnableSmsReminders && EnableSmsNotifications && CustomerPhone != null`)
5. Sets `Appointment.ReminderSent = true`

```csharp
// Schedule reminders at booking time
_reminderScheduler.ScheduleReminders(
    appointmentId,
    tenantId,
    appointmentStart,
    tenant.ReminderAlerts?.Split(',') ?? new[] { "1440" });

// Cancel reminders if appointment is cancelled/rescheduled
_reminderScheduler.CancelReminders(appointmentId);
```

### Cancellation Handling

`HangfireReminderScheduler.CancelReminders` logs the cancellation intent.
Even if the Hangfire job is not deleted (basic Hangfire API limitation), the job always
re-checks the appointment status at fire time and silently skips if cancelled — so no
spurious notifications are ever sent.

---

## Tenant Settings UI

The admin Settings page (`/{slug}/admin/settings`) has three new sections, all saved via
the existing "Save Settings" button with toast feedback.

### SMS Notifications Section

```
┌─────────────────────────────────────────────────────┐
│ 💬  SMS Notifications                               │
├─────────────────────────────────────────────────────┤
│  [Toggle] Enable SMS notifications                  │
│                                                     │
│  SMS Provider:  [ClickSend ▼]                       │
│                                                     │
│  ClickSend Username   [____________]                │
│  ClickSend API Key    [••••••••••••] (masked)       │
│  From Number / Name   [+447700900000]               │
└─────────────────────────────────────────────────────┘
```

When **Twilio** is selected:
```
│  Twilio Account SID   [____________]                │
│  Twilio Auth Token    [••••••••••••] (masked)       │
│  Twilio From Number   [+441234567890]               │
```

### Email Notifications Section

```
┌─────────────────────────────────────────────────────┐
│ ✉️  Email Notifications (SendGrid)                  │
├─────────────────────────────────────────────────────┤
│  [Toggle] Enable booking confirmation & reminder    │
│           emails                                    │
│                                                     │
│  SendGrid API Key  [SG.xxxxx••••••••] (masked)     │
│  From Email        [noreply@yourdomain.com]         │
│  From Name         [Your Business Name]             │
└─────────────────────────────────────────────────────┘
```

### Reminder Alerts Section

```
┌─────────────────────────────────────────────────────┐
│ 🔔  Reminder Alerts                                 │
├─────────────────────────────────────────────────────┤
│  Choose when to send reminders before each          │
│  appointment. Multiple alerts can be set —          │
│  just like iOS Calendar.                            │
│                                                     │
│  [✓] Email reminders    [ ] SMS reminders           │
│                                                     │
│  Alert times:                                       │
│  [5 min] [10 min] [15 min] [30 min] [1 hour]       │
│  [2 hours] [3 hours] [6 hours] [12 hours]           │
│  [🔔 1 day] [2 days] [1 week]                      │
│  (selected chips are highlighted in purple)         │
│                                                     │
│  Custom alert (any value):                          │
│  Value [3]  Unit [Weeks ▼]  [+ Add]                │
│  ↳ Adds a removable "3 weeks" chip above the row   │
└─────────────────────────────────────────────────────┘
```

---

## Database Fields

New columns added to the `Tenants` table (applied via EF Core migration or `EnsureCreated`):

### SMS fields

| Column | Type | Default | Description |
|---|---|---|---|
| `SmsProvider` | `int` | `0` (None) | 0=None, 1=ClickSend, 2=Twilio |
| `ClickSendUsername` | `nvarchar(max)` | null | ClickSend account username |
| `ClickSendApiKey` | `nvarchar(max)` | null | ClickSend API key (encrypted at rest recommended) |
| `ClickSendFromNumber` | `nvarchar(max)` | null | ClickSend sender ID or phone number |
| `TwilioAccountSid` | `nvarchar(max)` | null | Twilio Account SID |
| `TwilioAuthToken` | `nvarchar(max)` | null | Twilio Auth Token (encrypted at rest recommended) |
| `TwilioFromNumber` | `nvarchar(max)` | null | Twilio from phone number |
| `EnableSmsNotifications` | `bit` | `0` | Master SMS on/off switch |

### Email fields

| Column | Type | Default | Description |
|---|---|---|---|
| `SendGridApiKey` | `nvarchar(max)` | null | SendGrid API key (encrypted at rest recommended) |
| `SendGridFromEmail` | `nvarchar(max)` | null | Verified sender email address |
| `SendGridFromName` | `nvarchar(max)` | null | Display name for outbound emails |
| `EnableEmailNotifications` | `bit` | `0` | Master email notifications on/off switch |

### Reminder fields

| Column | Type | Default | Description |
|---|---|---|---|
| `ReminderAlerts` | `nvarchar(max)` | `"1440"` | Comma-separated alert offsets in minutes |
| `EnableEmailReminders` | `bit` | `1` | Send email reminders |
| `EnableSmsReminders` | `bit` | `0` | Send SMS reminders |

> ⚠️ **Security note:** API keys and auth tokens stored in the database should be encrypted
> at rest using SQL Server Transparent Data Encryption (TDE) or application-level encryption
> before deploying to production.

---

## Getting Started

### 1. Register services

```csharp
// In Program.cs (or DependencyInjection.cs)
builder.Services.AddInfrastructure(configuration, environment);
// This automatically calls:
//   AddSmsNotifications()
//   AddSendGridEmail()
//   AddHangfire(config => config.UseInMemoryStorage())
//   AddHangfireServer()
```

### 2. Add Hangfire dashboard (optional)

```csharp
// In Program.cs
app.UseHangfireDashboard("/hangfire");
// Restrict to SuperAdmin in production!
```

### 3. Configure a tenant in Settings

1. Navigate to `/{your-slug}/admin/settings`
2. Scroll to **SMS Notifications** → enable and enter credentials
3. Scroll to **Email Notifications (SendGrid)** → enable and enter API key / from details
4. Scroll to **Reminder Alerts** → select alert times and enable email/SMS reminders
5. Click **Save Settings** — a toast confirms success

### 4. Test it

After saving, create a test appointment to verify:
- A booking confirmation email arrives via SendGrid
- Reminder jobs appear in the Hangfire dashboard at the correct fire times

---

## Production Hangfire Storage

The default InMemory storage **does not persist jobs across restarts**. For production,
switch to a persistent store:

### SQL Server

```bash
dotnet add package Hangfire.SqlServer
```

```csharp
services.AddHangfire(config =>
    config.UseSqlServerStorage(connectionString));
```

### SQLite

```bash
dotnet add package Hangfire.Sqlite
```

```csharp
services.AddHangfire(config =>
    config.UseSQLiteStorage("hangfire.db"));
```

Replace the `UseInMemoryStorage()` call in `BookIt.Infrastructure/DependencyInjection.cs`.
