using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_TenantId",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Services",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"),
                column: "Slug",
                value: "mens-haircut");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"),
                column: "Slug",
                value: "hair-beard-combo");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444403"),
                column: "Slug",
                value: "beard-trim");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444404"),
                column: "Slug",
                value: "hot-towel-shave");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEH7AVD2SZx/TGWG0GgiA3gDyZMnDqE8Mu2fTKraLYBI/HMYAAdGwsHDyISNm3/xQ4Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIqIPQqGNk4XAUbahQKZG/4qgSztfEFZB2Xb9kqkbY6eUcMmxwwsTCHJEqbGBczzkA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMvzXX0SJDHuitLm5LO7UK2w+24dLd/q3okZAJqnyr1+xRAUr5m0NbHegodbYGerNw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELz4Cl4o79N6pAZ4ZZqGUW8TW8cJ20sNUILa/2uP+ApkIFvtYxBRJraD06fhXJL+rA==");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TenantId_Slug",
                table: "Services",
                columns: new[] { "TenantId", "Slug" },
                unique: true,
                filter: "[Slug] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_TenantId_Slug",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Services");

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
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHb5w4xLoZYRtU1kBVpUMw2NgI1x+W+OmJiO7ptQtjcdfEOeuvGxdPSNrOp2DFaXBw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJHOhmVqv9bxzg/kHo580N5Ew2N4VwAUcN2hfI0NEYZDlJeDfya/6m3WWXyixcpxDA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000004"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA3HvWcx1zen0eFthQvU6K02+HXUL7ISmaH/zrfPtt0v4vgrs4gAxuL1ngd/qMap5w==");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TenantId",
                table: "Services",
                column: "TenantId");
        }
    }
}
