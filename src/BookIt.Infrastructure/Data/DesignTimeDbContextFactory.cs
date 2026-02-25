using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookIt.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookItDbContext>
{
    public BookItDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookItDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BookItDb;Trusted_Connection=true;");
        return new BookItDbContext(optionsBuilder.Options);
    }
}
