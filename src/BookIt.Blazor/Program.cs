using BookIt.Blazor.Components;
using BookIt.Blazor.Middleware;
using BookIt.UI.Shared;
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

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5248";

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

// Serve static web assets from RCL packages (_content/ paths, e.g. MudBlazor CSS/JS)
// in non-published mode (dotnet run, CI, Docker without publish step).
builder.WebHost.UseStaticWebAssets();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register BookIt UI (MudBlazor + API client + auth state)
builder.Services.AddBookItUI(apiBaseUrl);

// Session for storing auth state on page refresh
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o => o.IdleTimeout = TimeSpan.FromHours(8));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMiddleware<SubdomainRewriteMiddleware>();
app.UseSession();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

try
{
    Log.Information("BookIt Blazor starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "BookIt Blazor terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

