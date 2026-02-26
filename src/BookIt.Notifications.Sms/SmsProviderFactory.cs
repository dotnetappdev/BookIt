namespace BookIt.Notifications.Sms;

/// <summary>
/// Factory that selects the correct <see cref="ISmsProvider"/> based on a provider name.
/// </summary>
public sealed class SmsProviderFactory
{
    private readonly ClickSendSmsProvider _clickSend;
    private readonly TwilioSmsProvider _twilio;

    public SmsProviderFactory(ClickSendSmsProvider clickSend, TwilioSmsProvider twilio)
    {
        _clickSend = clickSend;
        _twilio = twilio;
    }

    /// <summary>
    /// Returns the provider matching <paramref name="providerName"/> (case-insensitive).
    /// Supported values: <c>"clicksend"</c>, <c>"twilio"</c>.
    /// </summary>
    public ISmsProvider Get(string providerName) =>
        providerName.ToLowerInvariant() switch
        {
            "twilio" => _twilio,
            _ => _clickSend   // default to ClickSend
        };
}
