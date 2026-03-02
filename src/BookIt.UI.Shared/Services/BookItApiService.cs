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

    public Task<AuthResponse?> RegisterAsync(RegisterRequest req) =>
        PostAsync<AuthResponse>("/api/auth/register", req);

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
        string slug, DateTime? from = null, DateTime? to = null, Guid? staffId = null)
    {
        var url = $"/api/tenants/{slug}/appointments";
        var qs = new List<string>();
        if (from.HasValue) qs.Add($"from={from.Value:yyyy-MM-ddTHH:mm:ss}");
        if (to.HasValue) qs.Add($"to={to.Value:yyyy-MM-ddTHH:mm:ss}");
        if (staffId.HasValue) qs.Add($"staffId={staffId.Value}");
        if (qs.Count > 0) url += "?" + string.Join("&", qs);
        return await GetAsync<List<AppointmentResponse>>(url) ?? new();
    }

    public Task<AppointmentResponse?> CreateAppointmentAsync(string slug, CreateAppointmentRequest req) =>
        PostAsync<AppointmentResponse>($"/api/tenants/{slug}/appointments", req);

    public Task<bool> ApproveAppointmentAsync(string slug, Guid id) =>
        PostBoolAsync($"/api/tenants/{slug}/appointments/{id}/approve", new { });

    public Task<bool> DeclineAppointmentAsync(string slug, Guid id, string? reason = null) =>
        PostBoolAsync($"/api/tenants/{slug}/appointments/{id}/decline", new { Reason = reason });

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

    public Task<BookingFormResponse?> UpdateFormAsync(string slug, Guid formId, UpdateBookingFormRequest req) =>
        PutAsync<BookingFormResponse>($"/api/tenants/{slug}/booking-forms/{formId}", req);

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

    // ── Staff ──
    public Task<List<StaffResponse>?> GetAllStaffAsync(string slug) =>
        GetAsync<List<StaffResponse>>($"/api/tenants/{slug}/staff/all");

    public Task<StaffResponse?> CreateStaffAsync(string slug, CreateStaffRequest req) =>
        PostAsync<StaffResponse>($"/api/tenants/{slug}/staff", req);

    public Task<StaffResponse?> UpdateStaffAsync(string slug, Guid id, UpdateStaffRequest req) =>
        PutAsync<StaffResponse>($"/api/tenants/{slug}/staff/{id}", req);

    public Task<bool> DeleteStaffAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/staff/{id}");

    public Task<bool> AssignStaffServicesAsync(string slug, Guid staffId, AssignStaffServicesRequest req) =>
        PostBoolAsync($"/api/tenants/{slug}/staff/{staffId}/services", req);

    public Task<StaffInvitationResponse?> GetStaffInvitationAsync(string token) =>
        GetAsync<StaffInvitationResponse>($"/api/tenants/staff/invitation/{token}");

    public Task<bool> AcceptStaffInvitationAsync(AcceptStaffInvitationRequest req) =>
        PostBoolAsync("/api/tenants/staff/accept-invitation", req);

    // ── Class Sessions ──
    public Task<List<ClassSessionResponse>?> GetClassSessionsAsync(string slug, int days = 365) =>
        GetAsync<List<ClassSessionResponse>>($"/api/tenants/{slug}/class-sessions?days={days}");

    public Task<ClassSessionResponse?> CreateClassSessionAsync(string slug, CreateClassSessionRequest req) =>
        PostAsync<ClassSessionResponse>($"/api/tenants/{slug}/class-sessions", req);

    public Task<ClassSessionResponse?> UpdateClassSessionAsync(string slug, Guid id, UpdateClassSessionRequest req) =>
        PutAsync<ClassSessionResponse>($"/api/tenants/{slug}/class-sessions/{id}", req);

    public Task<bool> CancelClassSessionAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/class-sessions/{id}");

    // ── Super Admin ──
    public Task<List<TenantListResponse>?> GetAllTenantsAsync() =>
        GetAsync<List<TenantListResponse>>("/api/admin/tenants");

    public Task<TenantResponse?> SuperAdminUpdateTenantAsync(Guid tenantId, UpdateTenantRequest req) =>
        PutAsync<TenantResponse>($"/api/admin/tenants/{tenantId}", req);

    public Task<bool> SuperAdminDeleteTenantAsync(Guid tenantId) =>
        DeleteAsync($"/api/admin/tenants/{tenantId}");

    // ── Clients ──
    public Task<List<ClientResponse>?> GetAllClientsAsync() =>
        GetAsync<List<ClientResponse>>("/api/admin/clients");

    public Task<ClientResponse?> GetClientAsync(Guid id) =>
        GetAsync<ClientResponse>($"/api/admin/clients/{id}");

    public Task<ClientResponse?> CreateClientAsync(CreateClientRequest req) =>
        PostAsync<ClientResponse>("/api/admin/clients", req);

    public Task<ClientResponse?> UpdateClientAsync(Guid id, UpdateClientRequest req) =>
        PutAsync<ClientResponse>($"/api/admin/clients/{id}", req);

    public Task<bool> DeleteClientAsync(Guid id) =>
        DeleteAsync($"/api/admin/clients/{id}");

    // ── Database Management ──
    public Task<object?> GetDatabaseStatusAsync() =>
        GetAsync<object>("/api/admin/database/status");

    public Task<bool> SeedDemoDataAsync() =>
        PostBoolAsync("/api/admin/database/seed", new { });

    public Task<bool> ClearDemoDataAsync() =>
        PostBoolAsync("/api/admin/database/clear", new { });

    // ── Email Templates ──
    public Task<List<EmailTemplateResponse>?> GetEmailTemplatesAsync(string slug) =>
        GetAsync<List<EmailTemplateResponse>>($"/api/tenants/{slug}/email-templates");

    public Task<EmailTemplateResponse?> CreateEmailTemplateAsync(string slug, UpsertEmailTemplateRequest req) =>
        PostAsync<EmailTemplateResponse>($"/api/tenants/{slug}/email-templates", req);

    public Task<EmailTemplateResponse?> UpdateEmailTemplateAsync(string slug, Guid id, UpsertEmailTemplateRequest req) =>
        PutAsync<EmailTemplateResponse>($"/api/tenants/{slug}/email-templates/{id}", req);

    public Task<bool> DeleteEmailTemplateAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/email-templates/{id}");

    // ── Webhooks ──
    public Task<List<WebhookResponse>?> GetWebhooksAsync(string slug) =>
        GetAsync<List<WebhookResponse>>($"/api/tenants/{slug}/webhooks");

    public Task<WebhookResponse?> CreateWebhookAsync(string slug, CreateWebhookRequest req) =>
        PostAsync<WebhookResponse>($"/api/tenants/{slug}/webhooks", req);

    public Task<WebhookResponse?> UpdateWebhookAsync(string slug, Guid id, UpdateWebhookRequest req) =>
        PutAsync<WebhookResponse>($"/api/tenants/{slug}/webhooks/{id}", req);

    public Task<bool> DeleteWebhookAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/webhooks/{id}");

    public Task<List<WebhookDeliveryResponse>?> GetWebhookDeliveriesAsync(string slug, Guid webhookId) =>
        GetAsync<List<WebhookDeliveryResponse>>($"/api/tenants/{slug}/webhooks/{webhookId}/deliveries");

    // ── Audit Trail ──
    public Task<PagedResult<AuditLogResponse>?> GetAuditTrailAsync(string slug, AuditLogQueryParams queryParams)
    {
        var qs = $"page={queryParams.Page}&pageSize={queryParams.PageSize}";
        if (!string.IsNullOrWhiteSpace(queryParams.EntityName)) qs += $"&entityName={Uri.EscapeDataString(queryParams.EntityName)}";
        if (!string.IsNullOrWhiteSpace(queryParams.Action))     qs += $"&action={Uri.EscapeDataString(queryParams.Action)}";
        if (!string.IsNullOrWhiteSpace(queryParams.ChangedBy))  qs += $"&changedBy={Uri.EscapeDataString(queryParams.ChangedBy)}";
        if (queryParams.From.HasValue) qs += $"&from={queryParams.From.Value:O}";
        if (queryParams.To.HasValue)   qs += $"&to={queryParams.To.Value:O}";
        return GetAsync<PagedResult<AuditLogResponse>>($"/api/tenants/{slug}/audit-trail?{qs}");
    }

    // ── Lodging Properties ──
    public Task<List<LodgingPropertyResponse>?> GetLodgingPropertiesAsync(string slug) =>
        GetAsync<List<LodgingPropertyResponse>>($"/api/tenants/{slug}/lodging/properties");

    public Task<LodgingPropertyResponse?> CreateLodgingPropertyAsync(string slug, CreateLodgingPropertyRequest req) =>
        PostAsync<LodgingPropertyResponse>($"/api/tenants/{slug}/lodging/properties", req);

    public Task<bool> UpdateLodgingPropertyAsync(string slug, Guid id, CreateLodgingPropertyRequest req) =>
        PutAsync($"/api/tenants/{slug}/lodging/properties/{id}", req);

    public Task<bool> DeleteLodgingPropertyAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/lodging/properties/{id}");

    // ── Rooms ──
    public Task<List<RoomResponse>?> GetRoomsAsync(string slug, Guid? propertyId = null)
    {
        var url = $"/api/tenants/{slug}/lodging/rooms";
        if (propertyId.HasValue) url += $"?propertyId={propertyId}";
        return GetAsync<List<RoomResponse>>(url);
    }

    public Task<RoomResponse?> CreateRoomAsync(string slug, CreateRoomRequest req) =>
        PostAsync<RoomResponse>($"/api/tenants/{slug}/lodging/rooms", req);

    public Task<bool> UpdateRoomAsync(string slug, Guid id, CreateRoomRequest req) =>
        PutAsync($"/api/tenants/{slug}/lodging/rooms/{id}", req);

    public Task<bool> DeleteRoomAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/lodging/rooms/{id}");

    public Task<RoomPhotoResponse?> AddRoomPhotoAsync(string slug, Guid roomId, AddRoomPhotoRequest req) =>
        PostAsync<RoomPhotoResponse>($"/api/tenants/{slug}/lodging/rooms/{roomId}/photos", req);

    public Task<bool> DeleteRoomPhotoAsync(string slug, Guid roomId, Guid photoId) =>
        DeleteAsync($"/api/tenants/{slug}/lodging/rooms/{roomId}/photos/{photoId}");

    // ── Amenities ──
    public Task<List<AmenityResponse>?> GetAmenitiesAsync(string slug) =>
        GetAsync<List<AmenityResponse>>($"/api/tenants/{slug}/lodging/amenities");

    public Task<AmenityResponse?> CreateAmenityAsync(string slug, CreateAmenityRequest req) =>
        PostAsync<AmenityResponse>($"/api/tenants/{slug}/lodging/amenities", req);

    public Task<bool> UpdateAmenityAsync(string slug, Guid id, CreateAmenityRequest req) =>
        PutAsync($"/api/tenants/{slug}/lodging/amenities/{id}", req);

    public Task<bool> DeleteAmenityAsync(string slug, Guid id) =>
        DeleteAsync($"/api/tenants/{slug}/lodging/amenities/{id}");

    public Task<bool> AssignRoomAmenitiesAsync(string slug, Guid roomId, AssignRoomAmenitiesRequest req) =>
        PostBoolAsync($"/api/tenants/{slug}/lodging/rooms/{roomId}/amenities", req);

    // ── Room Rates ──
    public Task<List<RoomRateResponse>?> GetRoomRatesAsync(string slug, Guid roomId) =>
        GetAsync<List<RoomRateResponse>>($"/api/tenants/{slug}/lodging/rooms/{roomId}/rates");

    public Task<RoomRateResponse?> CreateRoomRateAsync(string slug, Guid roomId, CreateRoomRateRequest req) =>
        PostAsync<RoomRateResponse>($"/api/tenants/{slug}/lodging/rooms/{roomId}/rates", req);

    public Task<bool> DeleteRoomRateAsync(string slug, Guid roomId, Guid rateId) =>
        DeleteAsync($"/api/tenants/{slug}/lodging/rooms/{roomId}/rates/{rateId}");

    // ── Public Listings ──
    public Task<List<PublicPropertyListingResponse>?> GetPublicListingsAsync(
        string? city = null, decimal? minRate = null, decimal? maxRate = null,
        string? amenity = null, bool? petFriendly = null, bool? wheelchairAccessible = null, string sortBy = "name")
    {
        var qs = new System.Text.StringBuilder("/api/listings?");
        if (!string.IsNullOrWhiteSpace(city)) qs.Append($"city={Uri.EscapeDataString(city)}&");
        if (minRate.HasValue) qs.Append($"minRate={minRate}&");
        if (maxRate.HasValue) qs.Append($"maxRate={maxRate}&");
        if (!string.IsNullOrWhiteSpace(amenity)) qs.Append($"amenity={Uri.EscapeDataString(amenity)}&");
        if (petFriendly.HasValue) qs.Append($"petFriendly={petFriendly.Value}&");
        if (wheelchairAccessible.HasValue) qs.Append($"wheelchairAccessible={wheelchairAccessible.Value}&");
        qs.Append($"sortBy={sortBy}");
        return GetAsync<List<PublicPropertyListingResponse>>(qs.ToString());
    }

    private async Task<bool> PostBoolAsync(string url, object body)
    {
        var r = await _http.PostAsync(url, Json(body));
        return r.IsSuccessStatusCode;
    }
}
