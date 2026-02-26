namespace BookIt.Notifications.Sms;

/// <summary>Result of an SMS send attempt.</summary>
public sealed record SmsSendResult(bool Success, string? MessageId, string? ErrorMessage);

/// <summary>
/// Abstraction over an SMS provider.
/// Accepts credentials per-call so a single instance can serve multiple tenants.
/// </summary>
public interface ISmsProvider
{
    /// <summary>Sends a plain-text SMS message to the given E.164 phone number.</summary>
    Task<SmsSendResult> SendAsync(
        string toPhone,
        string message,
        string fromNumber,
        CancellationToken cancellationToken = default);
}
