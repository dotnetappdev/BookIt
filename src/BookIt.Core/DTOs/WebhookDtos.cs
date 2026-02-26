namespace BookIt.Core.DTOs;

public class CreateWebhookRequest
{
    public string Url { get; set; } = string.Empty;
    public string? Secret { get; set; }
    /// <summary>Comma-separated event names, or "*" for all.</summary>
    public string Events { get; set; } = "*";
}

public class UpdateWebhookRequest
{
    public string Url { get; set; } = string.Empty;
    public string? Secret { get; set; }
    public string Events { get; set; } = "*";
    public bool IsActive { get; set; } = true;
}

public class WebhookResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Events { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class WebhookDeliveryResponse
{
    public Guid Id { get; set; }
    public Guid WebhookId { get; set; }
    public string Event { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public int AttemptCount { get; set; }
    public DateTime DeliveredAt { get; set; }
}
