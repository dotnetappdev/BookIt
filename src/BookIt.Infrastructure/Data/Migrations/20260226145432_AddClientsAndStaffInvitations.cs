using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClientsAndStaffInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffAvailabilities_Staff_StaffId",
                table: "StaffAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffServices_Services_ServiceId",
                table: "StaffServices");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffServices_Staff_StaffId",
                table: "StaffServices");

            migrationBuilder.AddColumn<string>(
                name: "MembershipNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerImageUrl",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingPageTitle",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClickSendApiKey",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClickSendFromNumber",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClickSendUsername",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ElevenLabsApiKey",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ElevenLabsVoiceId",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableAiChat",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableEmailNotifications",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableEmailReminders",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableSmsNotifications",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableSmsReminders",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OpenAiApiKey",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReminderAlerts",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevenueCatApiKey",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevenueCatEntitlementId",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendGridApiKey",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendGridFromEmail",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendGridFromName",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmsProvider",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Theme",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TwilioAccountSid",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwilioAuthToken",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwilioFromNumber",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VapiPublicKey",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId1",
                table: "StaffServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Staff",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorIdsJson",
                table: "ClassSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingPin",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketingOptIn = table.Column<bool>(type: "bit", nullable: false),
                    SmsOptIn = table.Column<bool>(type: "bit", nullable: false),
                    TotalBookings = table.Column<int>(type: "int", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastVisit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectLine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InterviewerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlotStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlotEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoConferenceProvider = table.Column<int>(type: "int", nullable: false),
                    ConferenceMeetingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConferencePassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConferenceHostUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConferenceDialIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookedByInvitationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSlots_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterviewSlots_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterviewSlots_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffInvitations_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffInvitations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Webhooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Events = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Webhooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Webhooks_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CandidateEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CandidatePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    BookedSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvitedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateInvitations_InterviewSlots_BookedSlotId",
                        column: x => x.BookedSlotId,
                        principalTable: "InterviewSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CandidateInvitations_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateInvitations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebhookDeliveries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebhookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    AttemptCount = table.Column<int>(type: "int", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WebhookId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookDeliveries_Webhooks_WebhookId",
                        column: x => x.WebhookId,
                        principalTable: "Webhooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WebhookDeliveries_Webhooks_WebhookId1",
                        column: x => x.WebhookId1,
                        principalTable: "Webhooks",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "BannerImageUrl", "BookingPageTitle", "ClickSendApiKey", "ClickSendFromNumber", "ClickSendUsername", "ElevenLabsApiKey", "ElevenLabsVoiceId", "EnableAiChat", "EnableEmailNotifications", "EnableEmailReminders", "EnableSmsNotifications", "EnableSmsReminders", "OpenAiApiKey", "ReminderAlerts", "RevenueCatApiKey", "RevenueCatEntitlementId", "SendGridApiKey", "SendGridFromEmail", "SendGridFromName", "SmsProvider", "Theme", "TwilioAccountSid", "TwilioAuthToken", "TwilioFromNumber", "VapiPublicKey" },
                values: new object[] { null, null, null, null, null, null, null, true, false, true, false, false, null, "1440", null, "premium", null, null, null, 0, 0, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                columns: new[] { "MembershipNumber", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEHYrX7xDY2iwphSGei/lc26IL2GUbXiCnwDHU/Z1kK7aolDVCIB9UhI8/8Ds/HlS+g==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                columns: new[] { "MembershipNumber", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEPqUdMjJgdaHMpo8Ln3levOG2U9Ya8vqnUshT0+7A3CqcbuMt7D1PgMQ3Lo9UJ/r1w==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                columns: new[] { "MembershipNumber", "PasswordHash" },
                values: new object[] { null, "AQAAAAIAAYagAAAAEA0wEiAUBliFIlYq3Vj7qV6RPE24dYp2dL0WY2zSKZMliiYx4iIsulSze33AKadrbQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_StaffServices_ServiceId1",
                table: "StaffServices",
                column: "ServiceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_ClientId",
                table: "Staff",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateInvitations_BookedSlotId",
                table: "CandidateInvitations",
                column: "BookedSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateInvitations_ServiceId",
                table: "CandidateInvitations",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateInvitations_TenantId_CandidateEmail",
                table: "CandidateInvitations",
                columns: new[] { "TenantId", "CandidateEmail" });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateInvitations_Token",
                table: "CandidateInvitations",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_TenantId_Email",
                table: "Clients",
                columns: new[] { "TenantId", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UserId",
                table: "Clients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_TenantId",
                table: "EmailTemplates",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlots_IsBooked",
                table: "InterviewSlots",
                column: "IsBooked");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlots_ServiceId",
                table: "InterviewSlots",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlots_StaffId",
                table: "InterviewSlots",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSlots_TenantId_SlotStart",
                table: "InterviewSlots",
                columns: new[] { "TenantId", "SlotStart" });

            migrationBuilder.CreateIndex(
                name: "IX_StaffInvitations_StaffId",
                table: "StaffInvitations",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffInvitations_TenantId_Email",
                table: "StaffInvitations",
                columns: new[] { "TenantId", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_StaffInvitations_Token",
                table: "StaffInvitations",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebhookDeliveries_WebhookId",
                table: "WebhookDeliveries",
                column: "WebhookId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookDeliveries_WebhookId1",
                table: "WebhookDeliveries",
                column: "WebhookId1");

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_TenantId",
                table: "Webhooks",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Clients_ClientId",
                table: "Staff",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffAvailabilities_Staff_StaffId",
                table: "StaffAvailabilities",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffServices_Services_ServiceId",
                table: "StaffServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffServices_Services_ServiceId1",
                table: "StaffServices",
                column: "ServiceId1",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffServices_Staff_StaffId",
                table: "StaffServices",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Clients_ClientId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffAvailabilities_Staff_StaffId",
                table: "StaffAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffServices_Services_ServiceId",
                table: "StaffServices");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffServices_Services_ServiceId1",
                table: "StaffServices");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffServices_Staff_StaffId",
                table: "StaffServices");

            migrationBuilder.DropTable(
                name: "CandidateInvitations");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "StaffInvitations");

            migrationBuilder.DropTable(
                name: "WebhookDeliveries");

            migrationBuilder.DropTable(
                name: "InterviewSlots");

            migrationBuilder.DropTable(
                name: "Webhooks");

            migrationBuilder.DropIndex(
                name: "IX_StaffServices_ServiceId1",
                table: "StaffServices");

            migrationBuilder.DropIndex(
                name: "IX_Staff_ClientId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "MembershipNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BannerImageUrl",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "BookingPageTitle",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ClickSendApiKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ClickSendFromNumber",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ClickSendUsername",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ElevenLabsApiKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ElevenLabsVoiceId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableAiChat",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableEmailNotifications",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableEmailReminders",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableSmsNotifications",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "EnableSmsReminders",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "OpenAiApiKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ReminderAlerts",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "RevenueCatApiKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "RevenueCatEntitlementId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SendGridApiKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SendGridFromEmail",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SendGridFromName",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SmsProvider",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "TwilioAccountSid",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "TwilioAuthToken",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "TwilioFromNumber",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "VapiPublicKey",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ServiceId1",
                table: "StaffServices");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "InstructorIdsJson",
                table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "BookingPin",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGwR/2NYtHitxRuzYjmL8wJ4ENavVF6ewLrU2fqk6RPL/6Oe2CwePPLYIE11AEt1bg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000002"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBzMgl0j74wwXZGH9HQ2iStXPVkB90Ubfkpjmx9jrVhOTAc7+oYCjfFUnRUrrvwwsA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bb000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAnmxV/MkJ1ZMiM8q9/9VMRO3eHv8DPlDW64edSr964Lp597anQeJ7atqp3d6xVTpA==");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffAvailabilities_Staff_StaffId",
                table: "StaffAvailabilities",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffServices_Services_ServiceId",
                table: "StaffServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffServices_Staff_StaffId",
                table: "StaffServices",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
