using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantSubdomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "EnableSoftDelete",
                table: "Tenants",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Subdomain",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SubdomainApproved",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Subdomain", "SubdomainApproved" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHjCffROOR9Kw5lCZz/MPm1r5zSmL7+9h7p1821Mq5c1LJeBHy/t1/NBaOzFyb3qKw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKjtvcuzZReovKbQVt3u9/rN4pXpuhmQJL0WztNuh2jMb2VpPfhRA6Yu+V0JTTYeQA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPVPMaFkcxnIBXSfYSZAJ9dBTC3pwFfILRd7RSfYMF1fTQj3ZhL9O0ACD6etSQRsWw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN99ZY6T4sKSsmweyYC7o/xpn9cN9U+PrelTPxvEqkOuUvCxsnpvCwIStHj7h+rB0w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subdomain",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SubdomainApproved",
                table: "Tenants");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableSoftDelete",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

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
    }
}
