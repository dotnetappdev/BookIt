namespace BookIt.Core.Entities;

public class Webhook : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    /// <summary>Target URL that receives the HTTP POST payload.</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Optional HMAC-SHA256 signing secret (sent in X-BookIt-Signature header).</summary>
    public string? Secret { get; set; }

    /// <summary>Comma-separated event names to subscribe to, or "*" for all events.</summary>
    public string Events { get; set; } = "*";

    public bool IsActive { get; set; } = true;

    public ICollection<WebhookDelivery> Deliveries { get; set; } = new List<WebhookDelivery>();
}
