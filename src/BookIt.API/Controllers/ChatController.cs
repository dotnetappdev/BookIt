using BookIt.Core.DTOs;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookIt.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantSlug}/chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly BookItDbContext _context;

    public ChatController(IChatService chatService, BookItDbContext context)
    {
        _chatService = chatService;
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Chat(string tenantSlug, [FromBody] ChatRequest request)
    {
        var response = await _chatService.ProcessAsync(tenantSlug, request);
        return Ok(response);
    }

    // ── Agent dashboard endpoints ──────────────────────────────────────────────

    [Authorize]
    [HttpGet("sessions")]
    public async Task<ActionResult<List<ChatSessionSummary>>> GetSessions(string tenantSlug)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();
        var sessions = await _chatService.GetSessionsAsync(tenant.Id);
        return Ok(sessions);
    }

    [Authorize]
    [HttpGet("sessions/{sessionId}")]
    public async Task<ActionResult<ChatSessionDetail>> GetSession(string tenantSlug, string sessionId)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();
        var session = await _chatService.GetSessionAsync(tenant.Id, sessionId);
        if (session == null) return NotFound();
        return Ok(session);
    }

    [Authorize]
    [HttpPost("sessions/{sessionId}/reply")]
    public async Task<IActionResult> AgentReply(string tenantSlug, string sessionId, [FromBody] AgentReplyRequest request)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();
        var ok = await _chatService.AgentReplyAsync(tenant.Id, sessionId, request.Message);
        return ok ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpPost("sessions/{sessionId}/resolve")]
    public async Task<IActionResult> ResolveSession(string tenantSlug, string sessionId)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null) return NotFound();
        var ok = await _chatService.ResolveSessionAsync(tenant.Id, sessionId);
        return ok ? NoContent() : NotFound();
    }
}
