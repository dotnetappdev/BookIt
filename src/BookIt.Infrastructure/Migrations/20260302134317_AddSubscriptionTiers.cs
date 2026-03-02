using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionTiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plan = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MonthlyPriceGbp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyPriceUsd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyPriceEur = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxServices = table.Column<int>(type: "int", nullable: false),
                    MaxStaff = table.Column<int>(type: "int", nullable: false),
                    MaxLocations = table.Column<int>(type: "int", nullable: false),
                    MaxBookingsPerMonth = table.Column<int>(type: "int", nullable: false),
                    CanUseOnlinePayments = table.Column<bool>(type: "bit", nullable: false),
                    CanUseCustomForms = table.Column<bool>(type: "bit", nullable: false),
                    CanUseAiAssistant = table.Column<bool>(type: "bit", nullable: false),
                    CanUseInterviews = table.Column<bool>(type: "bit", nullable: false),
                    CanUseApiAccess = table.Column<bool>(type: "bit", nullable: false),
                    CanRemoveBranding = table.Column<bool>(type: "bit", nullable: false),
                    CanUseMultipleStaff = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_SubscriptionTiers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SubscriptionTiers",
                columns: new[] { "Id", "CanRemoveBranding", "CanUseAiAssistant", "CanUseApiAccess", "CanUseCustomForms", "CanUseInterviews", "CanUseMultipleStaff", "CanUseOnlinePayments", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "EditedAt", "EditedBy", "IsActive", "IsDeleted", "MaxBookingsPerMonth", "MaxLocations", "MaxServices", "MaxStaff", "MonthlyPriceEur", "MonthlyPriceGbp", "MonthlyPriceUsd", "Name", "Plan", "SortOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aa000000-0000-0000-0000-000000000001"), false, false, false, false, false, false, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Perfect for solo businesses just getting started", null, null, true, false, -1, 1, 3, 1, 0m, 0m, 0m, "Free", 0, 0, null, null },
                    { new Guid("aa000000-0000-0000-0000-000000000002"), false, false, false, true, false, true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "For growing teams and small businesses", null, null, true, false, -1, 1, 10, 5, 22.42m, 19m, 24.13m, "Starter", 1, 1, null, null },
                    { new Guid("aa000000-0000-0000-0000-000000000003"), false, true, true, true, true, true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Scale your business with advanced features", null, null, true, false, -1, 3, 50, 25, 57.82m, 49m, 62.23m, "Pro", 2, 2, null, null },
                    { new Guid("aa000000-0000-0000-0000-000000000004"), true, true, true, true, true, true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Unlimited scale for large organisations", null, null, true, false, -1, -1, -1, -1, 152.22m, 129m, 163.83m, "Enterprise", 3, 3, null, null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000000"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKFGCwhkyPdjNT+NSiHTSi8O+I7YScWGpsmMTx5psTWrNb6HLmGJRObd2ChLqwHKaw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEJ2oAdjPX+zZRi+CjVauHoiREhQuMdLJy/8l495XAFzvTNOQxhqtGJo6apaCWf4cg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF0k+hHiD85nRYYSzKvetxBClH3EwuFILG7s03JWrtCsqrI6ybsD51bBFGRu9ibvKg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFotFNOLoG92yXi/CjWEkw5GlPMIKrn6Pvl9T07Q4Am3ZlYaIMQYLLHMyqEQO+4RxQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKkVKg+yxT5y7xWBptPjPsJ5wW3XjI5BNNqo7rE44F6lyTcBncj2A7dL5V/ki3Hgiw==");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTiers_Plan",
                table: "SubscriptionTiers",
                column: "Plan",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionTiers");

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
        }
    }
}
