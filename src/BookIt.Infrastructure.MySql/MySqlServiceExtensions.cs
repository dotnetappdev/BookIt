using BookIt.Infrastructure;
using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookIt.Infrastructure.MySql;

/// <summary>
/// Extension methods for registering BookIt infrastructure using MySQL as the database provider.
/// </summary>
public static class MySqlServiceExtensions
{
    /// <summary>
    /// Registers the <see cref="BookItDbContext"/> using MySQL (MySql.EntityFrameworkCore) and wires up all
    /// BookIt application services (Identity, repositories, payments, notifications, etc.).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Application configuration used to resolve the connection string and shared settings.</param>
    /// <param name="connectionStringName">Name of the connection string entry in configuration (defaults to "DefaultConnection").</param>
    public static IServiceCollection AddInfrastructureWithMySql(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = "DefaultConnection")
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException(
                $"MySQL connection string '{connectionStringName}' was not found in application configuration.");

        services.AddDbContext<BookItDbContext>(options =>
            options.UseMySQL(connectionString, mysqlOptions =>
                mysqlOptions.MigrationsAssembly("BookIt.Infrastructure.MySql")));

        return services.AddInfrastructureServices(configuration);
    }
}
