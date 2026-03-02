using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyPhotosAmenitiesAndCustomBusinessType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomBusinessType",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PropertyAmenities",
                columns: table => new
                {
                    LodgingPropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmenityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAmenities", x => new { x.LodgingPropertyId, x.AmenityId });
                    table.ForeignKey(
                        name: "FK_PropertyAmenities_Amenities_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropertyAmenities_LodgingProperties_LodgingPropertyId",
                        column: x => x.LodgingPropertyId,
                        principalTable: "LodgingProperties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PropertyPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LodgingPropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_PropertyPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyPhotos_LodgingProperties_LodgingPropertyId",
                        column: x => x.LodgingPropertyId,
                        principalTable: "LodgingProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CustomBusinessType",
                value: null);

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

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAmenities_AmenityId",
                table: "PropertyAmenities",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPhotos_LodgingPropertyId",
                table: "PropertyPhotos",
                column: "LodgingPropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyAmenities");

            migrationBuilder.DropTable(
                name: "PropertyPhotos");

            migrationBuilder.DropColumn(
                name: "CustomBusinessType",
                table: "Tenants");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000000"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGGLtaIQUBnla3GztUqXd8kYbNuGfFP/tpKRBd3UpEycddIdBEtZ0tXb6NxPP7ny2Q==");

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
        }
    }
}
