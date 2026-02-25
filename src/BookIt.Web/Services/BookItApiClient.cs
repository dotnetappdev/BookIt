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
}
