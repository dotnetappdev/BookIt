using BookIt.Core.DTOs;
using BookIt.Core.Enums;
using BookIt.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.Web.Controllers;

public class BookingController : Controller
{
    private readonly BookItApiClient _apiClient;

    public BookingController(BookItApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(string tenantSlug)
    {
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var services = await _apiClient.GetServicesAsync(tenantSlug);

        ViewBag.Tenant = tenant;
        ViewBag.Services = services;
        ViewBag.TenantSlug = tenantSlug;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> SelectSlot(string tenantSlug, Guid serviceId, Guid? staffId, DateOnly? date)
    {
        var selectedDate = date ?? DateOnly.FromDateTime(DateTime.Today);
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var slots = await _apiClient.GetAvailableSlotsAsync(tenantSlug, serviceId, staffId, selectedDate);

        ViewBag.Tenant = tenant;
        ViewBag.ServiceId = serviceId;
        ViewBag.StaffId = staffId;
        ViewBag.Date = selectedDate;
        ViewBag.Slots = slots;
        ViewBag.TenantSlug = tenantSlug;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Details(string tenantSlug, [FromQuery] Guid[] serviceIds, [FromQuery] Guid? staffId, [FromQuery] DateTime startTime)
    {
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        ViewBag.Tenant = tenant;
        ViewBag.ServiceIds = serviceIds;
        ViewBag.StaffId = staffId;
        ViewBag.StartTime = startTime;
        ViewBag.TenantSlug = tenantSlug;
        ViewBag.MeetingTypes = Enum.GetValues<MeetingType>();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string tenantSlug, CreateAppointmentRequest request)
    {
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        request.TenantId = tenant.Id;
        var appointment = await _apiClient.CreateAppointmentAsync(tenantSlug, request);

        if (appointment == null)
        {
            TempData["Error"] = "Failed to create appointment. Please try again.";
            return RedirectToAction(nameof(Index), new { tenantSlug });
        }

        return RedirectToAction(nameof(Confirmation), new { tenantSlug, token = appointment.ConfirmationToken });
    }

    public async Task<IActionResult> Confirmation(string tenantSlug, string token)
    {
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        ViewBag.Tenant = tenant;
        ViewBag.Token = token;
        ViewBag.TenantSlug = tenantSlug;
        return View();
    }

    /// <summary>
    /// Embeddable widget endpoint - returns a minimal HTML booking widget 
    /// designed to be inserted into 3rd party websites via iframe or JS
    /// </summary>
    public async Task<IActionResult> Widget(string tenantSlug)
    {
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var services = await _apiClient.GetServicesAsync(tenantSlug);

        ViewBag.Tenant = tenant;
        ViewBag.Services = services;
        ViewBag.TenantSlug = tenantSlug;
        return View("Widget");
    }
}
