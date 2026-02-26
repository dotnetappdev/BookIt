using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BookIt.Notifications.Sms;

/// <summary>
/// Twilio SMS provider. Uses the Twilio Messages API.
/// Credentials are passed per-call so a single instance serves multiple tenants.
/// </summary>
public sealed class TwilioSmsProvider : ISmsProvider
{
    private const string BaseUrl = "https://api.twilio.com";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TwilioSmsProvider> _logger;

    public TwilioSmsProvider(IHttpClientFactory httpClientFactory, ILogger<TwilioSmsProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    /// <remarks>
    /// For Twilio the <paramref name="fromNumber"/> parameter should carry the tenant's credentials
    /// in the format <c>AccountSid:AuthToken:FromNumber</c>.
    /// </remarks>
    public async Task<SmsSendResult> SendAsync(
        string toPhone,
        string message,
        string fromNumber,
        CancellationToken cancellationToken = default)
    {
        // Expected format: "ACCOUNT_SID:AUTH_TOKEN:FROM_NUMBER"
        var parts = fromNumber.Split(':', 3);
        if (parts.Length != 3)
        {
            return new SmsSendResult(false, null,
                "Twilio: fromNumber must be in format 'ACCOUNT_SID:AUTH_TOKEN:FROM_NUMBER'.");
        }

        var (accountSid, authToken, from) = (parts[0], parts[1], parts[2]);

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseUrl);
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{accountSid}:{authToken}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        var formData = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["From"] = from,
            ["To"] = toPhone,
            ["Body"] = message,
        });

        var response = await client.PostAsync(
            $"/2010-04-01/Accounts/{accountSid}/Messages.json",
            formData,
            cancellationToken);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Twilio SMS failed to {To}: {Status} — {Body}", toPhone, response.StatusCode, json);
            return new SmsSendResult(false, null, $"HTTP {(int)response.StatusCode}");
        }

        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        var sid = doc.GetProperty("sid").GetString();

        _logger.LogInformation("Twilio SMS sent to {To}, sid={Sid}", toPhone, sid);
        return new SmsSendResult(true, sid, null);
    }
}
