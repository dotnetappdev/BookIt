using BookIt.Core.DTOs;
using BookIt.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Web.Controllers;

public class AccountController : Controller
{
    private readonly BookItApiClient _apiClient;

    public AccountController(BookItApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult Login(string? tenantSlug = null)
    {
        // Prefer route/query parameter; fall back to subdomain detection
        var slug = tenantSlug ?? HttpContext.Items["TenantSlug"] as string;
        ViewBag.TenantSlug = slug;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request, string? returnUrl = null)
    {
        var result = await _apiClient.LoginAsync(request);
        if (result == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(request);
        }

        HttpContext.Session.SetString("AccessToken", result.AccessToken);
        HttpContext.Session.SetString("TenantSlug", result.TenantSlug);
        HttpContext.Session.SetString("UserName", result.FullName);
        HttpContext.Session.SetString("UserRole", ((int)result.Role).ToString());

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Admin", new { tenantSlug = result.TenantSlug });
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
