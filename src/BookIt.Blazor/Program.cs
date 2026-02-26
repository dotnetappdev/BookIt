using BookIt.Blazor.Components;
using BookIt.UI.Shared;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5248";

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
app.UseSession();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

