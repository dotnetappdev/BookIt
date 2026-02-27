using BookIt.Core.DTOs;

namespace BookIt.Core.Interfaces;

public interface IChatService
{
    Task<ChatResponse> ProcessAsync(string tenantSlug, ChatRequest request);
    Task<List<ChatSessionSummary>> GetSessionsAsync(Guid tenantId);
    Task<ChatSessionDetail?> GetSessionAsync(Guid tenantId, string sessionId);
    Task<bool> AgentReplyAsync(Guid tenantId, string sessionId, string message);
    Task<bool> ResolveSessionAsync(Guid tenantId, string sessionId);
}
