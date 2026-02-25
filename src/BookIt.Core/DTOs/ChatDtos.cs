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
    public string Type { get; set; } = string.Empty; // "redirect" | "show_slots" | "show_services"
    public string? Url { get; set; }
    public object? Data { get; set; }
}
