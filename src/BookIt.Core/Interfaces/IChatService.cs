using BookIt.Core.DTOs;

namespace BookIt.Core.Interfaces;

public interface IChatService
{
    Task<ChatResponse> ProcessAsync(string tenantSlug, ChatRequest request);
}
