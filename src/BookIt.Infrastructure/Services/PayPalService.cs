using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BookIt.Core.Enums;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastructure.Services;

public class PayPalService : IPayPalService
{
    private readonly BookItDbContext _context;
    private readonly ILogger<PayPalService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    private const string SandboxBaseUrl = "https://api-m.sandbox.paypal.com";

    public PayPalService(BookItDbContext context, ILogger<PayPalService> logger, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> CreateOrderAsync(Guid tenantId, Guid appointmentId, decimal amount, string currency)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.Id == appointmentId);

        if (appointment == null)
            throw new InvalidOperationException("Appointment not found");

        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null)
            throw new InvalidOperationException("Tenant not found");

        if (string.IsNullOrEmpty(tenant.PayPalClientId) || string.IsNullOrEmpty(tenant.PayPalClientSecret))
            throw new InvalidOperationException("PayPal is not configured for this tenant");

        try
        {
            var accessToken = await GetPayPalAccessTokenAsync(tenant.PayPalClientId, tenant.PayPalClientSecret, SandboxBaseUrl);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SandboxBaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var orderPayload = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        reference_id = appointmentId.ToString(),
                        amount = new { currency_code = currency.ToUpper(), value = amount.ToString("F2") },
                        description = $"BookIt Appointment - {appointment.CustomerName}"
                    }
                }
            };

            var json = JsonSerializer.Serialize(orderPayload);
            var response = await client.PostAsync("/v2/checkout/orders",
                new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            var orderId = result.GetProperty("id").GetString()!;

            var payment = new Core.Entities.Payment
            {
                TenantId = tenantId,
                AppointmentId = appointmentId,
                Amount = amount,
                Currency = currency,
                Provider = PaymentProvider.PayPal,
                Status = PaymentStatus.Pending,
                ProviderPaymentIntentId = orderId,
                Metadata = content
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return orderId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create PayPal order for appointment {AppointmentId}", appointmentId);
            throw;
        }
    }

    public async Task<bool> CaptureOrderAsync(Guid tenantId, string orderId)
    {
        var payment = await _context.Payments
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.ProviderPaymentIntentId == orderId);

        if (payment == null) return false;

        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null || string.IsNullOrEmpty(tenant.PayPalClientId)) return false;

        try
        {
            var accessToken = await GetPayPalAccessTokenAsync(tenant.PayPalClientId, tenant.PayPalClientSecret!, SandboxBaseUrl);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SandboxBaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsync($"/v2/checkout/orders/{orderId}/capture",
                new StringContent("{}", Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) return false;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            var status = result.GetProperty("status").GetString();

            if (status == "COMPLETED")
            {
                payment.Status = PaymentStatus.Paid;
                payment.PaidAt = DateTime.UtcNow;
                payment.Appointment.PaymentStatus = PaymentStatus.Paid;
                payment.Appointment.PaymentReference = orderId;
                payment.Appointment.PaymentProvider = PaymentProvider.PayPal;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture PayPal order {OrderId}", orderId);
            return false;
        }
    }

    private static async Task<string> GetPayPalAccessTokenAsync(string clientId, string clientSecret, string baseUrl)
    {
        var client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        var response = await client.PostAsync("/v1/oauth2/token",
            new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(content);
        return result.GetProperty("access_token").GetString()!;
    }
}
