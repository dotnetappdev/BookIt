# MySQL Migrations

This folder contains EF Core migrations for the **MySQL** provider.

## Generating migrations

Run the following commands from the `BookIt.Infrastructure.MySql` project directory:

```bash
# Generate the initial migration (first-time setup)
dotnet ef migrations add InitialCreate --startup-project ../BookIt.API

# Apply the migration to a MySQL database
dotnet ef database update --startup-project ../BookIt.API
```

## Connection string

Set the `DefaultConnection` connection string in your application's `appsettings.json` to a valid MySQL connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=BookItDb;User=root;Password=your_password"
  }
}
```

## Wiring up in Program.cs / DI

Replace `AddInfrastructure` with `AddInfrastructureWithMySql` in your host startup:

```csharp
using BookIt.Infrastructure.MySql;

// In Program.cs or Startup.cs
builder.Services.AddInfrastructureWithMySql(builder.Configuration);
```
