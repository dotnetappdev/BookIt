using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using BookIt.Infrastructure.Repositories;
using BookIt.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookIt.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment? environment = null)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var useSqlite = string.IsNullOrEmpty(connectionString) || connectionString.Contains("localdb", StringComparison.OrdinalIgnoreCase);

        if (useSqlite)
        {
            // Use SQLite as a file-based database for development/demo when no SQL Server is configured
            var dbPath = Path.Combine(AppContext.BaseDirectory, "bookit.db");
            services.AddDbContext<BookItDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));
        }
        else
        {
            services.AddDbContext<BookItDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddHttpContextAccessor();
        services.AddScoped<ITenantContext, HttpTenantContext>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPaymentService, StripePaymentService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
