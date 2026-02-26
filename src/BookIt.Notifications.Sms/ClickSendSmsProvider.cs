using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BookIt.Notifications.Sms;

/// <summary>
/// ClickSend SMS provider. Uses the ClickSend REST API v3.
/// Credentials are passed per-call so a single instance serves multiple tenants.
/// </summary>
public sealed class ClickSendSmsProvider : ISmsProvider
{
    private const string BaseUrl = "https://rest.clicksend.com";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ClickSendSmsProvider> _logger;

    public ClickSendSmsProvider(IHttpClientFactory httpClientFactory, ILogger<ClickSendSmsProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<SmsSendResult> SendAsync(
        string toPhone,
        string message,
        string fromNumber,
        CancellationToken cancellationToken = default)
    {
        // ClickSend API key is passed as "username:api_key" Basic auth
        // For multi-tenant use, fromNumber carries "username:api_key"
        // Expected format: "USERNAME:API_KEY"
        var parts = fromNumber.Split(':', 2);
        if (parts.Length != 2)
        {
            return new SmsSendResult(false, null,
                "ClickSend: fromNumber must be in format 'USERNAME:API_KEY'.");
        }

        var (username, apiKey) = (parts[0], parts[1]);
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseUrl);
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{apiKey}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        var payload = new
        {
            messages = new[]
            {
                new
                {
                    source = "bookit",
                    body = message,
                    to = toPhone,
                }
            }
        };

        var response = await client.PostAsync(
            "/v3/sms/send",
            new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"),
            cancellationToken);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("ClickSend SMS failed to {To}: {Status} — {Body}", toPhone, response.StatusCode, json);
            return new SmsSendResult(false, null, $"HTTP {(int)response.StatusCode}");
        }

        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        var msgId = doc
            .GetProperty("data")
            .GetProperty("messages")[0]
            .GetProperty("message_id")
            .GetString();

        _logger.LogInformation("ClickSend SMS sent to {To}, messageId={Id}", toPhone, msgId);
        return new SmsSendResult(true, msgId, null);
    }
}
