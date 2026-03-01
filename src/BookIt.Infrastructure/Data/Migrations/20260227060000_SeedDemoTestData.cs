using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoTestData : Migration
    {
        // Tenant
        private static readonly Guid TenantId     = new("11111111-1111-1111-1111-111111111111");

        // Existing user IDs from earlier migrations
        private static readonly Guid UserManager  = new("bb000000-0000-0000-0000-000000000004"); // manager@demo-barber.example
        private static readonly Guid UserStaff    = new("bb000000-0000-0000-0000-000000000002"); // staff@demo-barber.example
        private static readonly Guid UserCustomer = new("bb000000-0000-0000-0000-000000000003"); // customer@bookit-demo.example

        // Existing staff record (linked to staff@demo-barber.example)
        private static readonly Guid StaffJohn    = new("cc000000-0000-0000-0000-000000000001"); // John Barber

        // New staff user accounts
        private static readonly Guid UserJames    = new("ee000000-0000-0000-0000-000000000001");
        private static readonly Guid UserEmma     = new("ee000000-0000-0000-0000-000000000002");
        private static readonly Guid UserOliver   = new("ee000000-0000-0000-0000-000000000003");

        // New staff records
        private static readonly Guid StaffJames   = new("cc000000-0000-0000-0000-000000000002");
        private static readonly Guid StaffEmma    = new("cc000000-0000-0000-0000-000000000003");
        private static readonly Guid StaffOliver  = new("cc000000-0000-0000-0000-000000000004");

        // Client records
        private static readonly Guid Client1      = new("ff000000-0000-0000-0000-000000000001"); // Elite Hair Solutions
        private static readonly Guid Client2      = new("ff000000-0000-0000-0000-000000000002"); // Urban Style Group

        // Client contact user accounts (role = Customer = 5)
        private static readonly Guid UserClient1  = new("ee000000-0000-0000-0000-000000000004");
        private static readonly Guid UserClient2  = new("ee000000-0000-0000-0000-000000000005");

        // Customer records
        private static readonly Guid Cust1 = new("dd000000-0000-0000-0000-000000000001");
        private static readonly Guid Cust2 = new("dd000000-0000-0000-0000-000000000002");
        private static readonly Guid Cust3 = new("dd000000-0000-0000-0000-000000000003");
        private static readonly Guid Cust4 = new("dd000000-0000-0000-0000-000000000004");
        private static readonly Guid Cust5 = new("dd000000-0000-0000-0000-000000000005");

        // Services seeded in InitialCreate
        private static readonly Guid SvcHaircut   = new("44444444-4444-4444-4444-444444444401");
        private static readonly Guid SvcCombo     = new("44444444-4444-4444-4444-444444444402");
        private static readonly Guid SvcBeard     = new("44444444-4444-4444-4444-444444444403");
        private static readonly Guid SvcHotShave  = new("44444444-4444-4444-4444-444444444404");

        // Appointment IDs
        private static readonly Guid Appt01 = new("aa100000-0000-0000-0000-000000000001");
        private static readonly Guid Appt02 = new("aa100000-0000-0000-0000-000000000002");
        private static readonly Guid Appt03 = new("aa100000-0000-0000-0000-000000000003");
        private static readonly Guid Appt04 = new("aa100000-0000-0000-0000-000000000004");
        private static readonly Guid Appt05 = new("aa100000-0000-0000-0000-000000000005");
        private static readonly Guid Appt06 = new("aa100000-0000-0000-0000-000000000006");
        private static readonly Guid Appt07 = new("aa100000-0000-0000-0000-000000000007");
        private static readonly Guid Appt08 = new("aa100000-0000-0000-0000-000000000008");
        private static readonly Guid Appt09 = new("aa100000-0000-0000-0000-000000000009");
        private static readonly Guid Appt10 = new("aa100000-0000-0000-0000-000000000010");
        private static readonly Guid Appt11 = new("aa100000-0000-0000-0000-000000000011");
        private static readonly Guid Appt12 = new("aa100000-0000-0000-0000-000000000012");
        private static readonly Guid Appt13 = new("aa100000-0000-0000-0000-000000000013");
        private static readonly Guid Appt14 = new("aa100000-0000-0000-0000-000000000014");
        private static readonly Guid Appt15 = new("aa100000-0000-0000-0000-000000000015");

        // Staff role ID
        private static readonly Guid RoleStaff    = new("aa000000-0000-0000-0000-000000000003");
        private static readonly Guid RoleCustomer = new("aa000000-0000-0000-0000-000000000004");

        // Seed date anchor (fixed so migration is idempotent across time)
        private static readonly DateTime Anchor = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Client contact user accounts (role = Customer in Users table) ──────
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "Role", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { UserClient1, 0, "client1-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "sarah@elitehair.example", true, "Sarah", false, "Johnson", false, null, null, "SARAH@ELITEHAIR.EXAMPLE", "SARAH@ELITEHAIR.EXAMPLE", "AQAAAAIAAYagAAAAEEg7zuYqljGMg56j3zUnQcWfW3B1ES52Rac1If1LF5QpglTD9iCwA2fjdi++mqLUNQ==", "555-0101", false, null, null, 5, "client1-security-stamp", TenantId, false, null, "sarah@elitehair.example" },
                    { UserClient2, 0, "client2-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "michael@urbanstyle.example", true, "Michael", false, "Chen", false, null, null, "MICHAEL@URBANSTYLE.EXAMPLE", "MICHAEL@URBANSTYLE.EXAMPLE", "AQAAAAIAAYagAAAAEEg7zuYqljGMg56j3zUnQcWfW3B1ES52Rac1If1LF5QpglTD9iCwA2fjdi++mqLUNQ==", "555-0102", false, null, null, 5, "client2-security-stamp", TenantId, false, null, "michael@urbanstyle.example" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { RoleCustomer, UserClient1 },
                    { RoleCustomer, UserClient2 }
                });

            // ── Client companies ─────────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "CompanyName", "ContactName", "CreatedAt", "Email", "IsActive", "IsDeleted", "Notes", "Phone", "TenantId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { Client1, null, "Elite Hair Solutions", "Sarah Johnson", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "sarah@elitehair.example", true, false, "VIP client — priority scheduling", "555-0101", TenantId, null, UserClient1 },
                    { Client2, null, "Urban Style Group", "Michael Chen", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "michael@urbanstyle.example", true, false, null, "555-0102", TenantId, null, UserClient2 }
                });

            // ── Staff user accounts ──────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "Role", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { UserJames,  0, "james-concurrency-stamp",  new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "james@elitehair.example",    true, "James",  false, "Martinez", false, null, null, "JAMES@ELITEHAIR.EXAMPLE",    "JAMES@ELITEHAIR.EXAMPLE",    "AQAAAAIAAYagAAAAEJG449ssHHixsjQHRmtQ77ObBnLZBt3s4t54hGgYCLV7/EuqQsTEMNl8VueuB7hknw==", "555-0201", false, null, null, 4, "james-security-stamp",  TenantId, false, null, "james@elitehair.example" },
                    { UserEmma,   0, "emma-concurrency-stamp",   new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "emma@elitehair.example",     true, "Emma",   false, "Wilson",   false, null, null, "EMMA@ELITEHAIR.EXAMPLE",     "EMMA@ELITEHAIR.EXAMPLE",     "AQAAAAIAAYagAAAAEJG449ssHHixsjQHRmtQ77ObBnLZBt3s4t54hGgYCLV7/EuqQsTEMNl8VueuB7hknw==", "555-0202", false, null, null, 4, "emma-security-stamp",   TenantId, false, null, "emma@elitehair.example" },
                    { UserOliver, 0, "oliver-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "oliver@urbanstyle.example",  true, "Oliver", false, "Brown",    false, null, null, "OLIVER@URBANSTYLE.EXAMPLE",  "OLIVER@URBANSTYLE.EXAMPLE",  "AQAAAAIAAYagAAAAEJG449ssHHixsjQHRmtQ77ObBnLZBt3s4t54hGgYCLV7/EuqQsTEMNl8VueuB7hknw==", "555-0203", false, null, null, 4, "oliver-security-stamp", TenantId, false, null, "oliver@urbanstyle.example" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { RoleStaff, UserJames  },
                    { RoleStaff, UserEmma   },
                    { RoleStaff, UserOliver }
                });

            // ── Staff records ─────────────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "Bio", "ClientId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "Email", "FirstName", "IsActive", "IsDeleted", "LastName", "Phone", "PhotoUrl", "SortOrder", "TenantId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { StaffJames,  "Master Barber with 10 years experience", Client1, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "james@elitehair.example",   "James",  true, false, "Martinez", "555-0201", null, 2, TenantId, null, null, UserJames  },
                    { StaffEmma,   "Specialist in modern cuts and styling",   Client1, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "emma@elitehair.example",    "Emma",   true, false, "Wilson",   "555-0202", null, 3, TenantId, null, null, UserEmma   },
                    { StaffOliver, "Expert in beard grooming and hot shaves", Client2, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "oliver@urbanstyle.example", "Oliver", true, false, "Brown",    "555-0203", null, 4, TenantId, null, null, UserOliver }
                });

            // ── Staff ↔ Services ──────────────────────────────────────────────────
            // John (StaffJohn) — Haircut, Beard Trim
            migrationBuilder.InsertData(
                table: "StaffServices",
                columns: new[] { "StaffId", "ServiceId" },
                values: new object[,]
                {
                    { StaffJohn,   SvcHaircut  },
                    { StaffJohn,   SvcBeard    },
                    { StaffJames,  SvcHaircut  },
                    { StaffJames,  SvcCombo    },
                    { StaffJames,  SvcBeard    },
                    { StaffEmma,   SvcHaircut  },
                    { StaffEmma,   SvcCombo    },
                    { StaffOliver, SvcBeard    },
                    { StaffOliver, SvcHotShave }
                });

            // ── Customer records ──────────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "City", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "Email", "FirstName", "Gender", "IsDeleted", "LastName", "LastVisit", "MarketingOptIn", "MembershipNumber", "Mobile", "Notes", "Phone", "PostCode", "SmsOptIn", "Tags", "TenantId", "TotalBookings", "TotalSpent", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { Cust1, null, "London",     "UK", new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "alice.thompson@example.com", "Alice",  "Female", false, "Thompson", new DateTime(2026, 1, 20, 10, 0, 0, DateTimeKind.Utc), true,  "MBR-001", null, null, "555-1001", null, false, "regular",   TenantId, 5, 125.00m, null, null },
                    { Cust2, null, "Manchester", "UK", new DateTime(2025, 7, 15, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "bob.williams@example.com",   "Bob",    "Male",   false, "Williams", new DateTime(2026, 1, 28, 11, 0, 0, DateTimeKind.Utc), false, "MBR-002", null, null, "555-1002", null, false, null,        TenantId, 3,  75.00m, null, null },
                    { Cust3, null, "Birmingham", "UK", new DateTime(2025, 8, 10, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "carol.davies@example.com",   "Carol",  "Female", false, "Davies",   new DateTime(2026, 2, 5,  9, 0, 0, DateTimeKind.Utc),  true,  "MBR-003", null, null, "555-1003", null, true,  "vip",       TenantId, 8, 320.00m, null, null },
                    { Cust4, null, "Leeds",      "UK", new DateTime(2025, 9, 5, 0, 0, 0, DateTimeKind.Utc),  null, null, null, null, null, "david.harrison@example.com", "David",  "Male",   false, "Harrison", new DateTime(2026, 1, 15, 14, 0, 0, DateTimeKind.Utc), true,  null,      null, null, "555-1004", null, false, null,        TenantId, 2,  50.00m, null, null },
                    { Cust5, null, "Bristol",    "UK", new DateTime(2025, 10, 20, 0, 0, 0, DateTimeKind.Utc),null, null, null, null, null, "eve.jackson@example.com",    "Eve",    "Female", false, "Jackson",  new DateTime(2026, 2, 10, 13, 0, 0, DateTimeKind.Utc), false, "MBR-005", null, "Regular client — prefers Emma", "555-1005", null, false, "regular",   TenantId, 4, 160.00m, null, null }
                });

            // ── Appointments (past — Completed) ──────────────────────────────────
            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "BookingPin", "CancellationReason", "ConfirmationToken", "CreatedAt", "CustomFormDataJson", "CustomerEmail", "CustomerId", "CustomerName", "CustomerNotes", "CustomerPhone", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "EndTime", "InternalNotes", "IsDeleted", "MeetingId", "MeetingInstructions", "MeetingLink", "MeetingPassword", "MeetingType", "PaymentProvider", "PaymentReference", "PaymentStatus", "ReminderSent", "StaffId", "StartTime", "Status", "TenantId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    // Appt01: Alice → John, Haircut, 7 Jan
                    { Appt01, "A1B2C3", null, "tok-appt01", Anchor.AddDays(-31), null, "alice.thompson@example.com", Cust1, "Alice Thompson", null, "555-1001", null, null, null, null, Anchor.AddDays(-24).AddHours(10).AddMinutes(30), null, false, null, null, null, null, 1/*InPerson*/, null, null, 1/*Paid*/, true,  StaffJohn,  Anchor.AddDays(-24).AddHours(10), 3/*Completed*/, TenantId, 25.00m, null, null },
                    // Appt02: Bob → James, Combo, 10 Jan
                    { Appt02, "D4E5F6", null, "tok-appt02", Anchor.AddDays(-30), null, "bob.williams@example.com",   Cust2, "Bob Williams",   null, "555-1002", null, null, null, null, Anchor.AddDays(-22).AddHours(12), null, false, null, null, null, null, 1, null, null, 1, true,  StaffJames, Anchor.AddDays(-22).AddHours(11), 3, TenantId, 40.00m, null, null },
                    // Appt03: Carol → Emma, Haircut, 12 Jan
                    { Appt03, "G7H8I9", null, "tok-appt03", Anchor.AddDays(-29), null, "carol.davies@example.com",   Cust3, "Carol Davies",   null, "555-1003", null, null, null, null, Anchor.AddDays(-20).AddHours(9).AddMinutes(30),  null, false, null, null, null, null, 1, null, null, 1, true,  StaffEmma,  Anchor.AddDays(-20).AddHours(9),  3, TenantId, 25.00m, null, null },
                    // Appt04: David → Oliver, Beard, 15 Jan
                    { Appt04, "J1K2L3", null, "tok-appt04", Anchor.AddDays(-28), null, "david.harrison@example.com", Cust4, "David Harrison", null, "555-1004", null, null, null, null, Anchor.AddDays(-17).AddHours(14).AddMinutes(20), null, false, null, null, null, null, 1, null, null, 1, true,  StaffOliver,Anchor.AddDays(-17).AddHours(14), 3, TenantId, 15.00m, null, null },
                    // Appt05: Eve → James, Combo, 18 Jan
                    { Appt05, "M4N5O6", null, "tok-appt05", Anchor.AddDays(-27), null, "eve.jackson@example.com",    Cust5, "Eve Jackson",    null, "555-1005", null, null, null, null, Anchor.AddDays(-14).AddHours(11), null, false, null, null, null, null, 1, null, null, 1, true,  StaffJames, Anchor.AddDays(-14).AddHours(10), 3, TenantId, 40.00m, null, null },
                    // Appt06: Alice → Emma, Combo, 22 Jan
                    { Appt06, "P7Q8R9", null, "tok-appt06", Anchor.AddDays(-25), null, "alice.thompson@example.com", Cust1, "Alice Thompson", null, "555-1001", null, null, null, null, Anchor.AddDays(-10).AddHours(10), null, false, null, null, null, null, 1, null, null, 1, true,  StaffEmma,  Anchor.AddDays(-10).AddHours(9),  3, TenantId, 40.00m, null, null },
                    // Appt07: Carol → Oliver, HotShave, 25 Jan
                    { Appt07, "S1T2U3", null, "tok-appt07", Anchor.AddDays(-22), null, "carol.davies@example.com",   Cust3, "Carol Davies",   null, "555-1003", null, null, null, null, Anchor.AddDays(-7).AddHours(11).AddMinutes(45),  null, false, null, null, null, null, 1, null, null, 1, true,  StaffOliver,Anchor.AddDays(-7).AddHours(11),  3, TenantId, 35.00m, null, null },
                    // Appt08: Bob → John, Haircut, 28 Jan
                    { Appt08, "V4W5X6", null, "tok-appt08", Anchor.AddDays(-20), null, "bob.williams@example.com",   Cust2, "Bob Williams",   null, "555-1002", null, null, null, null, Anchor.AddDays(-4).AddHours(12).AddMinutes(30),  null, false, null, null, null, null, 1, null, null, 1, true,  StaffJohn,  Anchor.AddDays(-4).AddHours(12),  3, TenantId, 25.00m, null, null },
                    // Appt09: Eve → John, Beard, 29 Jan
                    { Appt09, "Y7Z8A1", null, "tok-appt09", Anchor.AddDays(-19), null, "eve.jackson@example.com",    Cust5, "Eve Jackson",    null, "555-1005", null, null, null, null, Anchor.AddDays(-3).AddHours(14).AddMinutes(20),  null, false, null, null, null, null, 1, null, null, 1, true,  StaffJohn,  Anchor.AddDays(-3).AddHours(14),  3, TenantId, 15.00m, null, null },
                    // Appt10: David → James, Haircut, 31 Jan — cancelled with reason
                    { Appt10, "B2C3D4", "Customer requested reschedule", "tok-appt10", Anchor.AddDays(-18), null, "david.harrison@example.com", Cust4, "David Harrison", null, "555-1004", null, null, null, null, Anchor.AddDays(-1).AddHours(15).AddMinutes(30), null, false, null, null, null, null, 1, null, null, 0/*Unpaid*/, false, StaffJames, Anchor.AddDays(-1).AddHours(15), 4/*Cancelled*/, TenantId, 25.00m, null, null },
                });

            // ── Appointments (upcoming — Confirmed / Pending) ─────────────────────
            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "BookingPin", "CancellationReason", "ConfirmationToken", "CreatedAt", "CustomFormDataJson", "CustomerEmail", "CustomerId", "CustomerName", "CustomerNotes", "CustomerPhone", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "EndTime", "InternalNotes", "IsDeleted", "MeetingId", "MeetingInstructions", "MeetingLink", "MeetingPassword", "MeetingType", "PaymentProvider", "PaymentReference", "PaymentStatus", "ReminderSent", "StaffId", "StartTime", "Status", "TenantId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    // Appt11: Alice → James, Combo, 3 Feb future
                    { Appt11, "E5F6G7", null, "tok-appt11", Anchor.AddDays(-5),  null, "alice.thompson@example.com", Cust1, "Alice Thompson", null, "555-1001", null, null, null, null, Anchor.AddDays(3).AddHours(11),  null, false, null, null, null, null, 1, null, null, 0, false, StaffJames, Anchor.AddDays(3).AddHours(10),  2/*Confirmed*/, TenantId, 40.00m, null, null },
                    // Appt12: Carol → Emma, Haircut, 5 Feb
                    { Appt12, "H8I9J1", null, "tok-appt12", Anchor.AddDays(-4),  null, "carol.davies@example.com",   Cust3, "Carol Davies",   null, "555-1003", null, null, null, null, Anchor.AddDays(5).AddHours(9).AddMinutes(30), null, false, null, null, null, null, 1, null, null, 0, false, StaffEmma,  Anchor.AddDays(5).AddHours(9),   2, TenantId, 25.00m, null, null },
                    // Appt13: Bob → Oliver, HotShave, 7 Feb
                    { Appt13, "K2L3M4", null, "tok-appt13", Anchor.AddDays(-3),  null, "bob.williams@example.com",   Cust2, "Bob Williams",   null, "555-1002", null, null, null, null, Anchor.AddDays(7).AddHours(12).AddMinutes(45), null, false, null, null, null, null, 1, null, null, 0, false, StaffOliver,Anchor.AddDays(7).AddHours(12),  0/*Pending*/,   TenantId, 35.00m, null, null },
                    // Appt14: Eve → John, Haircut, 10 Feb
                    { Appt14, "N5O6P7", null, "tok-appt14", Anchor.AddDays(-2),  null, "eve.jackson@example.com",    Cust5, "Eve Jackson",    null, "555-1005", null, null, null, null, Anchor.AddDays(10).AddHours(10).AddMinutes(30), null, false, null, null, null, null, 1, null, null, 0, false, StaffJohn,  Anchor.AddDays(10).AddHours(10), 2, TenantId, 25.00m, null, null },
                    // Appt15: David → James, Haircut, 14 Feb
                    { Appt15, "Q8R9S1", null, "tok-appt15", Anchor.AddDays(-1),  null, "david.harrison@example.com", Cust4, "David Harrison", "Please trim short on sides", "555-1004", null, null, null, null, Anchor.AddDays(14).AddHours(15).AddMinutes(30), null, false, null, null, null, null, 1, null, null, 0, false, StaffJames, Anchor.AddDays(14).AddHours(15), 2, TenantId, 25.00m, null, null }
                });

            // ── AppointmentServices ───────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "AppointmentServices",
                columns: new[] { "AppointmentId", "ServiceId", "DurationAtBooking", "PriceAtBooking" },
                values: new object[,]
                {
                    { Appt01, SvcHaircut,  30, 25.00m },
                    { Appt02, SvcCombo,    60, 40.00m },
                    { Appt03, SvcHaircut,  30, 25.00m },
                    { Appt04, SvcBeard,    20, 15.00m },
                    { Appt05, SvcCombo,    60, 40.00m },
                    { Appt06, SvcCombo,    60, 40.00m },
                    { Appt07, SvcHotShave, 45, 35.00m },
                    { Appt08, SvcHaircut,  30, 25.00m },
                    { Appt09, SvcBeard,    20, 15.00m },
                    { Appt10, SvcHaircut,  30, 25.00m },
                    { Appt11, SvcCombo,    60, 40.00m },
                    { Appt12, SvcHaircut,  30, 25.00m },
                    { Appt13, SvcHotShave, 45, 35.00m },
                    { Appt14, SvcHaircut,  30, 25.00m },
                    { Appt15, SvcHaircut,  30, 25.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt01, SvcHaircut  });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt02, SvcCombo    });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt03, SvcHaircut  });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt04, SvcBeard    });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt05, SvcCombo    });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt06, SvcCombo    });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt07, SvcHotShave });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt08, SvcHaircut  });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt09, SvcBeard    });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt10, SvcHaircut  });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt11, SvcCombo    });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt12, SvcHaircut  });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt13, SvcHotShave });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt14, SvcHaircut  });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { Appt15, SvcHaircut  });

            foreach (var id in new[] { Appt01, Appt02, Appt03, Appt04, Appt05, Appt06, Appt07, Appt08, Appt09, Appt10, Appt11, Appt12, Appt13, Appt14, Appt15 })
                migrationBuilder.DeleteData("Appointments", "Id", id);

            foreach (var id in new[] { Cust1, Cust2, Cust3, Cust4, Cust5 })
                migrationBuilder.DeleteData("Customers", "Id", id);

            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffJohn,   SvcHaircut  });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffJohn,   SvcBeard    });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffJames,  SvcHaircut  });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffJames,  SvcCombo    });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffJames,  SvcBeard    });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffEmma,   SvcHaircut  });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffEmma,   SvcCombo    });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffOliver, SvcBeard    });
            migrationBuilder.DeleteData("StaffServices", new[] { "StaffId", "ServiceId" }, new object[] { StaffOliver, SvcHotShave });

            foreach (var id in new[] { StaffJames, StaffEmma, StaffOliver })
                migrationBuilder.DeleteData("Staff", "Id", id);

            migrationBuilder.DeleteData("UserRoles", new[] { "RoleId", "UserId" }, new object[] { RoleStaff, UserJames  });
            migrationBuilder.DeleteData("UserRoles", new[] { "RoleId", "UserId" }, new object[] { RoleStaff, UserEmma   });
            migrationBuilder.DeleteData("UserRoles", new[] { "RoleId", "UserId" }, new object[] { RoleStaff, UserOliver });
            migrationBuilder.DeleteData("UserRoles", new[] { "RoleId", "UserId" }, new object[] { RoleCustomer, UserClient1 });
            migrationBuilder.DeleteData("UserRoles", new[] { "RoleId", "UserId" }, new object[] { RoleCustomer, UserClient2 });

            foreach (var id in new[] { Client1, Client2 })
                migrationBuilder.DeleteData("Clients", "Id", id);

            foreach (var id in new[] { UserJames, UserEmma, UserOliver, UserClient1, UserClient2 })
                migrationBuilder.DeleteData("Users", "Id", id);
        }
    }
}
