using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVapiPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VapiPhoneNumber",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "VapiPhoneNumber",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAbO2/s9PhcqerVem6i3DxHPtmFGn7O/+CrLG+QbH2yYcveXZH/5dWGOGRyEpOxNJA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI/MLy23NMA4KpeSwEqspag2/4UTaSSZWi7XmfggCB1Seug6FbiH0yykaA/GN/HITw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEMnZbTG0dlOEALqKDSWCYxB1+QW4/i6aC3oMn9waH/uC9wz5MMAkXHswKknbSIPGQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENLN9YF/ahs0fyvSJ4ZwLAPvs05ijjB9HOKSUa5XNSra3nmGDtgoE9+mqUILVdlasQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VapiPhoneNumber",
                table: "Tenants");

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
        }
    }
}
