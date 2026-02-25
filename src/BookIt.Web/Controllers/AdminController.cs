using BookIt.Core.DTOs;
using BookIt.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Web.Controllers;

public class AdminController : Controller
{
    private readonly BookItApiClient _apiClient;

    public AdminController(BookItApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    private bool IsAuthenticated() =>
        !string.IsNullOrEmpty(HttpContext.Session.GetString("AccessToken"));

    private IActionResult RequireAuth(string tenantSlug) =>
        RedirectToAction("Login", "Account", new { returnUrl = $"/{tenantSlug}/admin" });

    public async Task<IActionResult> Index(string tenantSlug)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);

        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        ViewBag.Tenant = tenant;
        ViewBag.TenantSlug = tenantSlug;
        ViewBag.UserName = HttpContext.Session.GetString("UserName");

        var now = DateTime.Now;
        var appointments = await _apiClient.GetAppointmentsAsync(tenantSlug,
            from: now.Date,
            to: now.Date.AddDays(30));

        ViewBag.Appointments = appointments;
        ViewBag.TodayAppointments = appointments.Where(a => a.StartTime.Date == now.Date).ToList();
        ViewBag.UpcomingAppointments = appointments.Where(a => a.StartTime.Date > now.Date).ToList();

        return View();
    }

    public async Task<IActionResult> Calendar(string tenantSlug)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);

        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        ViewBag.Tenant = tenant;
        ViewBag.TenantSlug = tenantSlug;

        var appointments = await _apiClient.GetAppointmentsAsync(tenantSlug,
            from: DateTime.Now.AddMonths(-1),
            to: DateTime.Now.AddMonths(3));

        ViewBag.Appointments = appointments;
        return View();
    }

    public async Task<IActionResult> Services(string tenantSlug)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);

        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var services = await _apiClient.GetServicesAsync(tenantSlug);

        ViewBag.Tenant = tenant;
        ViewBag.TenantSlug = tenantSlug;
        ViewBag.Services = services;
        return View();
    }

    public async Task<IActionResult> Settings(string tenantSlug)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);

        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        ViewBag.Tenant = tenant;
        ViewBag.TenantSlug = tenantSlug;
        return View(tenant);
    }

    [HttpPost]
    public async Task<IActionResult> Settings(string tenantSlug, UpdateTenantRequest request)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);

        await _apiClient.UpdateTenantAsync(tenantSlug, request);
        TempData["Success"] = "Settings saved successfully!";
        return RedirectToAction(nameof(Settings), new { tenantSlug });
    }

    public async Task<IActionResult> Interviews(string tenantSlug)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);

        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var services = await _apiClient.GetServicesAsync(tenantSlug);
        var slots = await _apiClient.GetInterviewSlotsAsync(tenantSlug);
        var invitations = await _apiClient.GetInterviewInvitationsAsync(tenantSlug);

        ViewBag.Tenant = tenant;
        ViewBag.TenantSlug = tenantSlug;
        ViewBag.Services = services;
        ViewBag.Slots = slots;
        ViewBag.Invitations = invitations;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateInterviewSlot(string tenantSlug, CreateInterviewSlotRequest request)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);
        await _apiClient.CreateInterviewSlotAsync(tenantSlug, request);
        TempData["Success"] = "Interview slot created successfully.";
        return RedirectToAction("Interviews", new { tenantSlug });
    }

    [HttpPost]
    public async Task<IActionResult> SendInterviewInvitation(string tenantSlug, SendInvitationRequest request)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);
        var result = await _apiClient.SendInterviewInvitationAsync(tenantSlug, request);
        if (result != null)
            TempData["Success"] = $"Invitation sent to {request.CandidateEmail}. Booking link: {result.BookingUrl}";
        else
            TempData["Error"] = "Failed to send invitation.";
        return RedirectToAction("Interviews", new { tenantSlug });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteInterviewSlot(string tenantSlug, Guid slotId)
    {
        if (!IsAuthenticated()) return RequireAuth(tenantSlug);
        await _apiClient.DeleteInterviewSlotAsync(tenantSlug, slotId);
        TempData["Success"] = "Slot removed.";
        return RedirectToAction("Interviews", new { tenantSlug });
    }
}
