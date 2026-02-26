using BookIt.Blazor.Components;
using BookIt.UI.Shared;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5248";

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
// UseStaticFiles is required to serve _content/ paths from RCL packages (MudBlazor CSS/JS)
app.UseStaticFiles();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

