namespace BookIt.Core.Entities;

public class WebhookDelivery : BaseEntity
{
    public Guid WebhookId { get; set; }
    public Webhook Webhook { get; set; } = null!;

    public string Event { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;

    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    public bool Success { get; set; }
    public int AttemptCount { get; set; } = 1;
    public DateTime DeliveredAt { get; set; } = DateTime.UtcNow;
}
