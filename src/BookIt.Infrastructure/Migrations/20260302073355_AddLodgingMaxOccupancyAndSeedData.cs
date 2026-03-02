using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLodgingMaxOccupancyAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxOccupancy",
                table: "LodgingProperties",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAP+wUCa1C4tJXJh7Sv+og8QATA5celliz2fqWWhDeylSNbx6ExezSWSq4nsIN8lmg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDEdJZLsLRMjssY6AZGnepOYS8hdXjjrkuro/vldB90/nY6kSznHWyb5kczL2njWqg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGD+4qqB7qGSndCSVf84ZFpQFLd5avkCv3uqfM3u+czJnyV1CIa+ASn8lx9HiyB3FA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAB/WvzrdD5+wfoUHzMM7pgnDBHtX6GD2D7oLzvvgHVEUQkSQeIWzHfBc6NqisQNkQ==");

            // ── Seed missing users ──
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "Role", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("bb000000-0000-0000-0000-000000000005"), 0, "superadmin-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "superadmin@bookit.app", true, "Super", false, "Admin", false, null, null, "SUPERADMIN@BOOKIT.APP", "SUPERADMIN@BOOKIT.APP", "AQAAAAIAAYagAAAAEJOjLprSu2+QZPN3abPJxvuBmKuo9oADcoDGGh6pc2xDMmE24uBbdF632SrgJkp3BA==", null, false, null, null, 1, "superadmin-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "superadmin@bookit.app" },
                    { new Guid("ee000000-0000-0000-0000-000000000001"), 0, "james-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "james@elitehair.com", true, "James", false, "Martinez", false, null, null, "JAMES@ELITEHAIR.COM", "JAMES@ELITEHAIR.COM", "AQAAAAIAAYagAAAAEDEdJZLsLRMjssY6AZGnepOYS8hdXjjrkuro/vldB90/nY6kSznHWyb5kczL2njWqg==", null, false, null, null, 4, "james-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "james@elitehair.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000002"), 0, "emma-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "emma@elitehair.com", true, "Emma", false, "Wilson", false, null, null, "EMMA@ELITEHAIR.COM", "EMMA@ELITEHAIR.COM", "AQAAAAIAAYagAAAAEDEdJZLsLRMjssY6AZGnepOYS8hdXjjrkuro/vldB90/nY6kSznHWyb5kczL2njWqg==", null, false, null, null, 4, "emma-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "emma@elitehair.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000003"), 0, "oliver-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "oliver@urbanstyle.com", true, "Oliver", false, "Brown", false, null, null, "OLIVER@URBANSTYLE.COM", "OLIVER@URBANSTYLE.COM", "AQAAAAIAAYagAAAAEDEdJZLsLRMjssY6AZGnepOYS8hdXjjrkuro/vldB90/nY6kSznHWyb5kczL2njWqg==", null, false, null, null, 4, "oliver-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "oliver@urbanstyle.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000004"), 0, "sarah-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sarah@elitehair.com", true, "Sarah", false, "Johnson", false, null, null, "SARAH@ELITEHAIR.COM", "SARAH@ELITEHAIR.COM", "AQAAAAIAAYagAAAAEOUzw2K8jAXSTvImxitEWvwweqh3OFCeRdk3cFgtb2PVc3DCG6Sds2EaqBnc02ju7A==", null, false, null, null, 5, "sarah-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "sarah@elitehair.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000005"), 0, "michael-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "michael@urbanstyle.com", true, "Michael", false, "Chen", false, null, null, "MICHAEL@URBANSTYLE.COM", "MICHAEL@URBANSTYLE.COM", "AQAAAAIAAYagAAAAEOUzw2K8jAXSTvImxitEWvwweqh3OFCeRdk3cFgtb2PVc3DCG6Sds2EaqBnc02ju7A==", null, false, null, null, 5, "michael-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "michael@urbanstyle.com" }
                });

            // ── Seed role assignments ──
            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aa000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000005") },
                    { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000001") },
                    { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000002") },
                    { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000003") },
                    { new Guid("aa000000-0000-0000-0000-000000000004"), new Guid("ee000000-0000-0000-0000-000000000004") },
                    { new Guid("aa000000-0000-0000-0000-000000000004"), new Guid("ee000000-0000-0000-0000-000000000005") }
                });

            // ── Seed client companies ──
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "TenantId", "UserId", "CompanyName", "ContactName", "Email", "Phone", "Address", "Notes", "IsActive", "IsDeleted", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy", "EditedAt", "EditedBy", "DeletedAt", "DeletedBy" },
                values: new object[,]
                {
                    { new Guid("ff000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("ee000000-0000-0000-0000-000000000004"), "Elite Hair Solutions", "Sarah Johnson", "sarah@elitehair.com", "555-0101", null, "VIP client - priority scheduling", true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("ff000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("ee000000-0000-0000-0000-000000000005"), "Urban Style Group", "Michael Chen", "michael@urbanstyle.com", "555-0102", null, null, true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null }
                });

            // ── Seed staff records ──
            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "TenantId", "UserId", "ClientId", "FirstName", "LastName", "Email", "Phone", "PhotoUrl", "Bio", "IsActive", "SortOrder", "IsDeleted", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy", "EditedAt", "EditedBy", "DeletedAt", "DeletedBy" },
                values: new object[,]
                {
                    { new Guid("cc000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("bb000000-0000-0000-0000-000000000002"), null, "John", "Barber", "staff@demo-barber.com", "555-0001", null, "Demo barber for testing the staff experience", true, 1, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("cc000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("ee000000-0000-0000-0000-000000000001"), new Guid("ff000000-0000-0000-0000-000000000001"), "James", "Martinez", "james@elitehair.com", "555-0201", null, "Master Barber with 10 years experience", true, 2, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("cc000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("ee000000-0000-0000-0000-000000000002"), new Guid("ff000000-0000-0000-0000-000000000001"), "Emma", "Wilson", "emma@elitehair.com", "555-0202", null, "Specialist in modern cuts and styling", true, 3, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("cc000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("ee000000-0000-0000-0000-000000000003"), new Guid("ff000000-0000-0000-0000-000000000002"), "Oliver", "Brown", "oliver@urbanstyle.com", "555-0203", null, "Expert in beard grooming and hot shaves", true, 4, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null }
                });

            // ── Seed staff services ──
            migrationBuilder.InsertData(
                table: "StaffServices",
                columns: new[] { "StaffId", "ServiceId" },
                values: new object[,]
                {
                    { new Guid("cc000000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444401") },
                    { new Guid("cc000000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444403") },
                    { new Guid("cc000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444401") },
                    { new Guid("cc000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444402") },
                    { new Guid("cc000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444403") },
                    { new Guid("cc000000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444401") },
                    { new Guid("cc000000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444402") },
                    { new Guid("cc000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444403") },
                    { new Guid("cc000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444404") }
                });

            // ── Seed customers ──
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "TenantId", "FirstName", "LastName", "Email", "Phone", "Mobile", "Address", "City", "PostCode", "Country", "DateOfBirth", "Gender", "Notes", "Tags", "MembershipNumber", "MarketingOptIn", "SmsOptIn", "TotalBookings", "TotalSpent", "LastVisit", "UserId", "IsDeleted", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy", "EditedAt", "EditedBy", "DeletedAt", "DeletedBy" },
                values: new object[,]
                {
                    { new Guid("dd000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111"), "Alice", "Thompson", "alice.thompson@example.com", "555-1001", null, null, "London", null, null, null, null, null, "regular", "MBR-001", false, false, 5, 125.00m, null, null, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111"), "Bob", "Williams", "bob.williams@example.com", "555-1002", null, null, "Manchester", null, null, null, null, null, null, "MBR-002", false, false, 3, 75.00m, null, null, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111"), "Carol", "Davies", "carol.davies@example.com", "555-1003", null, null, "Birmingham", null, null, null, null, null, "vip", "MBR-003", false, false, 8, 320.00m, null, null, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111"), "David", "Harrison", "david.harrison@example.com", "555-1004", null, null, "Leeds", null, null, null, null, null, null, null, false, false, 2, 50.00m, null, null, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000005"), new Guid("11111111-1111-1111-1111-111111111111"), "Eve", "Jackson", "eve.jackson@example.com", "555-1005", null, null, "Bristol", null, null, null, null, null, "regular", "MBR-005", false, false, 4, 160.00m, null, null, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null }
                });

            // ── Seed appointments (15) ──
            // Status: Pending=1, Confirmed=2, Cancelled=3, Completed=4
            // PaymentStatus: Unpaid=1, Paid=3  MeetingType: InPerson=1
            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "TenantId", "StaffId", "CustomerId", "CustomerName", "CustomerEmail", "CustomerPhone", "CustomerNotes", "StartTime", "EndTime", "Status", "CancellationReason", "InternalNotes", "PaymentStatus", "TotalAmount", "PaymentReference", "PaymentProvider", "MeetingType", "MeetingLink", "MeetingId", "MeetingPassword", "MeetingInstructions", "CustomFormDataJson", "ConfirmationToken", "ReminderSent", "BookingPin", "IsDeleted", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy", "EditedAt", "EditedBy", "DeletedAt", "DeletedBy" },
                values: new object[,]
                {
                    { new Guid("aa100000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000001"), null, "Alice Thompson", "alice.thompson@example.com", null, null, new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 8, 10, 30, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 25.00m, null, null, 1, null, null, null, null, null, null, false, "A1B2C3", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000002"), null, "Bob Williams", "bob.williams@example.com", null, null, new DateTime(2026, 1, 10, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 40.00m, null, null, 1, null, null, null, null, null, null, false, "D4E5F6", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000003"), null, "Carol Davies", "carol.davies@example.com", null, null, new DateTime(2026, 1, 12, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 12, 9, 30, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 25.00m, null, null, 1, null, null, null, null, null, null, false, "G7H8I9", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000004"), null, "David Harrison", "david.harrison@example.com", null, null, new DateTime(2026, 1, 15, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 14, 20, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 15.00m, null, null, 1, null, null, null, null, null, null, false, "J1K2L3", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000005"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000002"), null, "Eve Jackson", "eve.jackson@example.com", null, null, new DateTime(2026, 1, 18, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 18, 11, 0, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 40.00m, null, null, 1, null, null, null, null, null, null, false, "M4N5O6", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000006"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000003"), null, "Alice Thompson", "alice.thompson@example.com", null, null, new DateTime(2026, 1, 22, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 22, 10, 0, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 40.00m, null, null, 1, null, null, null, null, null, null, false, "P7Q8R9", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000007"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000004"), null, "Carol Davies", "carol.davies@example.com", null, null, new DateTime(2026, 1, 25, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 25, 11, 45, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 35.00m, null, null, 1, null, null, null, null, null, null, false, "S1T2U3", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000008"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000001"), null, "Bob Williams", "bob.williams@example.com", null, null, new DateTime(2026, 1, 28, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 28, 12, 30, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 25.00m, null, null, 1, null, null, null, null, null, null, false, "V4W5X6", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000009"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000001"), null, "Eve Jackson", "eve.jackson@example.com", null, null, new DateTime(2026, 1, 29, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 29, 14, 20, 0, 0, DateTimeKind.Utc), 4, null, null, 3, 15.00m, null, null, 1, null, null, null, null, null, null, false, "Y7Z8A1", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000010"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000002"), null, "David Harrison", "david.harrison@example.com", null, null, new DateTime(2026, 1, 31, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 31, 15, 30, 0, 0, DateTimeKind.Utc), 3, "Customer requested reschedule", null, 1, 25.00m, null, null, 1, null, null, null, null, null, null, false, "B2C3D4", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000011"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000002"), null, "Alice Thompson", "alice.thompson@example.com", null, null, new DateTime(2026, 3, 3, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 3, 11, 0, 0, 0, DateTimeKind.Utc), 2, null, null, 1, 40.00m, null, null, 1, null, null, null, null, null, null, false, "E5F6G7", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000012"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000003"), null, "Carol Davies", "carol.davies@example.com", null, null, new DateTime(2026, 3, 5, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 5, 9, 30, 0, 0, DateTimeKind.Utc), 2, null, null, 1, 25.00m, null, null, 1, null, null, null, null, null, null, false, "H8I9J1", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000013"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000004"), null, "Bob Williams", "bob.williams@example.com", null, null, new DateTime(2026, 3, 7, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 7, 12, 45, 0, 0, DateTimeKind.Utc), 1, null, null, 1, 35.00m, null, null, 1, null, null, null, null, null, null, false, "K2L3M4", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000014"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000001"), null, "Eve Jackson", "eve.jackson@example.com", null, null, new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 10, 30, 0, 0, DateTimeKind.Utc), 2, null, null, 1, 25.00m, null, null, 1, null, null, null, null, null, null, false, "N5O6P7", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000015"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("cc000000-0000-0000-0000-000000000002"), null, "David Harrison", "david.harrison@example.com", null, null, new DateTime(2026, 3, 14, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc), 2, null, "Please trim short on sides", 1, 25.00m, null, null, 1, null, null, null, null, null, null, false, "Q8R9S1", false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, null }
                });

            // ── Seed appointment-service links ──
            migrationBuilder.InsertData(
                table: "AppointmentServices",
                columns: new[] { "AppointmentId", "ServiceId", "PriceAtBooking", "DurationAtBooking" },
                values: new object[,]
                {
                    { new Guid("aa100000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444401"), 25.00m, 30 },
                    { new Guid("aa100000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444402"), 40.00m, 60 },
                    { new Guid("aa100000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444401"), 25.00m, 30 },
                    { new Guid("aa100000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444403"), 15.00m, 20 },
                    { new Guid("aa100000-0000-0000-0000-000000000005"), new Guid("44444444-4444-4444-4444-444444444402"), 40.00m, 60 },
                    { new Guid("aa100000-0000-0000-0000-000000000006"), new Guid("44444444-4444-4444-4444-444444444402"), 40.00m, 60 },
                    { new Guid("aa100000-0000-0000-0000-000000000007"), new Guid("44444444-4444-4444-4444-444444444404"), 35.00m, 45 },
                    { new Guid("aa100000-0000-0000-0000-000000000008"), new Guid("44444444-4444-4444-4444-444444444401"), 25.00m, 30 },
                    { new Guid("aa100000-0000-0000-0000-000000000009"), new Guid("44444444-4444-4444-4444-444444444403"), 15.00m, 20 },
                    { new Guid("aa100000-0000-0000-0000-000000000010"), new Guid("44444444-4444-4444-4444-444444444401"), 25.00m, 30 },
                    { new Guid("aa100000-0000-0000-0000-000000000011"), new Guid("44444444-4444-4444-4444-444444444402"), 40.00m, 60 },
                    { new Guid("aa100000-0000-0000-0000-000000000012"), new Guid("44444444-4444-4444-4444-444444444401"), 25.00m, 30 },
                    { new Guid("aa100000-0000-0000-0000-000000000013"), new Guid("44444444-4444-4444-4444-444444444404"), 35.00m, 45 },
                    { new Guid("aa100000-0000-0000-0000-000000000014"), new Guid("44444444-4444-4444-4444-444444444401"), 25.00m, 30 },
                    { new Guid("aa100000-0000-0000-0000-000000000015"), new Guid("44444444-4444-4444-4444-444444444401"), 25.00m, 30 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove appointment-service links
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444402") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444403") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000005"), new Guid("44444444-4444-4444-4444-444444444402") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000006"), new Guid("44444444-4444-4444-4444-444444444402") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000007"), new Guid("44444444-4444-4444-4444-444444444404") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000008"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000009"), new Guid("44444444-4444-4444-4444-444444444403") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000010"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000011"), new Guid("44444444-4444-4444-4444-444444444402") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000012"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000013"), new Guid("44444444-4444-4444-4444-444444444404") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000014"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData("AppointmentServices", new[] { "AppointmentId", "ServiceId" }, new object[] { new Guid("aa100000-0000-0000-0000-000000000015"), new Guid("44444444-4444-4444-4444-444444444401") });

            // Remove appointments
            for (int i = 1; i <= 15; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Appointments",
                    keyColumn: "Id",
                    keyValue: new Guid($"aa100000-0000-0000-0000-{i:D12}"));
            }

            // Remove customers
            for (int i = 1; i <= 5; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Customers",
                    keyColumn: "Id",
                    keyValue: new Guid($"dd000000-0000-0000-0000-{i:D12}"));
            }

            // Remove staff services
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444403") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444402") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444403") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444401") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444402") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444403") });
            migrationBuilder.DeleteData(table: "StaffServices", keyColumns: new[] { "StaffId", "ServiceId" }, keyValues: new object[] { new Guid("cc000000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444404") });

            // Remove staff records
            for (int i = 1; i <= 4; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Staff",
                    keyColumn: "Id",
                    keyValue: new Guid($"cc000000-0000-0000-0000-{i:D12}"));
            }

            // Remove client companies
            migrationBuilder.DeleteData(table: "Clients", keyColumn: "Id", keyValue: new Guid("ff000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Clients", keyColumn: "Id", keyValue: new Guid("ff000000-0000-0000-0000-000000000002"));

            // Remove user roles
            migrationBuilder.DeleteData(table: "UserRoles", keyColumns: new[] { "RoleId", "UserId" }, keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000005") });
            migrationBuilder.DeleteData(table: "UserRoles", keyColumns: new[] { "RoleId", "UserId" }, keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000001") });
            migrationBuilder.DeleteData(table: "UserRoles", keyColumns: new[] { "RoleId", "UserId" }, keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000002") });
            migrationBuilder.DeleteData(table: "UserRoles", keyColumns: new[] { "RoleId", "UserId" }, keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000003") });
            migrationBuilder.DeleteData(table: "UserRoles", keyColumns: new[] { "RoleId", "UserId" }, keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000004"), new Guid("ee000000-0000-0000-0000-000000000004") });
            migrationBuilder.DeleteData(table: "UserRoles", keyColumns: new[] { "RoleId", "UserId" }, keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000004"), new Guid("ee000000-0000-0000-0000-000000000005") });

            // Remove users
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: new Guid("bb000000-0000-0000-0000-000000000005"));
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: new Guid("ee000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: new Guid("ee000000-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: new Guid("ee000000-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: new Guid("ee000000-0000-0000-0000-000000000004"));
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: new Guid("ee000000-0000-0000-0000-000000000005"));

            migrationBuilder.DropColumn(
                name: "MaxOccupancy",
                table: "LodgingProperties");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOrpz+1/S4f/oZ0KZZ/ayfwQbC1IjRmSdbn0bzqiTNuUovThGA1uZARcsVYtntc2qA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJfa98SNJ5Zr1R0Ebc5RQcH75DgtQhz1Cb7d7xX8yDpujxfaIU4RyCqobHg3B6sYCQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFAVL4g5zlB49o0SLuDVn9nSVi6763EMx9QSPpdsYRQ89JKjMcTAJ5BtJ7meEl++Gg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPia+Ijls54RNyoc5yEV4ZyqNx5rkXitSUxppn25mPHnPC2CS+enJxWgB/okkW+wtg==");
        }
    }
}
