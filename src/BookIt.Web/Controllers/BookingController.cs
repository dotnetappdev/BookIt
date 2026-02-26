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
    public async Task<IActionResult> FormStep(string tenantSlug, Guid serviceId)
    {
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();
        var form = await _apiClient.GetDefaultFormAsync(tenantSlug);
        ViewBag.Tenant = tenant;
        ViewBag.TenantSlug = tenantSlug;
        ViewBag.ServiceId = serviceId;
        ViewBag.Form = form;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> FormStep(string tenantSlug, Guid serviceId, IFormCollection formData)
    {
        var answers = new Dictionary<string, string>();
        foreach (var key in formData.Keys.Where(k => k.StartsWith("field_")))
        {
            var vals = formData[key].ToArray();
            answers[key] = string.Join(", ", vals);
        }
        var json = System.Text.Json.JsonSerializer.Serialize(answers);
        HttpContext.Session.SetString("BookingFormAnswers_" + serviceId, json);
        var today = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");
        return Redirect($"/{tenantSlug}/booking/selectslot?serviceId={serviceId}&date={today}");
    }

    [HttpGet]
    public async Task<IActionResult> SelectSlot(string tenantSlug, Guid serviceId, Guid? staffId, DateOnly? date)
    {
        var selectedDate = date ?? DateOnly.FromDateTime(DateTime.Today);
        var tenant = await _apiClient.GetTenantAsync(tenantSlug);
        if (tenant == null) return NotFound();

        var staff = await _apiClient.GetStaffAsync(tenantSlug);
        var slots = await _apiClient.GetAvailableSlotsAsync(tenantSlug, serviceId, staffId, selectedDate);

        ViewBag.Tenant = tenant;
        ViewBag.ServiceId = serviceId;
        ViewBag.StaffId = staffId;
        ViewBag.Date = selectedDate;
        ViewBag.Slots = slots;
        ViewBag.Staff = staff;
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

        // Retrieve any saved form answers for this service
        var formAnswersKey = "BookingFormAnswers_" + (request.ServiceIds.Any() ? request.ServiceIds.First() : request.ServiceId);
        var savedAnswers = HttpContext.Session.GetString(formAnswersKey);
        if (!string.IsNullOrEmpty(savedAnswers))
            request.CustomFormDataJson = savedAnswers;

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
        // Fetch appointment to show PIN
        var appointment = await _apiClient.GetAppointmentByTokenAsync(tenantSlug, token);
        ViewBag.BookingPin = appointment?.BookingPin;
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

    [Route("/{tenantSlug}/interview/{token}")]
    public async Task<IActionResult> Interview(string tenantSlug, string token)
    {
        var invitation = await _apiClient.GetInterviewInvitationAsync(tenantSlug, token);
        if (invitation == null) return NotFound();

        ViewBag.Invitation = invitation;
        ViewBag.TenantSlug = tenantSlug;
        return View();
    }

    [HttpPost]
    [Route("/{tenantSlug}/interview/{token}/book")]
    public async Task<IActionResult> BookInterview(string tenantSlug, string token, BookInterviewRequest request)
    {
        var success = await _apiClient.BookInterviewSlotAsync(tenantSlug, token, request);
        if (!success)
        {
            TempData["Error"] = "Unable to book the selected slot. It may have already been taken.";
            return RedirectToAction("Interview", new { tenantSlug, token });
        }
        return RedirectToAction("InterviewConfirmation", new { tenantSlug });
    }

    [Route("/{tenantSlug}/interview/confirmed")]
    public IActionResult InterviewConfirmation(string tenantSlug)
    {
        ViewBag.TenantSlug = tenantSlug;
        return View();
    }
}
