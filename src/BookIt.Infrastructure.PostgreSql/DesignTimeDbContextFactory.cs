using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookIt.Infrastructure.PostgreSql;

/// <summary>
/// Used by the EF Core tooling (dotnet-ef) at design time to create migrations
/// for the PostgreSQL provider.
///
/// To scaffold the initial migration run from the <c>BookIt.Infrastructure.PostgreSql</c> directory:
/// <code>
/// dotnet ef migrations add InitialCreate --startup-project ../BookIt.API
/// </code>
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookItDbContext>
{
    public BookItDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookItDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=BookItDb;Username=postgres;Password=your_password_here",
            opts => opts.MigrationsAssembly("BookIt.Infrastructure.PostgreSql"));

        return new BookItDbContext(optionsBuilder.Options);
    }
}
