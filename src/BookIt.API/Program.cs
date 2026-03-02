using System.Text;
using BookIt.API.Middleware;
using BookIt.API.Services;
using BookIt.Core.Enums;
using BookIt.Infrastructure;
using BookIt.Infrastructure.Data;
using BookIt.Infrastructure.MySql;
using BookIt.Infrastructure.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

// Build a startup-time log path: logs/{yyyy}/{MM}/{dd}/{HH-mm-ss}.log
var startupTime = DateTime.Now;
var logPath = Path.Combine("logs",
    startupTime.ToString("yyyy"),
    startupTime.ToString("MM"),
    startupTime.ToString("dd"),
    $"{startupTime:HH-mm-ss}.log");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Infinite,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Infrastructure (EF Core, services) — provider selected by Database:Provider in appsettings
var dbProvider = builder.Configuration["Database:Provider"] ?? "SqlServer";
switch (dbProvider.ToLowerInvariant())
{
    case "postgresql":
    case "postgres":
        builder.Services.AddInfrastructureWithPostgreSql(builder.Configuration,
            connectionStringName: builder.Configuration["ConnectionStrings:PostgreSqlConnection"] is not null
                ? "PostgreSqlConnection"
                : "DefaultConnection");
        break;
    case "mysql":
        builder.Services.AddInfrastructureWithMySql(builder.Configuration,
            connectionStringName: builder.Configuration["ConnectionStrings:MySqlConnection"] is not null
                ? "MySqlConnection"
                : "DefaultConnection");
        break;
    default: // SqlServer — handled inside AddInfrastructure (SQLite is not supported in the API; use SqlServer, PostgreSql, or MySql)
        builder.Services.AddInfrastructure(builder.Configuration);
        break;
}

// Sentry error monitoring (only active when Dsn is configured)
var sentryDsn = builder.Configuration["Sentry:Dsn"];
if (!string.IsNullOrEmpty(sentryDsn))
{
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = sentryDsn;
        o.TracesSampleRate = double.TryParse(builder.Configuration["Sentry:TracesSampleRate"], out var rate) ? rate : 0.1;
        o.Environment = builder.Environment.EnvironmentName;
        o.SendDefaultPii = false;
    });
}

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "BookItDefaultSecretKey-ChangeInProduction-32chars!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "BookIt.API";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "BookIt.Web";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy => policy.RequireClaim("role", ((int)UserRole.SuperAdmin).ToString()));
    options.AddPolicy("TenantAdmin", policy => policy.RequireClaim("role",
        ((int)UserRole.TenantAdmin).ToString(),
        ((int)UserRole.SuperAdmin).ToString()));
    options.AddPolicy("Manager", policy => policy.RequireClaim("role",
        ((int)UserRole.Manager).ToString(),
        ((int)UserRole.TenantAdmin).ToString(),
        ((int)UserRole.SuperAdmin).ToString()));
    options.AddPolicy("Staff", policy => policy.RequireClaim("role",
        ((int)UserRole.Staff).ToString(),
        ((int)UserRole.Manager).ToString(),
        ((int)UserRole.TenantAdmin).ToString(),
        ((int)UserRole.SuperAdmin).ToString()));
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDatabaseSeederService, DatabaseSeederService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookIt API", Version = "v1", Description = "Multi-tenant booking CRM API" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// CORS - allow Web project and embedded widget usage
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookItDbContext>();
    try
    {
        // Run all pending migrations at startup — this also applies seed data defined in migrations
        await db.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("Database initialization failed: {Message}. The app will run without database support.", ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookIt API v1"));
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("BookIt API starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "BookIt API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
