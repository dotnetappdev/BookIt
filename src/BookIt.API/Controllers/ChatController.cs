using BookIt.Core.DTOs;
using BookIt.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(string tenantSlug, [FromBody] ChatRequest request)
    {
        var response = await _chatService.ProcessAsync(tenantSlug, request);
        return Ok(response);
    }
}
