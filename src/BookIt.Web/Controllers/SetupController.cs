using BookIt.Core.DTOs;
using BookIt.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Web.Controllers;

public class SetupController : Controller
{
    private readonly BookItApiClient _apiClient;

    public SetupController(BookItApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpPost]
    public async Task<IActionResult> Create(TenantSetupRequest request)
    {
        var result = await _apiClient.SetupTenantAsync(request);
        if (result == null)
        {
            TempData["Error"] = "Failed to create tenant. The business name may already be taken.";
            return RedirectToAction("Setup", "Home");
        }

        HttpContext.Session.SetString("AccessToken", result.AccessToken);
        HttpContext.Session.SetString("TenantSlug", result.TenantSlug);
        HttpContext.Session.SetString("UserName", result.FullName);
        HttpContext.Session.SetString("UserRole", ((int)result.Role).ToString());

        TempData["Success"] = $"Welcome to BookIt, {result.FullName}! Your booking page is ready.";
        return RedirectToAction("Index", "Admin", new { tenantSlug = result.TenantSlug });
    }
}
