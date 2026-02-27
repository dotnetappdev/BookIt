using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsAndAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Webhooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Webhooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Webhooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Webhooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Webhooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Webhooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WebhookDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WebhookDeliveries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "WebhookDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "WebhookDeliveries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "WebhookDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "WebhookDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StaffInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "StaffInvitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StaffInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "StaffInvitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "StaffInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StaffInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StaffAvailabilities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "StaffAvailabilities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StaffAvailabilities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "StaffAvailabilities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "StaffAvailabilities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StaffAvailabilities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Staff",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Staff",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ServiceCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ServiceCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ServiceCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "ServiceCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "ServiceCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ServiceCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "InterviewSlots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "InterviewSlots",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "InterviewSlots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "InterviewSlots",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "InterviewSlots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "InterviewSlots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EmailTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "EmailTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ClassSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ClassSessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ClassSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "ClassSessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "ClassSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ClassSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CandidateInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CandidateInvitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CandidateInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "CandidateInvitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "CandidateInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CandidateInvitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BusinessHours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "BusinessHours",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "BusinessHours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "BusinessHours",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "BusinessHours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "BusinessHours",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BookingForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "BookingForms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "BookingForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "BookingForms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "BookingForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "BookingForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BookingFormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "BookingFormFields",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "BookingFormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "BookingFormFields",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "BookingFormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "BookingFormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AppConfigurations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AppConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditedAt",
                table: "AppConfigurations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "AppConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EntityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "BookingForms",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555501"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BusinessHours",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222201"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BusinessHours",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222202"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BusinessHours",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222203"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BusinessHours",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222204"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BusinessHours",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222205"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BusinessHours",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222206"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333301"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333302"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444403"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444404"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedBy", "DeletedAt", "DeletedBy", "EditedAt", "EditedBy", "UpdatedBy" },
                values: new object[] { null, null, null, null, null, null });

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
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEMkThscyR7jpGtUzsnmtQ7rclygQbzDX8Mwy2vzPuN5tAXQ+/QmnlMShH6wt+WqRg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI53NgoT3podwonUEjQw6xIc9LTgU2jqJUOBzghSyTEHYNHQIugO6eeQMwqJVtuLRw==");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName_EntityId",
                table: "AuditLogs",
                columns: new[] { "EntityName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TenantId_ChangedAt",
                table: "AuditLogs",
                columns: new[] { "TenantId", "ChangedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Webhooks");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Webhooks");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Webhooks");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Webhooks");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Webhooks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Webhooks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WebhookDeliveries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WebhookDeliveries");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "WebhookDeliveries");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "WebhookDeliveries");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "WebhookDeliveries");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "WebhookDeliveries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StaffInvitations");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "StaffInvitations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StaffInvitations");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "StaffInvitations");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "StaffInvitations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StaffInvitations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StaffAvailabilities");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "StaffAvailabilities");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StaffAvailabilities");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "StaffAvailabilities");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "StaffAvailabilities");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StaffAvailabilities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "InterviewSlots");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "InterviewSlots");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "InterviewSlots");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "InterviewSlots");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "InterviewSlots");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "InterviewSlots");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CandidateInvitations");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CandidateInvitations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CandidateInvitations");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "CandidateInvitations");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "CandidateInvitations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CandidateInvitations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BusinessHours");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BusinessHours");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "BusinessHours");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "BusinessHours");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "BusinessHours");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BusinessHours");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BookingForms");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BookingForms");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "BookingForms");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "BookingForms");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "BookingForms");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BookingForms");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BookingFormFields");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BookingFormFields");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "BookingFormFields");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "BookingFormFields");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "BookingFormFields");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BookingFormFields");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppConfigurations");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AppConfigurations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AppConfigurations");

            migrationBuilder.DropColumn(
                name: "EditedAt",
                table: "AppConfigurations");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "AppConfigurations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppConfigurations");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHYrX7xDY2iwphSGei/lc26IL2GUbXiCnwDHU/Z1kK7aolDVCIB9UhI8/8Ds/HlS+g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPqUdMjJgdaHMpo8Ln3levOG2U9Ya8vqnUshT0+7A3CqcbuMt7D1PgMQ3Lo9UJ/r1w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA0wEiAUBliFIlYq3Vj7qV6RPE24dYp2dL0WY2zSKZMliiYx4iIsulSze33AKadrbQ==");
        }
    }
}
