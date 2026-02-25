# EF Core Migrations & Database Seeding

This guide covers creating migrations, applying them, and understanding the seed data bootstrapped when the app starts.

---

## Prerequisites

Install the EF Core CLI tool if you haven't already:

```bash
dotnet tool install --global dotnet-ef
```

Verify:

```bash
dotnet ef --version
```

---

## Project Layout

| Project | Role |
|---|---|
| `BookIt.Infrastructure` | Houses `BookItDbContext`, all entity configurations, and the `Migrations/` folder |
| `BookIt.API` | Startup project — EF uses its configuration (connection string, DI) when running migrations |

---

## Creating a Migration

Always run `dotnet ef` from the **solution root** (or pass `--project` / `--startup-project` flags):

```bash
# From solution root
dotnet ef migrations add <MigrationName> \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```

Example:

```bash
dotnet ef migrations add AddInterviewSlots \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```

This generates three files inside `src/BookIt.Infrastructure/Data/Migrations/`:

| File | Purpose |
|---|---|
| `<timestamp>_<Name>.cs` | Up/Down migration logic |
| `<timestamp>_<Name>.Designer.cs` | EF model snapshot metadata |
| `BookItDbContextModelSnapshot.cs` | Full model snapshot (updated automatically) |

---

## Applying Migrations

### Option A — CLI (recommended for production)

```bash
dotnet ef database update \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```

This runs all pending migrations against the connection string in `BookIt.API/appsettings.json`.

### Option B — `EnsureCreated` (used in development)

The app calls `context.Database.EnsureCreated()` inside `BookItDbContext.SeedDataAsync()` on first run. This creates the schema directly from the model **without** running migration files. It is used automatically when SQLite fallback is active (no real SQL Server connection string configured).

> **Note:** `EnsureCreated` and `Migrate()` are mutually exclusive. Switch to `context.Database.Migrate()` in production to support incremental schema updates.

---

## Rolling Back a Migration

Remove the most recent unapplied migration:

```bash
dotnet ef migrations remove \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```

Revert the database to a named migration:

```bash
dotnet ef database update <PreviousMigrationName> \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```

---

## Connection Strings

### SQLite (default / development)

No configuration needed. If `ConnectionStrings:DefaultConnection` is absent or contains `localdb`, the app automatically falls back to SQLite:

```
bookit.db    # created in the API working directory
```

### SQL Server (staging / production)

Set the connection string in `appsettings.json` or as an environment variable:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=myserver;Database=BookItDb;User Id=sa;Password=yourpassword;"
  }
}
```

Or via environment variable (overrides appsettings):

```bash
export ConnectionStrings__DefaultConnection="Server=myserver;Database=BookItDb;..."
```

---

## Seed Data

Seed data is applied in `BookItDbContext.SeedDataAsync()` (called from `Program.cs` at startup). It is **idempotent** — safe to run on every startup.

### What gets seeded

| Category | Data |
|---|---|
| **Identity Roles** | `SuperAdmin`, `TenantAdmin`, `Staff`, `Customer` |
| **Demo Tenant** | "Demo Barber Shop" (`slug: demo-barber`) |
| **Demo Users** | See table below |
| **Services** | Mens Haircut, Beard Trim, Hair & Beard Combo, Kids Haircut |
| **Business Hours** | Mon–Sat 09:00–18:00, Sunday closed |
| **Booking Form** | Default form with Name, Email, Phone fields |

### Demo Users

| Email | Password | Role |
|---|---|---|
| `admin@demo-barber.com` | `Admin123!` | TenantAdmin |
| `staff@demo-barber.com` | `Staff123!` | Staff |
| `customer@example.com` | `Customer123!` | Customer |

Passwords are hashed using ASP.NET Identity's PBKDF2 (via `UserManager<ApplicationUser>`) — never stored in plain text.

---

## Role Enum

Roles are defined in `BookIt.Core.Enums`:

```csharp
public enum UserRole
{
    SuperAdmin  = 1,
    TenantAdmin = 2,
    Staff       = 3,
    Customer    = 4,
}
```

The numeric value is included as a `role` claim in the JWT. Use the enum to avoid magic numbers:

```csharp
// Good
if (roleClaim == ((int)UserRole.SuperAdmin).ToString()) { ... }

// Avoid
if (roleClaim == "1") { ... }
```

---

## Full Reset (development only)

To wipe the SQLite database and re-seed from scratch:

```bash
# Delete the SQLite file
rm src/BookIt.API/bookit.db

# Restart the app — EnsureCreated + SeedDataAsync will run automatically
dotnet run --project src/BookIt.API
```

For SQL Server, drop and recreate:

```bash
dotnet ef database drop \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API

dotnet ef database update \
  --project src/BookIt.Infrastructure \
  --startup-project src/BookIt.API
```
