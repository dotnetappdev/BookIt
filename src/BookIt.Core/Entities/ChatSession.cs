namespace BookIt.Core.Entities;

public class ChatSession : BaseEntity
{
    public Guid TenantId { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public ChatSessionStatus Status { get; set; } = ChatSessionStatus.Active;
    public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;
    public bool HasUnreadAgentMessages { get; set; } = false;

    public Tenant? Tenant { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}

public class ChatMessage : BaseEntity
{
    public Guid ChatSessionId { get; set; }
    public string Role { get; set; } = "user"; // "user" | "assistant" | "agent"
    public string Content { get; set; } = string.Empty;
    public bool IsAgentMessage { get; set; } = false;

    public ChatSession? ChatSession { get; set; }
}

public enum ChatSessionStatus
{
    Active = 1,
    HandedOffToAgent = 2,
    Resolved = 3
}
