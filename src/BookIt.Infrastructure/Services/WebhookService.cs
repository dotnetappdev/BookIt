using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BookIt.Core.Entities;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookIt.Infrastructure.Services;

public class WebhookService : IWebhookService
{
    private readonly BookItDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<WebhookService> _logger;

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public WebhookService(BookItDbContext context, IHttpClientFactory httpClientFactory, ILogger<WebhookService> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task FireAsync(Guid tenantId, string eventType, object payload)
    {
        var webhooks = await _context.Webhooks
            .Where(w => w.TenantId == tenantId && w.IsActive && !w.IsDeleted)
            .ToListAsync();

        foreach (var webhook in webhooks.Where(w => MatchesEvent(w.Events, eventType)))
        {
            var envelope = new { @event = eventType, data = payload, timestamp = DateTime.UtcNow };
            var json = JsonSerializer.Serialize(envelope, _json);

            var statusCode = 0;
            string? responseBody = null;
            var success = false;

            try
            {
                var client = _httpClientFactory.CreateClient();
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                if (!string.IsNullOrEmpty(webhook.Secret))
                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "X-BookIt-Signature", ComputeHmac(json, webhook.Secret));

                var response = await client.PostAsync(webhook.Url, content);
                statusCode = (int)response.StatusCode;
                responseBody = await response.Content.ReadAsStringAsync();
                success = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Webhook delivery failed for {Url}", webhook.Url);
            }

            _context.WebhookDeliveries.Add(new WebhookDelivery
            {
                WebhookId = webhook.Id,
                Event = eventType,
                Payload = json,
                StatusCode = statusCode,
                ResponseBody = responseBody,
                Success = success,
                DeliveredAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
    }

    private static bool MatchesEvent(string events, string eventType)
    {
        if (string.IsNullOrEmpty(events) || events.Trim() == "*") return true;
        return events.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Any(e => e.Equals(eventType, StringComparison.OrdinalIgnoreCase));
    }

    private static string ComputeHmac(string payload, string secret)
    {
        var key = Encoding.UTF8.GetBytes(secret);
        var data = Encoding.UTF8.GetBytes(payload);
        return Convert.ToHexString(HMACSHA256.HashData(key, data)).ToLowerInvariant();
    }
}
