namespace BookIt.Core.Interfaces;

public interface IWebhookService
{
    /// <summary>Fires <paramref name="eventType"/> to all active webhooks registered for the tenant.</summary>
    Task FireAsync(Guid tenantId, string eventType, object payload);
}
