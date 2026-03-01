using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnableSoftDeleteToTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444401") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444402") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444401") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444403") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000005"), new Guid("44444444-4444-4444-4444-444444444402") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000006"), new Guid("44444444-4444-4444-4444-444444444402") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000007"), new Guid("44444444-4444-4444-4444-444444444404") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000008"), new Guid("44444444-4444-4444-4444-444444444401") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000009"), new Guid("44444444-4444-4444-4444-444444444403") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000010"), new Guid("44444444-4444-4444-4444-444444444401") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000011"), new Guid("44444444-4444-4444-4444-444444444402") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000012"), new Guid("44444444-4444-4444-4444-444444444401") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000013"), new Guid("44444444-4444-4444-4444-444444444404") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000014"), new Guid("44444444-4444-4444-4444-444444444401") });

            migrationBuilder.DeleteData(
                table: "AppointmentServices",
                keyColumns: new[] { "AppointmentId", "ServiceId" },
                keyValues: new object[] { new Guid("aa100000-0000-0000-0000-000000000015"), new Guid("44444444-4444-4444-4444-444444444401") });

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("dd000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("dd000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("dd000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("dd000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("dd000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444401"), new Guid("cc000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444403"), new Guid("cc000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444401"), new Guid("cc000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444402"), new Guid("cc000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444403"), new Guid("cc000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444401"), new Guid("cc000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444402"), new Guid("cc000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444403"), new Guid("cc000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "StaffServices",
                keyColumns: new[] { "ServiceId", "StaffId" },
                keyValues: new object[] { new Guid("44444444-4444-4444-4444-444444444404"), new Guid("cc000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000003"), new Guid("ee000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000004"), new Guid("ee000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000004"), new Guid("ee000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: new Guid("aa100000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: new Guid("cc000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: new Guid("cc000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: new Guid("cc000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: new Guid("cc000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("ff000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("ff000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ee000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ee000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ee000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ee000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ee000000-0000-0000-0000-000000000005"));

            migrationBuilder.AddColumn<bool>(
                name: "EnableSoftDelete",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "EnableSoftDelete",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMXGxzWeTZAU7Aj0KEx4/8Drg9GcfEs1y+6PvLVRdrw4CJB13vG2/GZsLizWXIAheQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK+V6xNl4WE6NGZPXXnW5VTgoI+fpHqK9HM/JugwZiAhxKihKdZJxY+sU74jJTBaaA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKpJOI+02H7SgzDZ7I8pyC+xCAKUpvpcez7fC1vUV+JVhj8QyZT1ktQC+E6n2Zo0LA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIL/PW/tSa5N8HiuxmVrvRURNEWdEQMfrLCSW1X65vrGMGg6o13BIlyk1y7zJ+JT7w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableSoftDelete",
                table: "Tenants");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "City", "Country", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "Email", "FirstName", "Gender", "IsDeleted", "LastName", "LastVisit", "MarketingOptIn", "MembershipNumber", "Mobile", "Notes", "Phone", "PostCode", "SmsOptIn", "Tags", "TenantId", "TotalBookings", "TotalSpent", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { new Guid("dd000000-0000-0000-0000-000000000001"), null, "London", "UK", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, "alice.thompson@example.com", "Alice", "Female", false, "Thompson", new DateTime(2026, 1, 20, 10, 0, 0, 0, DateTimeKind.Utc), true, "MBR-001", null, null, "555-1001", null, false, "regular", new Guid("11111111-1111-1111-1111-111111111111"), 5, 125.00m, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000002"), null, "Manchester", "UK", new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, "bob.williams@example.com", "Bob", "Male", false, "Williams", new DateTime(2026, 1, 28, 11, 0, 0, 0, DateTimeKind.Utc), false, "MBR-002", null, null, "555-1002", null, false, null, new Guid("11111111-1111-1111-1111-111111111111"), 3, 75.00m, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000003"), null, "Birmingham", "UK", new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, "carol.davies@example.com", "Carol", "Female", false, "Davies", new DateTime(2026, 2, 5, 9, 0, 0, 0, DateTimeKind.Utc), true, "MBR-003", null, null, "555-1003", null, true, "vip", new Guid("11111111-1111-1111-1111-111111111111"), 8, 320.00m, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000004"), null, "Leeds", "UK", new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, "david.harrison@example.com", "David", "Male", false, "Harrison", new DateTime(2026, 1, 15, 14, 0, 0, 0, DateTimeKind.Utc), true, null, null, null, "555-1004", null, false, null, new Guid("11111111-1111-1111-1111-111111111111"), 2, 50.00m, null, null, null },
                    { new Guid("dd000000-0000-0000-0000-000000000005"), null, "Bristol", "UK", new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, null, "eve.jackson@example.com", "Eve", "Female", false, "Jackson", new DateTime(2026, 2, 10, 13, 0, 0, 0, DateTimeKind.Utc), false, "MBR-005", null, "Regular client — prefers Emma", "555-1005", null, false, "regular", new Guid("11111111-1111-1111-1111-111111111111"), 4, 160.00m, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "Bio", "ClientId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "Email", "FirstName", "IsActive", "IsDeleted", "LastName", "Phone", "PhotoUrl", "SortOrder", "TenantId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[] { new Guid("cc000000-0000-0000-0000-000000000001"), "Demo barber for testing the staff experience", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "staff@demo-barber.com", "John", true, false, "Barber", "555-0001", null, 1, new Guid("11111111-1111-1111-1111-111111111111"), null, null, new Guid("bb000000-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE9gVhyh97Jy0kC4myommUB75CDDCfgPdJro4fldHZvKJqdoR1mcnG/qkYOr7dLadA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAW1DiicZMm6fD8MfSfDue7LOh6qzG3x6bprTLvZcd97AB0jc5a4CP36n2kWkAdjag==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEITOgJa/Ddj1h77yJwWa1lA5o/DCyPDQHhObFr2rV27npA2YOYUJ2eM9NHTUFKQGsg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO460tmlb3IuSivYONPuCgfSQpOIy+rrx3tm7WqN5n+0EMu7Jt8ZlEv/1SOiUgmC2w==");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "Role", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("bb000000-0000-0000-0000-000000000005"), 0, "superadmin-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "superadmin@bookit.app", true, "Super", false, "Admin", false, null, null, "SUPERADMIN@BOOKIT.APP", "SUPERADMIN@BOOKIT.APP", "AQAAAAIAAYagAAAAEP1vd7Ymovqm5D/Vi5LNHBEJvaGP3ZH6DTYDAWgqwEt0ufoljgzwKR8/PT1Ep/wz/g==", null, false, null, null, 1, "superadmin-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "superadmin@bookit.app" },
                    { new Guid("ee000000-0000-0000-0000-000000000001"), 0, "james-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "james@elitehair.com", true, "James", false, "Martinez", false, null, null, "JAMES@ELITEHAIR.COM", "JAMES@ELITEHAIR.COM", "AQAAAAIAAYagAAAAEJG449ssHHixsjQHRmtQ77ObBnLZBt3s4t54hGgYCLV7/EuqQsTEMNl8VueuB7hknw==", "555-0201", false, null, null, 4, "james-security-stamp", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "james@elitehair.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000002"), 0, "emma-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "emma@elitehair.com", true, "Emma", false, "Wilson", false, null, null, "EMMA@ELITEHAIR.COM", "EMMA@ELITEHAIR.COM", "AQAAAAIAAYagAAAAEJG449ssHHixsjQHRmtQ77ObBnLZBt3s4t54hGgYCLV7/EuqQsTEMNl8VueuB7hknw==", "555-0202", false, null, null, 4, "emma-security-stamp", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "emma@elitehair.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000003"), 0, "oliver-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "oliver@urbanstyle.com", true, "Oliver", false, "Brown", false, null, null, "OLIVER@URBANSTYLE.COM", "OLIVER@URBANSTYLE.COM", "AQAAAAIAAYagAAAAEJG449ssHHixsjQHRmtQ77ObBnLZBt3s4t54hGgYCLV7/EuqQsTEMNl8VueuB7hknw==", "555-0203", false, null, null, 4, "oliver-security-stamp", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "oliver@urbanstyle.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000004"), 0, "client1-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sarah@elitehair.com", true, "Sarah", false, "Johnson", false, null, null, "SARAH@ELITEHAIR.COM", "SARAH@ELITEHAIR.COM", "AQAAAAIAAYagAAAAEEg7zuYqljGMg56j3zUnQcWfW3B1ES52Rac1If1LF5QpglTD9iCwA2fjdi++mqLUNQ==", "555-0101", false, null, null, 5, "client1-security-stamp", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "sarah@elitehair.com" },
                    { new Guid("ee000000-0000-0000-0000-000000000005"), 0, "client2-concurrency-stamp", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "michael@urbanstyle.com", true, "Michael", false, "Chen", false, null, null, "MICHAEL@URBANSTYLE.COM", "MICHAEL@URBANSTYLE.COM", "AQAAAAIAAYagAAAAEEg7zuYqljGMg56j3zUnQcWfW3B1ES52Rac1If1LF5QpglTD9iCwA2fjdi++mqLUNQ==", "555-0102", false, null, null, 5, "client2-security-stamp", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "michael@urbanstyle.com" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "BookingPin", "CancellationReason", "ConfirmationToken", "CreatedAt", "CreatedBy", "CustomFormDataJson", "CustomerEmail", "CustomerId", "CustomerName", "CustomerNotes", "CustomerPhone", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "EndTime", "InternalNotes", "IsDeleted", "MeetingId", "MeetingInstructions", "MeetingLink", "MeetingPassword", "MeetingType", "PaymentProvider", "PaymentReference", "PaymentStatus", "ReminderSent", "StaffId", "StartTime", "Status", "TenantId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aa100000-0000-0000-0000-000000000001"), "A1B2C3", null, "tok-appt01", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "alice.thompson@example.com", new Guid("dd000000-0000-0000-0000-000000000001"), "Alice Thompson", null, "555-1001", null, null, null, null, new DateTime(2026, 1, 8, 10, 30, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 25.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000008"), "V4W5X6", null, "tok-appt08", new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "bob.williams@example.com", new Guid("dd000000-0000-0000-0000-000000000002"), "Bob Williams", null, "555-1002", null, null, null, null, new DateTime(2026, 1, 28, 12, 30, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 28, 12, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 25.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000009"), "Y7Z8A1", null, "tok-appt09", new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "eve.jackson@example.com", new Guid("dd000000-0000-0000-0000-000000000005"), "Eve Jackson", null, "555-1005", null, null, null, null, new DateTime(2026, 1, 29, 14, 20, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 29, 14, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 15.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000014"), "N5O6P7", null, "tok-appt14", new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "eve.jackson@example.com", new Guid("dd000000-0000-0000-0000-000000000005"), "Eve Jackson", null, "555-1005", null, null, null, null, new DateTime(2026, 3, 10, 10, 30, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 0, false, new Guid("cc000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), 2, new Guid("11111111-1111-1111-1111-111111111111"), 25.00m, null, null }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "CompanyName", "ContactName", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "Email", "IsActive", "IsDeleted", "Notes", "Phone", "TenantId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { new Guid("ff000000-0000-0000-0000-000000000001"), null, "Elite Hair Solutions", "Sarah Johnson", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "sarah@elitehair.com", true, false, "VIP client — priority scheduling", "555-0101", new Guid("11111111-1111-1111-1111-111111111111"), null, null, new Guid("ee000000-0000-0000-0000-000000000004") },
                    { new Guid("ff000000-0000-0000-0000-000000000002"), null, "Urban Style Group", "Michael Chen", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "michael@urbanstyle.com", true, false, null, "555-0102", new Guid("11111111-1111-1111-1111-111111111111"), null, null, new Guid("ee000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "StaffServices",
                columns: new[] { "ServiceId", "StaffId", "ServiceId1" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444401"), new Guid("cc000000-0000-0000-0000-000000000001"), null },
                    { new Guid("44444444-4444-4444-4444-444444444403"), new Guid("cc000000-0000-0000-0000-000000000001"), null }
                });

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

            migrationBuilder.InsertData(
                table: "AppointmentServices",
                columns: new[] { "AppointmentId", "ServiceId", "DurationAtBooking", "PriceAtBooking" },
                values: new object[,]
                {
                    { new Guid("aa100000-0000-0000-0000-000000000001"), new Guid("44444444-4444-4444-4444-444444444401"), 30, 25.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000008"), new Guid("44444444-4444-4444-4444-444444444401"), 30, 25.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000009"), new Guid("44444444-4444-4444-4444-444444444403"), 20, 15.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000014"), new Guid("44444444-4444-4444-4444-444444444401"), 30, 25.00m }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "Bio", "ClientId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "Email", "FirstName", "IsActive", "IsDeleted", "LastName", "Phone", "PhotoUrl", "SortOrder", "TenantId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { new Guid("cc000000-0000-0000-0000-000000000002"), "Master Barber with 10 years experience", new Guid("ff000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "james@elitehair.com", "James", true, false, "Martinez", "555-0201", null, 2, new Guid("11111111-1111-1111-1111-111111111111"), null, null, new Guid("ee000000-0000-0000-0000-000000000001") },
                    { new Guid("cc000000-0000-0000-0000-000000000003"), "Specialist in modern cuts and styling", new Guid("ff000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "emma@elitehair.com", "Emma", true, false, "Wilson", "555-0202", null, 3, new Guid("11111111-1111-1111-1111-111111111111"), null, null, new Guid("ee000000-0000-0000-0000-000000000002") },
                    { new Guid("cc000000-0000-0000-0000-000000000004"), "Expert in beard grooming and hot shaves", new Guid("ff000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "oliver@urbanstyle.com", "Oliver", true, false, "Brown", "555-0203", null, 4, new Guid("11111111-1111-1111-1111-111111111111"), null, null, new Guid("ee000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "BookingPin", "CancellationReason", "ConfirmationToken", "CreatedAt", "CreatedBy", "CustomFormDataJson", "CustomerEmail", "CustomerId", "CustomerName", "CustomerNotes", "CustomerPhone", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "EndTime", "InternalNotes", "IsDeleted", "MeetingId", "MeetingInstructions", "MeetingLink", "MeetingPassword", "MeetingType", "PaymentProvider", "PaymentReference", "PaymentStatus", "ReminderSent", "StaffId", "StartTime", "Status", "TenantId", "TotalAmount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aa100000-0000-0000-0000-000000000002"), "D4E5F6", null, "tok-appt02", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "bob.williams@example.com", new Guid("dd000000-0000-0000-0000-000000000002"), "Bob Williams", null, "555-1002", null, null, null, null, new DateTime(2026, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 10, 11, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 40.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000003"), "G7H8I9", null, "tok-appt03", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "carol.davies@example.com", new Guid("dd000000-0000-0000-0000-000000000003"), "Carol Davies", null, "555-1003", null, null, null, null, new DateTime(2026, 1, 12, 9, 30, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 12, 9, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 25.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000004"), "J1K2L3", null, "tok-appt04", new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "david.harrison@example.com", new Guid("dd000000-0000-0000-0000-000000000004"), "David Harrison", null, "555-1004", null, null, null, null, new DateTime(2026, 1, 15, 14, 20, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 15, 14, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 15.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000005"), "M4N5O6", null, "tok-appt05", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "eve.jackson@example.com", new Guid("dd000000-0000-0000-0000-000000000005"), "Eve Jackson", null, "555-1005", null, null, null, null, new DateTime(2026, 1, 18, 11, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 18, 10, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 40.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000006"), "P7Q8R9", null, "tok-appt06", new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "alice.thompson@example.com", new Guid("dd000000-0000-0000-0000-000000000001"), "Alice Thompson", null, "555-1001", null, null, null, null, new DateTime(2026, 1, 22, 10, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 22, 9, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 40.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000007"), "S1T2U3", null, "tok-appt07", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "carol.davies@example.com", new Guid("dd000000-0000-0000-0000-000000000003"), "Carol Davies", null, "555-1003", null, null, null, null, new DateTime(2026, 1, 25, 11, 45, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 1, true, new Guid("cc000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 25, 11, 0, 0, 0, DateTimeKind.Utc), 3, new Guid("11111111-1111-1111-1111-111111111111"), 35.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000010"), "B2C3D4", "Customer requested reschedule", "tok-appt10", new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "david.harrison@example.com", new Guid("dd000000-0000-0000-0000-000000000004"), "David Harrison", null, "555-1004", null, null, null, null, new DateTime(2026, 1, 31, 15, 30, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 0, false, new Guid("cc000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 31, 15, 0, 0, 0, DateTimeKind.Utc), 4, new Guid("11111111-1111-1111-1111-111111111111"), 25.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000011"), "E5F6G7", null, "tok-appt11", new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "alice.thompson@example.com", new Guid("dd000000-0000-0000-0000-000000000001"), "Alice Thompson", null, "555-1001", null, null, null, null, new DateTime(2026, 3, 3, 11, 0, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 0, false, new Guid("cc000000-0000-0000-0000-000000000002"), new DateTime(2026, 3, 3, 10, 0, 0, 0, DateTimeKind.Utc), 2, new Guid("11111111-1111-1111-1111-111111111111"), 40.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000012"), "H8I9J1", null, "tok-appt12", new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "carol.davies@example.com", new Guid("dd000000-0000-0000-0000-000000000003"), "Carol Davies", null, "555-1003", null, null, null, null, new DateTime(2026, 3, 5, 9, 30, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 0, false, new Guid("cc000000-0000-0000-0000-000000000003"), new DateTime(2026, 3, 5, 9, 0, 0, 0, DateTimeKind.Utc), 2, new Guid("11111111-1111-1111-1111-111111111111"), 25.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000013"), "K2L3M4", null, "tok-appt13", new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "bob.williams@example.com", new Guid("dd000000-0000-0000-0000-000000000002"), "Bob Williams", null, "555-1002", null, null, null, null, new DateTime(2026, 3, 7, 12, 45, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 0, false, new Guid("cc000000-0000-0000-0000-000000000004"), new DateTime(2026, 3, 7, 12, 0, 0, 0, DateTimeKind.Utc), 0, new Guid("11111111-1111-1111-1111-111111111111"), 35.00m, null, null },
                    { new Guid("aa100000-0000-0000-0000-000000000015"), "Q8R9S1", null, "tok-appt15", new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "david.harrison@example.com", new Guid("dd000000-0000-0000-0000-000000000004"), "David Harrison", "Please trim short on sides", "555-1004", null, null, null, null, new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc), null, false, null, null, null, null, 1, null, null, 0, false, new Guid("cc000000-0000-0000-0000-000000000002"), new DateTime(2026, 3, 14, 15, 0, 0, 0, DateTimeKind.Utc), 2, new Guid("11111111-1111-1111-1111-111111111111"), 25.00m, null, null }
                });

            migrationBuilder.InsertData(
                table: "StaffServices",
                columns: new[] { "ServiceId", "StaffId", "ServiceId1" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444401"), new Guid("cc000000-0000-0000-0000-000000000002"), null },
                    { new Guid("44444444-4444-4444-4444-444444444402"), new Guid("cc000000-0000-0000-0000-000000000002"), null },
                    { new Guid("44444444-4444-4444-4444-444444444403"), new Guid("cc000000-0000-0000-0000-000000000002"), null },
                    { new Guid("44444444-4444-4444-4444-444444444401"), new Guid("cc000000-0000-0000-0000-000000000003"), null },
                    { new Guid("44444444-4444-4444-4444-444444444402"), new Guid("cc000000-0000-0000-0000-000000000003"), null },
                    { new Guid("44444444-4444-4444-4444-444444444403"), new Guid("cc000000-0000-0000-0000-000000000004"), null },
                    { new Guid("44444444-4444-4444-4444-444444444404"), new Guid("cc000000-0000-0000-0000-000000000004"), null }
                });

            migrationBuilder.InsertData(
                table: "AppointmentServices",
                columns: new[] { "AppointmentId", "ServiceId", "DurationAtBooking", "PriceAtBooking" },
                values: new object[,]
                {
                    { new Guid("aa100000-0000-0000-0000-000000000002"), new Guid("44444444-4444-4444-4444-444444444402"), 60, 40.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000003"), new Guid("44444444-4444-4444-4444-444444444401"), 30, 25.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000004"), new Guid("44444444-4444-4444-4444-444444444403"), 20, 15.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000005"), new Guid("44444444-4444-4444-4444-444444444402"), 60, 40.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000006"), new Guid("44444444-4444-4444-4444-444444444402"), 60, 40.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000007"), new Guid("44444444-4444-4444-4444-444444444404"), 45, 35.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000010"), new Guid("44444444-4444-4444-4444-444444444401"), 30, 25.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000011"), new Guid("44444444-4444-4444-4444-444444444402"), 60, 40.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000012"), new Guid("44444444-4444-4444-4444-444444444401"), 30, 25.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000013"), new Guid("44444444-4444-4444-4444-444444444404"), 45, 35.00m },
                    { new Guid("aa100000-0000-0000-0000-000000000015"), new Guid("44444444-4444-4444-4444-444444444401"), 30, 25.00m }
                });
        }
    }
}
