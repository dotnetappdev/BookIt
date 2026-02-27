namespace BookIt.Core.DTOs;

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public List<ChatMessageDto> History { get; set; } = new();
}

public class ChatMessageDto
{
    public string Role { get; set; } = "user"; // "user" | "assistant"
    public string Content { get; set; } = string.Empty;
}

public class ChatResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string> QuickReplies { get; set; } = new();
    public ChatAction? Action { get; set; }
    public bool UseElevenLabs { get; set; }
    public string? ElevenLabsApiKey { get; set; }
    public string? ElevenLabsVoiceId { get; set; }
}

public class ChatAction
{
    public string Type { get; set; } = string.Empty; // "redirect" | "show_slots" | "show_services" | "booking_complete"
    public string? Url { get; set; }
    public object? Data { get; set; }
}

// Agent / dashboard DTOs
public class ChatSessionSummary
{
    public Guid Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime LastMessageAt { get; set; }
    public bool HasUnreadAgentMessages { get; set; }
    public string? LastMessage { get; set; }
    public int MessageCount { get; set; }
}

public class ChatSessionDetail
{
    public Guid Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime LastMessageAt { get; set; }
    public List<ChatMessageDetail> Messages { get; set; } = new();
}

public class ChatMessageDetail
{
    public Guid Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsAgentMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AgentReplyRequest
{
    public string Message { get; set; } = string.Empty;
}
