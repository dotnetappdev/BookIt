# BookIt — Staff Management, Classes & Customer Pre-fill

This document covers the staff management module, classes & group sessions module, and per-customer booking form data (auto-created profiles + booking form pre-fill).

---

## Table of Contents

1. [Overview](#overview)
2. [Staff Management](#staff-management)
   - [Entity & Database](#staff-entity--database)
   - [API Endpoints](#staff-api-endpoints)
   - [Admin UI](#staff-admin-ui)
   - [Booking Flow — Staff Picker](#booking-flow--staff-picker)
3. [Classes & Group Sessions](#classes--group-sessions)
   - [Entity & Database](#classes-entity--database)
   - [API Endpoints](#classes-api-endpoints)
   - [Admin UI](#classes-admin-ui)
4. [Customer Profiles & Booking Form Pre-fill](#customer-profiles--booking-form-pre-fill)
   - [Auto-create on Booking](#auto-create-on-booking)
   - [Booking Form Pre-fill](#booking-form-pre-fill)
   - [Admin CRUD](#admin-crud)
   - [API Endpoints](#customer-api-endpoints)
5. [Admin Screenshots](#admin-screenshots)
6. [EF Migrations](#ef-migrations)

---

## Overview

| Feature | Route / Endpoint | Who can use |
|---|---|---|
| Staff CRUD | `/{slug}/admin/Staff` | Admin (all business types) |
| Classes CRUD | `/{slug}/admin/Classes` | Admin (all business types) |
| Customer CRUD | `/{slug}/admin/Customers` | Admin |
| Staff picker | `/{slug}/book/selectslot` | Customers |
| Booking pre-fill | `/{slug}/booking/details` | Customers |
| Customer lookup | `GET /api/tenants/{slug}/customers/lookup?email=X` | Public |

---

## Staff Management

### Staff Entity & Database

```csharp
public class Staff : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }       // optional link to ApplicationUser
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public ICollection<StaffService> Services { get; set; }    // assigned services
    public ICollection<StaffAvailability> Availability { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}
```

`StaffService` is the many-to-many join between `Staff` and `Service`:

```csharp
public class StaffService
{
    public Guid StaffId { get; set; }
    public Guid ServiceId { get; set; }
}
```

> Staff management has **no business-type restriction**. It works for every business type configured in the `BusinessType` enum: barber, salon, gym, physio, spa, hotel, recruitment, etc.

### Staff API Endpoints

All endpoints are under `/api/tenants/{tenantSlug}/staff`.

| Method | Path | Auth | Description |
|---|---|---|---|
| `GET` | `/staff` | Public | Active staff only |
| `GET` | `/staff/all` | Admin | All staff including inactive |
| `GET` | `/staff/{id}` | Admin | Single staff member |
| `POST` | `/staff` | Admin | Create staff member |
| `PUT` | `/staff/{id}` | Admin | Update staff member |
| `DELETE` | `/staff/{id}` | Admin | Soft-delete staff member |
| `PUT` | `/staff/{id}/services` | Admin | Replace service assignments |

**Create / Update request body:**

```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "phone": "07700 900 123",
  "photoUrl": "https://example.com/photo.jpg",
  "bio": "Senior stylist with 10 years experience",
  "sortOrder": 1,
  "isActive": true
}
```

**Assign services request body:**

```json
{
  "serviceIds": [
    "44444444-4444-4444-4444-444444444401",
    "44444444-4444-4444-4444-444444444402"
  ]
}
```

### Staff Admin UI

Navigate to `/{slug}/admin/Staff`.

Three modals:

- **Add Staff Member** — name, email, phone, photo URL, bio, sort order, active toggle
- **Edit Staff Member** — same fields, pre-populated; data passed via `data-*` attributes (XSS-safe)
- **Assign Services** — checkbox list of all tenant services; saves via `PUT /staff/{id}/services`

Staff are displayed as cards showing initials/photo, contact info, assigned service badges, and active status.

### Booking Flow — Staff Picker

On the slot selection page (`/{slug}/book/selectslot?serviceId=X`), customers see a staff picker above the date selector:

```
Who do you prefer?
[Any staff member ▾]  [Jane Smith]  [Bob Jones]
```

- Default is "Any" — shows all available slots across all staff
- Selecting a staff member filters slots to that person only
- The selected `staffId` is threaded through to the booking details URL:
  `/{slug}/booking/details?serviceId=X&staffId=Y&date=2026-03-04&time=10:00`

---

## Classes & Group Sessions

### Classes Entity & Database

```csharp
public class ClassSession : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid? StaffId { get; set; }            // primary instructor (backward compat)
    public string? InstructorIdsJson { get; set; } // JSON array of all instructor Staff IDs
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public int MaxCapacity { get; set; } = 20;
    public int CurrentBookings { get; set; } = 0;
    public decimal Price { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Scheduled;
    public string? Location { get; set; }
    public ClassType ClassType { get; set; } = ClassType.General;
    public bool IsFull => CurrentBookings >= MaxCapacity;
    public int SpotsRemaining => MaxCapacity - CurrentBookings;
    public List<Guid> InstructorIds { get; }       // computed: merges StaffId + InstructorIdsJson
}
```

**`ClassType` enum** (not gym-specific — `General` covers any use case):

```
General, Yoga, Pilates, Spinning, Swimming, Aerobics, BodyPump, Zumba, CrossFit,
HiitCardio, WaterAerobics, OpenSwim, KidsSwim, PersonalTraining, Other
```

**EF migration** `20260226000001_ClassSessionMultiInstructors` adds `InstructorIdsJson nvarchar(max)` to `ClassSessions`.

### Classes API Endpoints

All endpoints are under `/api/tenants/{tenantSlug}/class-sessions`.

| Method | Path | Auth | Description |
|---|---|---|---|
| `GET` | `/class-sessions?days=90` | Public | Upcoming sessions |
| `GET` | `/class-sessions/{id}` | Public | Single session |
| `POST` | `/class-sessions` | Admin | Create session |
| `PUT` | `/class-sessions/{id}` | Admin | Update session |
| `DELETE` | `/class-sessions/{id}` | Admin | Cancel session (soft) |
| `POST` | `/class-sessions/{id}/book` | Public | Book a spot |

**Create / Update request body:**

```json
{
  "serviceId": "44444444-...",
  "name": "Morning Yoga",
  "description": "Energising 60-minute yoga session",
  "sessionDate": "2026-03-10T00:00:00Z",
  "startTime": "09:00",
  "durationMinutes": 60,
  "maxCapacity": 15,
  "price": 12.00,
  "location": "Studio 1",
  "classType": 2,
  "instructorIds": [
    "aaaaaaaa-...",
    "bbbbbbbb-..."
  ]
}
```

The first entry in `instructorIds` is stored as `StaffId` for backward compatibility.

### Classes Admin UI

Navigate to `/{slug}/admin/Classes`.

| UI element | Description |
|---|---|
| Searchable table | Name, date/time, duration, capacity badge, price, instructor badges, status |
| Capacity badge | `booked / max` — red when full |
| Instructor badges | One badge per assigned instructor |
| **New Class** modal | All fields + multi-checkbox instructor list |
| **Edit** modal | Pre-populates all fields; existing instructors pre-checked |
| **Cancel** | Confirmation dialog; marks session as Cancelled |

---

## Customer Profiles & Booking Form Pre-fill

### Auto-create on Booking

Every time a customer completes a booking via `POST /api/tenants/{slug}/appointments`, the API upserts a `Customer` record:

```
IF customer (email + tenantId) does NOT exist → create new Customer
IF customer exists → update Name, Phone; increment TotalBookings; refresh LastVisit
```

This means no manual data entry is needed — the Customers page in the admin always reflects up-to-date booking history.

### Booking Form Pre-fill

On the booking details page (`/{slug}/booking/details`), when a returning customer types their email:

1. A debounced fetch calls `GET /api/tenants/{slug}/customers/lookup?email=X`
2. If a profile is found, name and phone fields are pre-populated (only if currently blank)
3. A "Welcome back, {name}!" hint appears below the email field

### Admin CRUD

Navigate to `/{slug}/admin/Customers`.

| Field | Notes |
|---|---|
| First Name, Last Name | Required for create |
| Email | Required; keyed with tenant for uniqueness |
| Phone, Mobile | Optional |
| Gender | Dropdown: Not specified / Male / Female / Non-binary / Prefer not to say |
| Address, City, Post Code, Country | Optional |
| Membership Number | For gym, club, loyalty schemes |
| Tags | Comma-separated, e.g. "VIP, New Client" |
| Notes | Internal admin notes |
| Marketing Opt-in | Checkbox |
| SMS Opt-in | Checkbox |

The table shows all customers with a live search filter across name, email, and phone.

All string values are populated into modals via `data-*` attributes to prevent XSS from names or notes containing special characters.

### Customer API Endpoints

| Method | Path | Auth | Description |
|---|---|---|---|
| `GET` | `/customers` | Admin | All customers |
| `GET` | `/customers/lookup?email=X` | Public | Pre-fill lookup |
| `GET` | `/customers/{id}` | Admin | Single customer |
| `POST` | `/customers` | Admin | Create customer |
| `PUT` | `/customers/{id}` | Admin | Update customer |
| `DELETE` | `/customers/{id}` | Admin | Soft-delete customer |

---

## Admin Screenshots

**Staff page** — card layout, service badges, Add/Edit/Assign modals:

![Admin Staff Page](https://github.com/user-attachments/assets/4bcb0bd6-c0b4-4949-ace5-f29098400170)

**Classes page** — searchable table, multi-instructor Add modal:

![Admin Classes Page](https://github.com/user-attachments/assets/e36dc566-8372-4b17-923d-842b6e111a4e)

**Customers page** — full CRUD, auto-populated from bookings:

![Admin Customers Page](https://github.com/user-attachments/assets/d760ca5c-f5fe-4333-828f-cb3809d1e3fe)

**Booking Forms page** — mobile-responsive form builder:

![Admin Booking Forms Page](https://github.com/user-attachments/assets/ed361fb4-2b42-4b41-9f94-78355ee4e361)

---

## EF Migrations

The following migrations are applied to support these features (via `EnsureCreated` in development or `Migrate()` in production):

| Migration | Table(s) changed |
|---|---|
| `InitialCreate` | All core tables including `Staff`, `StaffServices`, `ClassSessions`, `Customers` |
| `IdentityAndClassSessions` | ASP.NET Identity columns; `ClassSessions` + `ClassSessionBookings` tables |
| `ClassSessionMultiInstructors` | Adds `InstructorIdsJson` column to `ClassSessions` |

To apply pending migrations:

```bash
dotnet ef database update \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```

To add a new migration after entity changes:

```bash
dotnet ef migrations add <MigrationName> \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```

See [EF-Migrations.md](EF-Migrations.md) for the complete guide.
