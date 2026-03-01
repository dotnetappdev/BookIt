using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookIt.Infrastructure.MySql;

/// <summary>
/// Used by the EF Core tooling (dotnet-ef) at design time to create migrations
/// for the MySQL provider.
///
/// To scaffold the initial migration run from the <c>BookIt.Infrastructure.MySql</c> directory:
/// <code>
/// dotnet ef migrations add InitialCreate --startup-project ../BookIt.API
/// </code>
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookItDbContext>
{
    public BookItDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookItDbContext>();
        optionsBuilder.UseMySQL(
            "Server=localhost;Port=3306;Database=BookItDb;User=root;Password=your_password_here",
            opts => opts.MigrationsAssembly("BookIt.Infrastructure.MySql"));

        return new BookItDbContext(optionsBuilder.Options);
    }
}
