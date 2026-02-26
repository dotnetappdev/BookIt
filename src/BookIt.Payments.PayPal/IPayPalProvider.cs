namespace BookIt.Payments.PayPal;

/// <summary>
/// Abstraction over the PayPal Orders v2 API. Accepts credentials per-call so a
/// single instance can serve multiple tenants without storing secrets.
/// </summary>
public interface IPayPalProvider
{
    /// <summary>
    /// Creates a PayPal Order (CAPTURE intent) and returns the PayPal Order ID.
    /// </summary>
    Task<string> CreateOrderAsync(
        string clientId,
        string clientSecret,
        decimal amount,
        string currency,
        string referenceId,
        string description,
        bool useSandbox = false);

    /// <summary>
    /// Captures an approved PayPal Order and returns true if the status is COMPLETED.
    /// </summary>
    Task<bool> CaptureOrderAsync(
        string clientId,
        string clientSecret,
        string orderId,
        bool useSandbox = false);
}
