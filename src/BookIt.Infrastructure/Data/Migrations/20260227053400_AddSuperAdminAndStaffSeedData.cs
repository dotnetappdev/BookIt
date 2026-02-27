using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminAndStaffSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed SuperAdmin user (platform-level)
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "Role", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("bb000000-0000-0000-0000-000000000005"), 0, "superadmin-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "superadmin@bookit.app", true, "Super", false, "Admin", false, null, null, "SUPERADMIN@BOOKIT.APP", "SUPERADMIN@BOOKIT.APP", "AQAAAAIAAYagAAAAEP1vd7Ymovqm5D/Vi5LNHBEJvaGP3ZH6DTYDAWgqwEt0ufoljgzwKR8/PT1Ep/wz/g==", null, false, null, null, 1, "superadmin-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "superadmin@bookit.app" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("aa000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000005") });

            // Seed Staff record linked to the demo staff user so they can see their own appointments
            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "Bio", "ClientId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "Email", "FirstName", "IsActive", "IsDeleted", "LastName", "Phone", "PhotoUrl", "SortOrder", "TenantId", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[] { new Guid("cc000000-0000-0000-0000-000000000001"), "Demo barber for testing the staff experience", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, "staff@demo-barber.com", "John", true, false, "Barber", "555-0001", null, 1, new Guid("11111111-1111-1111-1111-111111111111"), null, null, new Guid("bb000000-0000-0000-0000-000000000002") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: new Guid("cc000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000005"));
        }
    }
}
