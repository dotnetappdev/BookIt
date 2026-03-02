using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFormPublishStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublishStatus",
                table: "BookingForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BookingForms",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555501"),
                column: "PublishStatus",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000000"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAhDm+XyLOjK241OCvLORMJ3Vq9RwIHRRevtZSp422oSq8nOccGZAskM5m0yV1q/eg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGYLuvBZNq0GbfiW3JPzJEcalKMw7nU3GLmjseLjFDBVjO9voLAvF/TzMEWxxEdxog==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFC1vGVdYE7jHezDVTA8Xzz2NK9r5v6lDV6dQQjkOYV47GZ0xRBtYAoE+dxf4hSJnw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHAbiNZJHc4Wo1KEmY6Cwe2uG53967ZKwJoX6SV5GOCaOsL2X06xAjZvwjg1gs1aoA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELF+E0R8lFfoz/6xzaxM8IK7eoADUCrtsUn3CGbaqJJx0ECLn13XCway/3mGqv7q1w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishStatus",
                table: "BookingForms");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000000"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKqN8rKO5uivQLFYEI4cReOFDjZlQ9U8yOPxrjJQdDfmC37TJjBfCp068Ono62b9ig==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBNesJ7En7Jeuq7zeSW1irNbLDtxYbP2h3lPTpRJj5Ih1hvKEQakJUlvza8Y100Beg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFbkLrdRldzghC2FF1QvOKJLOVyIR2GCzdGb8yms19q+BDKtEP4m9XVGMVQcL/Qjog==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDg0J8VOZ9JJ74SUYRn3Uwd88+HAouHzyyXT92c1m2mRgbu4Dzbpr8qDq4jdQoi1og==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKpGTmxvIcQ5ypTmz0FBMfJBY/PJRAbDVHIM8zjheUh4IS7Ar2tz8uQ+15WA1Xxc3w==");
        }
    }
}
