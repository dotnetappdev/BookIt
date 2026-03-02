using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingFormIdToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingFormId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plan = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProviderSubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevenueCatCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentProvider = table.Column<int>(type: "int", nullable: true),
                    MonthlyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrialEndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelAtPeriodEnd = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"),
                column: "BookingFormId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"),
                column: "BookingFormId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444403"),
                column: "BookingFormId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444404"),
                column: "BookingFormId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000000"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMMXO688ctMxXeD9R6sWE7G8da28KpPTupUdFxUJz4894fmXIyrv2oshXiEYwWe+ow==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOA2/ixSEI9PozeUDfSlLFxKeuknrOz8jhbjhUA2Xpyo+2ukxSl8Smxd4LeBgroP+w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEECgFrfZHEO2FRdhrK+PI1s3d0ArW9Tuo5jOzjHioXxUJ4NzFjwlbljqSeSEENRqVw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENtlviJ7trLUWeJE8KXtI4YpuKNzzKrmG8cG4nGUlmOhPVtGaKr/PoSqn3fH2jpQaA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO19sZ8Cg+QQDg07tr+81vY+H3vWbjJz3woTLWb8KCfTr3NflaFhC/jgwPVVuvWWXQ==");

            migrationBuilder.CreateIndex(
                name: "IX_Services_BookingFormId",
                table: "Services",
                column: "BookingFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_TenantId",
                table: "Subscriptions",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_BookingForms_BookingFormId",
                table: "Services",
                column: "BookingFormId",
                principalTable: "BookingForms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_BookingForms_BookingFormId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Services_BookingFormId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "BookingFormId",
                table: "Services");

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
    }
}
