using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookIt.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedEmailDomains : Migration
    {
        // Replace potentially live domains with RFC 6761 reserved .example TLD
        // so that seed data e-mail addresses can never route to real inboxes.

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Users table ──────────────────────────────────────────────────────

            // admin@demo-barber.com → admin@demo-barber.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'admin@demo-barber.example',
                NormalizedEmail = 'ADMIN@DEMO-BARBER.EXAMPLE',
                UserName = 'admin@demo-barber.example',
                NormalizedUserName = 'ADMIN@DEMO-BARBER.EXAMPLE'
                WHERE Id = 'bb000000-0000-0000-0000-000000000001'");

            // manager@demo-barber.com → manager@demo-barber.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'manager@demo-barber.example',
                NormalizedEmail = 'MANAGER@DEMO-BARBER.EXAMPLE',
                UserName = 'manager@demo-barber.example',
                NormalizedUserName = 'MANAGER@DEMO-BARBER.EXAMPLE'
                WHERE Id = 'bb000000-0000-0000-0000-000000000004'");

            // staff@demo-barber.com → staff@demo-barber.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'staff@demo-barber.example',
                NormalizedEmail = 'STAFF@DEMO-BARBER.EXAMPLE',
                UserName = 'staff@demo-barber.example',
                NormalizedUserName = 'STAFF@DEMO-BARBER.EXAMPLE'
                WHERE Id = 'bb000000-0000-0000-0000-000000000002'");

            // customer@example.com → customer@bookit-demo.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'customer@bookit-demo.example',
                NormalizedEmail = 'CUSTOMER@BOOKIT-DEMO.EXAMPLE',
                UserName = 'customer@bookit-demo.example',
                NormalizedUserName = 'CUSTOMER@BOOKIT-DEMO.EXAMPLE'
                WHERE Id = 'bb000000-0000-0000-0000-000000000003'");

            // james@elitehair.com → james@elitehair.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'james@elitehair.example',
                NormalizedEmail = 'JAMES@ELITEHAIR.EXAMPLE',
                UserName = 'james@elitehair.example',
                NormalizedUserName = 'JAMES@ELITEHAIR.EXAMPLE'
                WHERE Id = 'ee000000-0000-0000-0000-000000000001'");

            // emma@elitehair.com → emma@elitehair.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'emma@elitehair.example',
                NormalizedEmail = 'EMMA@ELITEHAIR.EXAMPLE',
                UserName = 'emma@elitehair.example',
                NormalizedUserName = 'EMMA@ELITEHAIR.EXAMPLE'
                WHERE Id = 'ee000000-0000-0000-0000-000000000002'");

            // oliver@urbanstyle.com → oliver@urbanstyle.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'oliver@urbanstyle.example',
                NormalizedEmail = 'OLIVER@URBANSTYLE.EXAMPLE',
                UserName = 'oliver@urbanstyle.example',
                NormalizedUserName = 'OLIVER@URBANSTYLE.EXAMPLE'
                WHERE Id = 'ee000000-0000-0000-0000-000000000003'");

            // sarah@elitehair.com → sarah@elitehair.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'sarah@elitehair.example',
                NormalizedEmail = 'SARAH@ELITEHAIR.EXAMPLE',
                UserName = 'sarah@elitehair.example',
                NormalizedUserName = 'SARAH@ELITEHAIR.EXAMPLE'
                WHERE Id = 'ee000000-0000-0000-0000-000000000004'");

            // michael@urbanstyle.com → michael@urbanstyle.example
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'michael@urbanstyle.example',
                NormalizedEmail = 'MICHAEL@URBANSTYLE.EXAMPLE',
                UserName = 'michael@urbanstyle.example',
                NormalizedUserName = 'MICHAEL@URBANSTYLE.EXAMPLE'
                WHERE Id = 'ee000000-0000-0000-0000-000000000005'");

            // ── Staff table ──────────────────────────────────────────────────────
            migrationBuilder.Sql("UPDATE Staff SET Email = 'staff@demo-barber.example' WHERE Id = 'cc000000-0000-0000-0000-000000000001'");
            migrationBuilder.Sql("UPDATE Staff SET Email = 'james@elitehair.example' WHERE Id = 'cc000000-0000-0000-0000-000000000002'");
            migrationBuilder.Sql("UPDATE Staff SET Email = 'emma@elitehair.example' WHERE Id = 'cc000000-0000-0000-0000-000000000003'");
            migrationBuilder.Sql("UPDATE Staff SET Email = 'oliver@urbanstyle.example' WHERE Id = 'cc000000-0000-0000-0000-000000000004'");

            // ── Clients table ────────────────────────────────────────────────────
            migrationBuilder.Sql("UPDATE Clients SET Email = 'sarah@elitehair.example' WHERE Id = 'ff000000-0000-0000-0000-000000000001'");
            migrationBuilder.Sql("UPDATE Clients SET Email = 'michael@urbanstyle.example' WHERE Id = 'ff000000-0000-0000-0000-000000000002'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert Users to original domains
            migrationBuilder.Sql(@"UPDATE Users SET Email = 'admin@demo-barber.com',
                NormalizedEmail = 'ADMIN@DEMO-BARBER.COM',
                UserName = 'admin@demo-barber.com',
                NormalizedUserName = 'ADMIN@DEMO-BARBER.COM'
                WHERE Id = 'bb000000-0000-0000-0000-000000000001'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'manager@demo-barber.com',
                NormalizedEmail = 'MANAGER@DEMO-BARBER.COM',
                UserName = 'manager@demo-barber.com',
                NormalizedUserName = 'MANAGER@DEMO-BARBER.COM'
                WHERE Id = 'bb000000-0000-0000-0000-000000000004'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'staff@demo-barber.com',
                NormalizedEmail = 'STAFF@DEMO-BARBER.COM',
                UserName = 'staff@demo-barber.com',
                NormalizedUserName = 'STAFF@DEMO-BARBER.COM'
                WHERE Id = 'bb000000-0000-0000-0000-000000000002'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'customer@example.com',
                NormalizedEmail = 'CUSTOMER@EXAMPLE.COM',
                UserName = 'customer@example.com',
                NormalizedUserName = 'CUSTOMER@EXAMPLE.COM'
                WHERE Id = 'bb000000-0000-0000-0000-000000000003'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'james@elitehair.com',
                NormalizedEmail = 'JAMES@ELITEHAIR.COM',
                UserName = 'james@elitehair.com',
                NormalizedUserName = 'JAMES@ELITEHAIR.COM'
                WHERE Id = 'ee000000-0000-0000-0000-000000000001'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'emma@elitehair.com',
                NormalizedEmail = 'EMMA@ELITEHAIR.COM',
                UserName = 'emma@elitehair.com',
                NormalizedUserName = 'EMMA@ELITEHAIR.COM'
                WHERE Id = 'ee000000-0000-0000-0000-000000000002'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'oliver@urbanstyle.com',
                NormalizedEmail = 'OLIVER@URBANSTYLE.COM',
                UserName = 'oliver@urbanstyle.com',
                NormalizedUserName = 'OLIVER@URBANSTYLE.COM'
                WHERE Id = 'ee000000-0000-0000-0000-000000000003'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'sarah@elitehair.com',
                NormalizedEmail = 'SARAH@ELITEHAIR.COM',
                UserName = 'sarah@elitehair.com',
                NormalizedUserName = 'SARAH@ELITEHAIR.COM'
                WHERE Id = 'ee000000-0000-0000-0000-000000000004'");

            migrationBuilder.Sql(@"UPDATE Users SET Email = 'michael@urbanstyle.com',
                NormalizedEmail = 'MICHAEL@URBANSTYLE.COM',
                UserName = 'michael@urbanstyle.com',
                NormalizedUserName = 'MICHAEL@URBANSTYLE.COM'
                WHERE Id = 'ee000000-0000-0000-0000-000000000005'");

            // Revert Staff
            migrationBuilder.Sql("UPDATE Staff SET Email = 'staff@demo-barber.com' WHERE Id = 'cc000000-0000-0000-0000-000000000001'");
            migrationBuilder.Sql("UPDATE Staff SET Email = 'james@elitehair.com' WHERE Id = 'cc000000-0000-0000-0000-000000000002'");
            migrationBuilder.Sql("UPDATE Staff SET Email = 'emma@elitehair.com' WHERE Id = 'cc000000-0000-0000-0000-000000000003'");
            migrationBuilder.Sql("UPDATE Staff SET Email = 'oliver@urbanstyle.com' WHERE Id = 'cc000000-0000-0000-0000-000000000004'");

            // Revert Clients
            migrationBuilder.Sql("UPDATE Clients SET Email = 'sarah@elitehair.com' WHERE Id = 'ff000000-0000-0000-0000-000000000001'");
            migrationBuilder.Sql("UPDATE Clients SET Email = 'michael@urbanstyle.com' WHERE Id = 'ff000000-0000-0000-0000-000000000002'");
        }
    }
}
