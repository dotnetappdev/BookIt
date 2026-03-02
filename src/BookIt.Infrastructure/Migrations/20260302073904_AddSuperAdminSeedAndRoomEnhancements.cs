using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminSeedAndRoomEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BedType",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBeds",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PetFriendly",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WheelchairAccessible",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH+eExqrcWlECyZ1gCJuLT6DR7Za7u7JbndYwKdXqFhcFEPNeu1I2hYVV2ZKNblNwg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAQMUFtZ4vI7duneiniRflWrEp4VMc86TCbA74vB7FNxlZrAaUWZtlPiB1LXtzBvSg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJux5LLLyaCPX41WWLqhFjnnzp8yGNJ04JEdyhH8WTYYRWfl6DSDfw9UCjSJLMW4wg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEQv0QP8pbOcIJgYtfWMnL7EepeuHKsJbN44fHxuzAyR8k/TpPi/rlce5YDmIbGWMg==");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "MembershipNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "Role", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("bb000000-0000-0000-0000-000000000000"), 0, "superadmin-concurrency-stamp-1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "superadmin@bookit.app", true, "Super", false, "Admin", false, null, null, "SUPERADMIN@BOOKIT.APP", "SUPERADMIN@BOOKIT.APP", "AQAAAAIAAYagAAAAEGGLtaIQUBnla3GztUqXd8kYbNuGfFP/tpKRBd3UpEycddIdBEtZ0tXb6NxPP7ny2Q==", null, false, null, null, 1, "superadmin-security-stamp-1", new Guid("11111111-1111-1111-1111-111111111111"), false, null, "superadmin@bookit.app" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("aa000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000000") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aa000000-0000-0000-0000-000000000001"), new Guid("bb000000-0000-0000-0000-000000000000") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000000"));

            migrationBuilder.DropColumn(
                name: "BedType",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NumberOfBeds",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PetFriendly",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "WheelchairAccessible",
                table: "Rooms");

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
