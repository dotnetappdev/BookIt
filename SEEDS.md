# BookIt — Seed Data Reference

All demo data is installed automatically at API startup via EF Core migrations.  
No manual steps needed — just run the API and the data is ready.

---

## User Accounts

| Role | Email | Password | Tenant |
|------|-------|----------|--------|
| SuperAdmin | superadmin@bookit.app | SuperAdmin123! | demo-barber (platform admin) |
| TenantAdmin | admin@demo-barber.com | Admin123! | demo-barber |
| Manager | manager@demo-barber.com | Manager123! | demo-barber |
| Staff | staff@demo-barber.com | Staff123! | demo-barber |
| Staff | james@elitehair.com | Staff123! | demo-barber |
| Staff | emma@elitehair.com | Staff123! | demo-barber |
| Staff | oliver@urbanstyle.com | Staff123! | demo-barber |
| Customer (client contact) | sarah@elitehair.com | Customer123! | demo-barber |
| Customer (client contact) | michael@urbanstyle.com | Customer123! | demo-barber |
| Customer | customer@example.com | Customer123! | demo-barber |

> **Login URL (Blazor):** `https://localhost:5001/login`  
> **Org / tenant slug:** `demo-barber`

---

## Tenant

```
Id:   11111111-1111-1111-1111-111111111111
Slug: demo-barber
Name: Demo Barber Shop
```

---

## Client Companies

```csv
Id,CompanyName,ContactName,Email,Phone,TenantId,Notes
ff000000-0000-0000-0000-000000000001,Elite Hair Solutions,Sarah Johnson,sarah@elitehair.com,555-0101,11111111-1111-1111-1111-111111111111,VIP client - priority scheduling
ff000000-0000-0000-0000-000000000002,Urban Style Group,Michael Chen,michael@urbanstyle.com,555-0102,11111111-1111-1111-1111-111111111111,
```

---

## Staff Members

```csv
Id,FirstName,LastName,Email,Phone,ClientId,UserId,Bio,SortOrder
cc000000-0000-0000-0000-000000000001,John,Barber,staff@demo-barber.com,555-0001,,bb000000-0000-0000-0000-000000000002,Demo barber for testing the staff experience,1
cc000000-0000-0000-0000-000000000002,James,Martinez,james@elitehair.com,555-0201,ff000000-0000-0000-0000-000000000001,ee000000-0000-0000-0000-000000000001,Master Barber with 10 years experience,2
cc000000-0000-0000-0000-000000000003,Emma,Wilson,emma@elitehair.com,555-0202,ff000000-0000-0000-0000-000000000001,ee000000-0000-0000-0000-000000000002,Specialist in modern cuts and styling,3
cc000000-0000-0000-0000-000000000004,Oliver,Brown,oliver@urbanstyle.com,555-0203,ff000000-0000-0000-0000-000000000002,ee000000-0000-0000-0000-000000000003,Expert in beard grooming and hot shaves,4
```

**Staff ↔ Services:**

| Staff | Services |
|-------|----------|
| John Barber | Mens Haircut, Beard Trim |
| James Martinez | Mens Haircut, Hair & Beard Combo, Beard Trim |
| Emma Wilson | Mens Haircut, Hair & Beard Combo |
| Oliver Brown | Beard Trim, Hot Towel Shave |

---

## Services

```csv
Id,Name,Price,DurationMinutes,CategoryId
44444444-4444-4444-4444-444444444401,Mens Haircut,25.00,30,33333333-3333-3333-3333-333333333301
44444444-4444-4444-4444-444444444402,Hair & Beard Combo,40.00,60,33333333-3333-3333-3333-333333333301
44444444-4444-4444-4444-444444444403,Beard Trim,15.00,20,33333333-3333-3333-3333-333333333302
44444444-4444-4444-4444-444444444404,Hot Towel Shave,35.00,45,33333333-3333-3333-3333-333333333302
```

---

## Customers

```csv
Id,FirstName,LastName,Email,Phone,City,MembershipNumber,TotalBookings,TotalSpent,Tags
dd000000-0000-0000-0000-000000000001,Alice,Thompson,alice.thompson@example.com,555-1001,London,MBR-001,5,125.00,regular
dd000000-0000-0000-0000-000000000002,Bob,Williams,bob.williams@example.com,555-1002,Manchester,MBR-002,3,75.00,
dd000000-0000-0000-0000-000000000003,Carol,Davies,carol.davies@example.com,555-1003,Birmingham,MBR-003,8,320.00,vip
dd000000-0000-0000-0000-000000000004,David,Harrison,david.harrison@example.com,555-1004,Leeds,,2,50.00,
dd000000-0000-0000-0000-000000000005,Eve,Jackson,eve.jackson@example.com,555-1005,Bristol,MBR-005,4,160.00,regular
```

---

## Appointments

```csv
Id,CustomerName,CustomerEmail,StaffId,Service,StartTime,EndTime,Status,TotalAmount,BookingPin,Notes
aa100000-0000-0000-0000-000000000001,Alice Thompson,alice.thompson@example.com,cc000000-0000-0000-0000-000000000001,Mens Haircut,2026-01-08 10:00,2026-01-08 10:30,Completed,25.00,A1B2C3,
aa100000-0000-0000-0000-000000000002,Bob Williams,bob.williams@example.com,cc000000-0000-0000-0000-000000000002,Hair & Beard Combo,2026-01-10 11:00,2026-01-10 12:00,Completed,40.00,D4E5F6,
aa100000-0000-0000-0000-000000000003,Carol Davies,carol.davies@example.com,cc000000-0000-0000-0000-000000000003,Mens Haircut,2026-01-12 09:00,2026-01-12 09:30,Completed,25.00,G7H8I9,
aa100000-0000-0000-0000-000000000004,David Harrison,david.harrison@example.com,cc000000-0000-0000-0000-000000000004,Beard Trim,2026-01-15 14:00,2026-01-15 14:20,Completed,15.00,J1K2L3,
aa100000-0000-0000-0000-000000000005,Eve Jackson,eve.jackson@example.com,cc000000-0000-0000-0000-000000000002,Hair & Beard Combo,2026-01-18 10:00,2026-01-18 11:00,Completed,40.00,M4N5O6,
aa100000-0000-0000-0000-000000000006,Alice Thompson,alice.thompson@example.com,cc000000-0000-0000-0000-000000000003,Hair & Beard Combo,2026-01-22 09:00,2026-01-22 10:00,Completed,40.00,P7Q8R9,
aa100000-0000-0000-0000-000000000007,Carol Davies,carol.davies@example.com,cc000000-0000-0000-0000-000000000004,Hot Towel Shave,2026-01-25 11:00,2026-01-25 11:45,Completed,35.00,S1T2U3,
aa100000-0000-0000-0000-000000000008,Bob Williams,bob.williams@example.com,cc000000-0000-0000-0000-000000000001,Mens Haircut,2026-01-28 12:00,2026-01-28 12:30,Completed,25.00,V4W5X6,
aa100000-0000-0000-0000-000000000009,Eve Jackson,eve.jackson@example.com,cc000000-0000-0000-0000-000000000001,Beard Trim,2026-01-29 14:00,2026-01-29 14:20,Completed,15.00,Y7Z8A1,
aa100000-0000-0000-0000-000000000010,David Harrison,david.harrison@example.com,cc000000-0000-0000-0000-000000000002,Mens Haircut,2026-01-31 15:00,2026-01-31 15:30,Cancelled,25.00,B2C3D4,Customer requested reschedule
aa100000-0000-0000-0000-000000000011,Alice Thompson,alice.thompson@example.com,cc000000-0000-0000-0000-000000000002,Hair & Beard Combo,2026-03-03 10:00,2026-03-03 11:00,Confirmed,40.00,E5F6G7,
aa100000-0000-0000-0000-000000000012,Carol Davies,carol.davies@example.com,cc000000-0000-0000-0000-000000000003,Mens Haircut,2026-03-05 09:00,2026-03-05 09:30,Confirmed,25.00,H8I9J1,
aa100000-0000-0000-0000-000000000013,Bob Williams,bob.williams@example.com,cc000000-0000-0000-0000-000000000004,Hot Towel Shave,2026-03-07 12:00,2026-03-07 12:45,Pending,35.00,K2L3M4,
aa100000-0000-0000-0000-000000000014,Eve Jackson,eve.jackson@example.com,cc000000-0000-0000-0000-000000000001,Mens Haircut,2026-03-10 10:00,2026-03-10 10:30,Confirmed,25.00,N5O6P7,
aa100000-0000-0000-0000-000000000015,David Harrison,david.harrison@example.com,cc000000-0000-0000-0000-000000000002,Mens Haircut,2026-03-14 15:00,2026-03-14 15:30,Confirmed,25.00,Q8R9S1,Please trim short on sides
```

**Status legend:** 0 = Pending · 2 = Confirmed · 3 = Completed · 4 = Cancelled

---

## Role Hierarchy

```
SuperAdmin  →  full platform access (all tenants)
TenantAdmin →  full access within their tenant
Manager     →  manage staff / services within tenant (TenantId hidden)
Staff       →  view own appointments, cancel own bookings with reason, view customers
Customer    →  book appointments, view own bookings
```

---

## Migrations

All seed data is defined in EF Core migrations and applied automatically when the API starts via `MigrateAsync()`:

| Migration | Description |
|-----------|-------------|
| `20260225082840_InitialCreate` | Schema + tenant, services, business hours |
| `20260225085632_IdentityAndClassSessions` | Roles, TenantAdmin + Staff + Customer seed users |
| `20260226145432_AddClientsAndStaffInvitations` | Clients + StaffInvitations tables |
| `20260227003030_AddManagerRoleAndSeedData` | Manager role + Manager seed user |
| `20260227053400_AddSuperAdminAndStaffSeedData` | SuperAdmin user + John Barber staff record |
| `20260227060000_SeedDemoTestData` | Client companies, 3 staff users/records, 5 customers, 15 appointments |
