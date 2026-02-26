using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Services;
using BookIt.Infrastructure.Data;
using BookIt.Infrastructure.Repositories;
using BookIt.Payments.Stripe;
using BookIt.Payments.PayPal;
using Microsoft.AspNetCore.Identity;
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
            var dbPath = Path.Combine(AppContext.BaseDirectory, "bookit.db");
            services.AddDbContext<BookItDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));
        }
        else
        {
            services.AddDbContext<BookItDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        // ASP.NET Identity
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<BookItDbContext>()
        .AddDefaultTokenProviders();

        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddScoped<ITenantContext, HttpTenantContext>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IAppointmentService, BookIt.Infrastructure.Services.AppointmentService>();
        services.AddStripePayments();
        services.AddPayPalPayments();
        services.AddScoped<IPaymentService, StripePaymentService>();
        services.AddScoped<IPayPalService, PayPalService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IEmailService, ConsoleEmailService>();
        services.AddScoped<IChatService, BookingChatService>();

        return services;
    }
}
