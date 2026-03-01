using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerRoleAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("aa000000-0000-0000-0000-000000000005"), "5", "Manager", "MANAGER" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGdmkIYYVLqbXdKkZg50IDGpxTrQ0TQPDvwtFcztxwNzTY9a53EwyHQ7Y7b7vmsREg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "AQAAAAIAAYagAAAAEHb5w4xLoZYRtU1kBVpUMw2NgI1x+W+OmJiO7ptQtjcdfEOeuvGxdPSNrOp2DFaXBw==", 4 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "AQAAAAIAAYagAAAAEJHOhmVqv9bxzg/kHo580N5Ew2N4VwAUcN2hfI0NEYZDlJeDfya/6m3WWXyixcpxDA==", 5 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "Role", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("bb000000-0000-0000-0000-000000000004"), 0, "manager-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "manager@demo-barber.example", true, "Sarah", false, "Manager", false, null, null, "MANAGER@DEMO-BARBER.EXAMPLE", "MANAGER@DEMO-BARBER.EXAMPLE", "AQAAAAIAAYagAAAAEA3HvWcx1zen0eFthQvU6K02+HXUL7ISmaH/zrfPtt0v4vgrs4gAxuL1ngd/qMap5w==", null, false, null, null, 3, "manager-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "manager@demo-barber.example" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("aa000000-0000-0000-0000-000000000005"), new Guid("bb000000-0000-0000-0000-000000000004") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000005"), new Guid("bb000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aa000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA2nCbeZm1TLryi6PZxfwiq4+hggmyC4NllfXHmdDBnIZEWyI9gwISa57IKdZBAByw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "AQAAAAIAAYagAAAAEEMkThscyR7jpGtUzsnmtQ7rclygQbzDX8Mwy2vzPuN5tAXQ+/QmnlMShH6wt+WqRg==", 3 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "AQAAAAIAAYagAAAAEI53NgoT3podwonUEjQw6xIc9LTgU2jqJUOBzghSyTEHYNHQIugO6eeQMwqJVtuLRw==", 4 });
        }
    }
}
