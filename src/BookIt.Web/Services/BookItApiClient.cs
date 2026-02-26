using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BookIt.Core.DTOs;

namespace BookIt.Web.Services;

public class BookItApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public BookItApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void SetAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync("/api/auth/login",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(content, _jsonOptions);
    }

    public async Task<TenantResponse?> GetTenantAsync(string slug)
    {
        var response = await _httpClient.GetAsync($"/api/tenants/{slug}");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TenantResponse>(content, _jsonOptions);
    }

    public async Task<List<ServiceResponse>> GetServicesAsync(string tenantSlug)
    {
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/services");
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ServiceResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<List<DateTime>> GetAvailableSlotsAsync(string tenantSlug, Guid serviceId, Guid? staffId, DateOnly date)
    {
        var url = $"/api/tenants/{tenantSlug}/appointments/slots?serviceId={serviceId}&date={date:yyyy-MM-dd}";
        if (staffId.HasValue) url += $"&staffId={staffId}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<DateTime>>(content, _jsonOptions) ?? new();
    }

    public async Task<AppointmentResponse?> CreateAppointmentAsync(string tenantSlug, CreateAppointmentRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/appointments",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AppointmentResponse>(content, _jsonOptions);
    }

    public async Task<AppointmentResponse?> GetAppointmentByTokenAsync(string tenantSlug, string token)
    {
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/appointments/confirm/{token}");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AppointmentResponse>(content, _jsonOptions);
    }

    public async Task<List<AppointmentResponse>> GetAppointmentsAsync(string tenantSlug, DateTime? from = null, DateTime? to = null)
    {
        SetAuthHeader();
        var url = $"/api/tenants/{tenantSlug}/appointments";
        var queryParams = new List<string>();
        if (from.HasValue) queryParams.Add($"from={from.Value:yyyy-MM-ddTHH:mm:ss}");
        if (to.HasValue) queryParams.Add($"to={to.Value:yyyy-MM-ddTHH:mm:ss}");
        if (queryParams.Any()) url += "?" + string.Join("&", queryParams);

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<AppointmentResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<TenantResponse?> UpdateTenantAsync(string slug, UpdateTenantRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PutAsync($"/api/tenants/{slug}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TenantResponse>(content, _jsonOptions);
    }

    public async Task<AuthResponse?> SetupTenantAsync(TenantSetupRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync("/api/auth/setup",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(content, _jsonOptions);
    }

    public async Task<InvitationDetailResponse?> GetInterviewInvitationAsync(string tenantSlug, string token)
    {
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/interviewslots/invitations/{token}");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<InvitationDetailResponse>(content, _jsonOptions);
    }

    public async Task<List<InterviewSlotResponse>> GetInterviewSlotsAsync(string tenantSlug, Guid? serviceId = null)
    {
        SetAuthHeader();
        var url = $"/api/tenants/{tenantSlug}/interviewslots";
        if (serviceId.HasValue) url += $"?serviceId={serviceId}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<InterviewSlotResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<InterviewSlotResponse?> CreateInterviewSlotAsync(string tenantSlug, CreateInterviewSlotRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/interviewslots",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<InterviewSlotResponse>(content, _jsonOptions);
    }

    public async Task<bool> DeleteInterviewSlotAsync(string tenantSlug, Guid slotId)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"/api/tenants/{tenantSlug}/interviewslots/{slotId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<CandidateInvitationResponse?> SendInterviewInvitationAsync(string tenantSlug, SendInvitationRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/interviewslots/invitations",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CandidateInvitationResponse>(content, _jsonOptions);
    }

    public async Task<List<CandidateInvitationResponse>> GetInterviewInvitationsAsync(string tenantSlug)
    {
        SetAuthHeader();
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/interviewslots/invitations");
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CandidateInvitationResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<bool> BookInterviewSlotAsync(string tenantSlug, string token, BookInterviewRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/interviewslots/invitations/{token}/book",
            new StringContent(json, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<List<BookingFormResponse>> GetFormsAsync(string tenantSlug)
    {
        SetAuthHeader();
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/booking-forms");
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<BookingFormResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<BookingFormResponse?> GetDefaultFormAsync(string tenantSlug)
    {
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/booking-forms/default");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BookingFormResponse>(content, _jsonOptions);
    }

    public async Task<BookingFormResponse?> GetFormAsync(string tenantSlug, Guid formId)
    {
        SetAuthHeader();
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/booking-forms/{formId}");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BookingFormResponse>(content, _jsonOptions);
    }

    public async Task<BookingFormResponse?> CreateFormAsync(string tenantSlug, CreateBookingFormRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/booking-forms",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BookingFormResponse>(content, _jsonOptions);
    }

    public async Task<bool> DeleteFormAsync(string tenantSlug, Guid formId)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"/api/tenants/{tenantSlug}/booking-forms/{formId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<BookingFormFieldResponse?> AddFormFieldAsync(string tenantSlug, Guid formId, AddFormFieldRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/booking-forms/{formId}/fields",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BookingFormFieldResponse>(content, _jsonOptions);
    }

    public async Task<bool> UpdateFormFieldAsync(string tenantSlug, Guid formId, Guid fieldId, UpdateFormFieldRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PutAsync($"/api/tenants/{tenantSlug}/booking-forms/{formId}/fields/{fieldId}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteFormFieldAsync(string tenantSlug, Guid formId, Guid fieldId)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"/api/tenants/{tenantSlug}/booking-forms/{formId}/fields/{fieldId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ReorderFormFieldsAsync(string tenantSlug, Guid formId, List<Guid> fieldIds)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(new ReorderFieldsRequest { FieldIds = fieldIds });
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/booking-forms/{formId}/fields/reorder",
            new StringContent(json, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<List<StaffResponse>> GetStaffAsync(string tenantSlug)
    {
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/staff");
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<StaffResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<List<StaffResponse>> GetAllStaffAsync(string tenantSlug)
    {
        SetAuthHeader();
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/staff/all");
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<StaffResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<StaffResponse?> CreateStaffAsync(string tenantSlug, CreateStaffRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/staff",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StaffResponse>(content, _jsonOptions);
    }

    public async Task<StaffResponse?> UpdateStaffAsync(string tenantSlug, Guid staffId, UpdateStaffRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PutAsync($"/api/tenants/{tenantSlug}/staff/{staffId}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<StaffResponse>(content, _jsonOptions);
    }

    public async Task<bool> DeleteStaffAsync(string tenantSlug, Guid staffId)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"/api/tenants/{tenantSlug}/staff/{staffId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AssignStaffServicesAsync(string tenantSlug, Guid staffId, List<Guid> serviceIds)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(new AssignStaffServicesRequest { ServiceIds = serviceIds });
        var response = await _httpClient.PutAsync($"/api/tenants/{tenantSlug}/staff/{staffId}/services",
            new StringContent(json, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CustomerResponse>> GetCustomersAsync(string tenantSlug)
    {
        SetAuthHeader();
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/customers");
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CustomerResponse>>(content, _jsonOptions) ?? new();
    }

    public async Task<CustomerResponse?> CreateCustomerAsync(string tenantSlug, CreateCustomerRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/customers",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerResponse>(content, _jsonOptions);
    }

    public async Task<CustomerResponse?> UpdateCustomerAsync(string tenantSlug, Guid customerId, UpdateCustomerRequest request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PutAsync($"/api/tenants/{tenantSlug}/customers/{customerId}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerResponse>(content, _jsonOptions);
    }

    public async Task<bool> DeleteCustomerAsync(string tenantSlug, Guid customerId)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"/api/tenants/{tenantSlug}/customers/{customerId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<JsonElement>> GetClassSessionsAsync(string tenantSlug, int days = 90)
    {
        SetAuthHeader();
        var response = await _httpClient.GetAsync($"/api/tenants/{tenantSlug}/class-sessions?days={days}");
        if (!response.IsSuccessStatusCode) return new();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<JsonElement>>(content, _jsonOptions) ?? new();
    }

    public async Task<JsonElement?> CreateClassSessionAsync(string tenantSlug, object request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PostAsync($"/api/tenants/{tenantSlug}/class-sessions",
            new StringContent(json, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<JsonElement>(content, _jsonOptions);
    }

    public async Task<bool> UpdateClassSessionAsync(string tenantSlug, Guid sessionId, object request)
    {
        SetAuthHeader();
        var json = JsonSerializer.Serialize(request);
        var response = await _httpClient.PutAsync($"/api/tenants/{tenantSlug}/class-sessions/{sessionId}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteClassSessionAsync(string tenantSlug, Guid sessionId)
    {
        SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"/api/tenants/{tenantSlug}/class-sessions/{sessionId}");
        return response.IsSuccessStatusCode;
    }
}
