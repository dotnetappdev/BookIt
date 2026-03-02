# PostgreSQL Migrations

This folder contains EF Core migrations for the **PostgreSQL** provider.

## Generating migrations

Run the following commands from the `BookIt.Infrastructure.PostgreSql` project directory:

```bash
# Generate the initial migration (first-time setup)
dotnet ef migrations add InitialCreate --startup-project ../BookIt.API

# Apply the migration to a PostgreSQL database
dotnet ef database update --startup-project ../BookIt.API
```

## Connection string

Set the `DefaultConnection` connection string in your application's `appsettings.json` to a valid PostgreSQL connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BookItDb;Username=postgres;Password=your_password"
  }
}
```

## Wiring up in Program.cs / DI

Replace `AddInfrastructure` with `AddInfrastructureWithPostgreSql` in your host startup:

```csharp
using BookIt.Infrastructure.PostgreSql;

// In Program.cs or Startup.cs
builder.Services.AddInfrastructureWithPostgreSql(builder.Configuration);
```
