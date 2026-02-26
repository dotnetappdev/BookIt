using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BookIt.Core.DTOs;

namespace BookIt.UI.Shared.Services;

/// <summary>
/// HTTP API client for BookIt API. Stateless — token must be passed per-call.
/// Compatible with Blazor Server, Blazor WASM, and .NET MAUI.
/// </summary>
public class BookItApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public BookItApiService(HttpClient http)
    {
        _http = http;
    }

    public void SetToken(string? token)
    {
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        else
            _http.DefaultRequestHeaders.Authorization = null;
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        var r = await _http.GetAsync(url);
        if (!r.IsSuccessStatusCode) return default;
        return JsonSerializer.Deserialize<T>(await r.Content.ReadAsStringAsync(), _json);
    }

    private async Task<T?> PostAsync<T>(string url, object body)
    {
        var r = await _http.PostAsync(url, Json(body));
        if (!r.IsSuccessStatusCode) return default;
        return JsonSerializer.Deserialize<T>(await r.Content.ReadAsStringAsync(), _json);
    }

    private async Task<T?> PutAsync<T>(string url, object body)
    {
        var r = await _http.PutAsync(url, Json(body));
        if (!r.IsSuccessStatusCode) return default;
        return JsonSerializer.Deserialize<T>(await r.Content.ReadAsStringAsync(), _json);
    }

    private async Task<bool> PutAsync(string url, object body)
    {
        var r = await _http.PutAsync(url, Json(body));
        return r.IsSuccessStatusCode;
    }

    private async Task<bool> DeleteAsync(string url)
    {
        var r = await _http.DeleteAsync(url);
        return r.IsSuccessStatusCode;
    }

    private static StringContent Json(object body) =>
        new(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

    // ── Auth ──
    public Task<AuthResponse?> LoginAsync(LoginRequest req) =>
        PostAsync<AuthResponse>("/api/auth/login", req);

    public Task<AuthResponse?> SetupTenantAsync(TenantSetupRequest req) =>
        PostAsync<AuthResponse>("/api/auth/setup", req);

    // ── Tenant ──
    public Task<TenantResponse?> GetTenantAsync(string slug) =>
        GetAsync<TenantResponse>($"/api/tenants/{slug}");

    public Task<TenantResponse?> UpdateTenantAsync(string slug, UpdateTenantRequest req) =>
        PutAsync<TenantResponse>($"/api/tenants/{slug}", req);

    // ── Services ──
    public Task<List<ServiceResponse>?> GetServicesAsync(string slug) =>
        GetAsync<List<ServiceResponse>>($"/api/tenants/{slug}/services");

    public Task<ServiceResponse?> CreateServiceAsync(string slug, CreateServiceRequest req) =>
        PostAsync<ServiceResponse>($"/api/tenants/{slug}/services", req);

    public Task<bool> DeleteServiceAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/services/{id}");

    // ── Appointments ──
    public async Task<List<AppointmentResponse>> GetAppointmentsAsync(
        string slug, DateTime? from = null, DateTime? to = null)
    {
        var url = $"/api/tenants/{slug}/appointments";
        var qs = new List<string>();
        if (from.HasValue) qs.Add($"from={from.Value:yyyy-MM-ddTHH:mm:ss}");
        if (to.HasValue) qs.Add($"to={to.Value:yyyy-MM-ddTHH:mm:ss}");
        if (qs.Count > 0) url += "?" + string.Join("&", qs);
        return await GetAsync<List<AppointmentResponse>>(url) ?? new();
    }

    public Task<AppointmentResponse?> CreateAppointmentAsync(string slug, CreateAppointmentRequest req) =>
        PostAsync<AppointmentResponse>($"/api/tenants/{slug}/appointments", req);

    public Task<List<DateTime>?> GetAvailableSlotsAsync(string slug, Guid serviceId, Guid? staffId, DateOnly date)
    {
        var url = $"/api/tenants/{slug}/appointments/slots?serviceId={serviceId}&date={date:yyyy-MM-dd}";
        if (staffId.HasValue) url += $"&staffId={staffId}";
        return GetAsync<List<DateTime>>(url);
    }

    // ── Forms ──
    public Task<List<BookingFormResponse>?> GetFormsAsync(string slug) =>
        GetAsync<List<BookingFormResponse>>($"/api/tenants/{slug}/booking-forms");

    public Task<BookingFormResponse?> GetFormAsync(string slug, Guid formId) =>
        GetAsync<BookingFormResponse>($"/api/tenants/{slug}/booking-forms/{formId}");

    public Task<BookingFormResponse?> CreateFormAsync(string slug, CreateBookingFormRequest req) =>
        PostAsync<BookingFormResponse>($"/api/tenants/{slug}/booking-forms", req);

    public Task<bool> DeleteFormAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/booking-forms/{id}");

    public Task<BookingFormFieldResponse?> AddFormFieldAsync(string slug, Guid formId, AddFormFieldRequest req) =>
        PostAsync<BookingFormFieldResponse>($"/api/tenants/{slug}/booking-forms/{formId}/fields", req);

    public Task<bool> DeleteFormFieldAsync(string slug, Guid formId, Guid fieldId) =>
        DeleteAsync($"/api/tenants/{slug}/booking-forms/{formId}/fields/{fieldId}");

    public Task<bool> ReorderFormFieldsAsync(string slug, Guid formId, ReorderFieldsRequest req) =>
        PutAsync($"/api/tenants/{slug}/booking-forms/{formId}/fields/reorder", req);

    // ── Interview Slots ──
    public Task<List<InterviewSlotResponse>?> GetInterviewSlotsAsync(string slug) =>
        GetAsync<List<InterviewSlotResponse>>($"/api/tenants/{slug}/interviewslots");

    public Task<InterviewSlotResponse?> CreateInterviewSlotAsync(string slug, CreateInterviewSlotRequest req) =>
        PostAsync<InterviewSlotResponse>($"/api/tenants/{slug}/interviewslots", req);

    public Task<bool> DeleteInterviewSlotAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/interviewslots/{id}");

    public Task<List<CandidateInvitationResponse>?> GetInterviewInvitationsAsync(string slug) =>
        GetAsync<List<CandidateInvitationResponse>>($"/api/tenants/{slug}/interviewslots/invitations");

    // ── Customers ──
    public Task<List<CustomerResponse>?> GetCustomersAsync(string slug) =>
        GetAsync<List<CustomerResponse>>($"/api/tenants/{slug}/customers");

    public Task<CustomerResponse?> GetCustomerAsync(string slug, Guid id) =>
        GetAsync<CustomerResponse>($"/api/tenants/{slug}/customers/{id}");

    public Task<CustomerResponse?> CreateCustomerAsync(string slug, CreateCustomerRequest req) =>
        PostAsync<CustomerResponse>($"/api/tenants/{slug}/customers", req);

    public Task<CustomerResponse?> UpdateCustomerAsync(string slug, Guid id, UpdateCustomerRequest req) =>
        PutAsync<CustomerResponse>($"/api/tenants/{slug}/customers/{id}", req);

    public Task<bool> DeleteCustomerAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/customers/{id}");

    // ── Super Admin ──
    public Task<List<TenantListResponse>?> GetAllTenantsAsync() =>
        GetAsync<List<TenantListResponse>>("/api/admin/tenants");

    public Task<TenantResponse?> SuperAdminUpdateTenantAsync(Guid tenantId, UpdateTenantRequest req) =>
        PutAsync<TenantResponse>($"/api/admin/tenants/{tenantId}", req);

    public Task<bool> SuperAdminDeleteTenantAsync(Guid tenantId) =>
        DeleteAsync($"/api/admin/tenants/{tenantId}");

    // ── Email Templates ──
    public Task<List<EmailTemplateResponse>?> GetEmailTemplatesAsync(string slug) =>
        GetAsync<List<EmailTemplateResponse>>($"/api/tenants/{slug}/email-templates");

    public Task<EmailTemplateResponse?> CreateEmailTemplateAsync(string slug, UpsertEmailTemplateRequest req) =>
        PostAsync<EmailTemplateResponse>($"/api/tenants/{slug}/email-templates", req);

    public Task<EmailTemplateResponse?> UpdateEmailTemplateAsync(string slug, Guid id, UpsertEmailTemplateRequest req) =>
        PutAsync<EmailTemplateResponse>($"/api/tenants/{slug}/email-templates/{id}", req);

    public Task<bool> DeleteEmailTemplateAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/email-templates/{id}");
}
