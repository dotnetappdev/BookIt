using BookIt.Web.Filters;
using BookIt.Web.Middleware;
using BookIt.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SubdomainTenantFilter>();
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001";
builder.Services.AddHttpClient<BookItApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<SubdomainTenantMiddleware>();
app.UseSession();
app.UseAuthorization();

// ── Subdomain-based routes (tenantSlug resolved from subdomain by middleware) ──
app.MapControllerRoute(
    name: "subdomainBook",
    pattern: "book",
    defaults: new { controller = "Booking", action = "Index" });

app.MapControllerRoute(
    name: "subdomainServiceBook",
    pattern: "book/{serviceSlug}",
    defaults: new { controller = "Booking", action = "ServiceLanding" });

app.MapControllerRoute(
    name: "subdomainAdmin",
    pattern: "admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

app.MapControllerRoute(
    name: "subdomainLogin",
    pattern: "login",
    defaults: new { controller = "Account", action = "Login" });

// ── Path-based routes (legacy / non-subdomain access) ──

app.MapControllerRoute(
    name: "tenant",
    pattern: "{tenantSlug}/book",
    defaults: new { controller = "Booking", action = "Index" });

app.MapControllerRoute(
    name: "tenantServiceBook",
    pattern: "{tenantSlug}/book/{serviceSlug}",
    defaults: new { controller = "Booking", action = "ServiceLanding" });

app.MapControllerRoute(
    name: "tenantAdmin",
    pattern: "{tenantSlug}/admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
