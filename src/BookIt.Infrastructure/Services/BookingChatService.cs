using BookIt.Core.DTOs;
using BookIt.Core.Interfaces;
using BookIt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookIt.Infrastructure.Services;

public class BookingChatService : IChatService
{
    private readonly BookItDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BookingChatService> _logger;

    public BookingChatService(BookItDbContext context, IHttpClientFactory httpClientFactory, ILogger<BookingChatService> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ChatResponse> ProcessAsync(string tenantSlug, ChatRequest request)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == tenantSlug && !t.IsDeleted);
        if (tenant == null)
            return Reply("Sorry, I couldn't find this business.");

        // Try OpenAI first if configured
        if (!string.IsNullOrEmpty(tenant.OpenAiApiKey))
        {
            try
            {
                return await ProcessWithOpenAiAsync(tenant, tenantSlug, request);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "OpenAI chat failed, falling back to rule-based");
            }
        }

        return await ProcessRuleBasedAsync(tenant, tenantSlug, request);
    }

    // ── Rule-based processing ──────────────────────────────────────────────────
    private async Task<ChatResponse> ProcessRuleBasedAsync(Core.Entities.Tenant tenant, string tenantSlug, ChatRequest request)
    {
        var msg = request.Message.Trim().ToLowerInvariant();

        // PIN lookup: detect "pin XXXXX" or just "XXXXXX" (6 upper chars)
        var pinMatch = System.Text.RegularExpressions.Regex.Match(request.Message.Trim(), @"\b([A-Z2-9]{6})\b");
        if (pinMatch.Success || msg.Contains("pin") || msg.Contains("my booking") || msg.Contains("look up") || msg.Contains("find booking"))
        {
            var rawPin = pinMatch.Success ? pinMatch.Value : ExtractPin(request.Message);
            if (rawPin != null)
            {
                var appt = await _context.Appointments
                    .Include(a => a.Services).ThenInclude(s => s.Service)
                    .FirstOrDefaultAsync(a => a.TenantId == tenant.Id && a.BookingPin == rawPin && !a.IsDeleted);
                if (appt != null)
                {
                    var svcList = string.Join(", ", appt.Services.Select(s => s.Service?.Name ?? "Service"));
                    return Reply(
                        $"📋 I found your booking!\n\n**{svcList}**\n📅 {appt.StartTime:dddd, d MMMM} at {appt.StartTime:h:mm tt}\n👤 {appt.CustomerName}\n🔖 Status: {appt.Status}\n\nIs there anything else I can help with?",
                        quickReplies: new[] { "Book another appointment", "Help", "What services are available?" }
                    );
                }
                else
                {
                    return Reply("I couldn't find a booking with that PIN. Please double-check and try again, or contact us directly.",
                        quickReplies: new[] { "Book a new appointment", "Help" });
                }
            }
        }

        // Greetings
        if (msg is "hi" or "hello" or "hey" or "good morning" or "good afternoon" or "start")
        {
            var services = await _context.Services
                .Where(s => s.TenantId == tenant.Id && s.IsActive && !s.IsDeleted)
                .Take(4).ToListAsync();
            var svcNames = services.Select(s => s.Name).ToList();
            return Reply(
                $"👋 Hi! I'm the booking assistant for **{tenant.Name}**. I can help you book an appointment, check availability, or look up an existing booking.\n\nWhat would you like to do?",
                quickReplies: svcNames.Concat(new[] { "Look up my booking", "Help" }).Take(5).ToArray()
            );
        }

        // Help
        if (msg.Contains("help") || msg.Contains("what can you do"))
        {
            return Reply(
                "Here's what I can help with:\n\n• **Book an appointment** – tell me what service you need\n• **Check availability** – I'll show you open slots\n• **Look up a booking** – share your 6-character PIN (e.g. AB3X7K)\n• **Cancel or reschedule** – I'll guide you through it\n\nWhat would you like to do?",
                quickReplies: new[] { "Book an appointment", "Look up booking with PIN", "What services are available?" }
            );
        }

        // Services list
        if (msg.Contains("service") || msg.Contains("what do you offer") || msg.Contains("available") || msg.Contains("menu") || msg.Contains("price"))
        {
            var services = await _context.Services
                .Where(s => s.TenantId == tenant.Id && s.IsActive && !s.IsDeleted)
                .OrderBy(s => s.SortOrder)
                .ToListAsync();
            if (!services.Any())
                return Reply("There are no services listed yet. Please contact us directly to book.");

            var list = string.Join("\n", services.Select(s => $"• **{s.Name}** – {tenant.Currency}{s.Price:F2} ({s.DurationMinutes} min)"));
            return Reply(
                $"Here are the available services:\n\n{list}\n\nWhich one would you like to book?",
                quickReplies: services.Take(5).Select(s => s.Name).ToArray(),
                action: new ChatAction { Type = "redirect", Url = $"/{tenantSlug}/book" }
            );
        }

        // Book intent
        if (msg.Contains("book") || msg.Contains("appointment") || msg.Contains("schedule") || msg.Contains("reserve"))
        {
            var services = await _context.Services
                .Where(s => s.TenantId == tenant.Id && s.IsActive && !s.IsDeleted)
                .ToListAsync();
            var matched = services.FirstOrDefault(s => msg.Contains(s.Name.ToLowerInvariant()));

            if (matched != null)
            {
                return Reply(
                    $"Great choice! Let me take you to the booking page for **{matched.Name}**.",
                    quickReplies: new[] { "Yes, let's go!", "Show me all services" },
                    action: new ChatAction { Type = "redirect", Url = $"/{tenantSlug}/booking/formstep?serviceId={matched.Id}" }
                );
            }

            return Reply(
                "I'd be happy to help you book! Which service are you interested in?",
                quickReplies: services.Take(5).Select(s => s.Name).ToArray(),
                action: new ChatAction { Type = "redirect", Url = $"/{tenantSlug}/book" }
            );
        }

        // Cancel intent
        if (msg.Contains("cancel") || msg.Contains("cancelation"))
        {
            return Reply(
                "To cancel your booking, please provide your 6-character booking PIN (found in your confirmation email).",
                quickReplies: new[] { "I have my PIN", "I don't have my PIN" }
            );
        }

        // Opening hours
        if (msg.Contains("hour") || msg.Contains("open") || msg.Contains("when"))
        {
            var hours = await _context.BusinessHours
                .Where(h => h.TenantId == tenant.Id && !h.IsClosed && !h.IsDeleted)
                .OrderBy(h => h.DayOfWeek)
                .ToListAsync();
            if (hours.Any())
            {
                var hoursText = string.Join("\n", hours.Select(h => $"• **{h.DayOfWeek}**: {h.OpenTime:hh\\:mm} – {h.CloseTime:hh\\:mm}"));
                return Reply($"Our opening hours are:\n\n{hoursText}", quickReplies: new[] { "Book an appointment", "Services & prices" });
            }
        }

        // Fallback
        return Reply(
            "I'm not sure I understood that. I can help you **book an appointment**, **check our services**, or **look up an existing booking** with your PIN.",
            quickReplies: new[] { "Book an appointment", "View services", "Look up booking with PIN", "Opening hours" }
        );
    }

    // ── OpenAI processing ──────────────────────────────────────────────────────
    private async Task<ChatResponse> ProcessWithOpenAiAsync(Core.Entities.Tenant tenant, string tenantSlug, ChatRequest request)
    {
        var services = await _context.Services
            .Where(s => s.TenantId == tenant.Id && s.IsActive && !s.IsDeleted)
            .Select(s => new { s.Name, s.Price, s.DurationMinutes, s.Description })
            .ToListAsync();

        var serviceList = string.Join(", ", services.Select(s => $"{s.Name} (£{s.Price:F2}, {s.DurationMinutes}min)"));

        var systemPrompt = $@"You are a friendly, concise booking assistant for {tenant.Name}. 
Help customers book appointments, check services, and look up existing bookings.
Available services: {serviceList}
Booking URL: /{tenantSlug}/book
Keep responses short (2-3 sentences max). Use markdown bold for service names and times.
If the customer wants to book, provide the booking URL. If they give a 6-character PIN (like AB3X7K), acknowledge and offer to look it up.
Always end with 1-3 quick reply suggestions in JSON format on the last line: QUICK_REPLIES:[""Option 1"",""Option 2""]";

        var messages = new List<object>
        {
            new { role = "system", content = systemPrompt }
        };
        foreach (var h in request.History.TakeLast(10))
            messages.Add(new { role = h.Role, content = h.Content });
        messages.Add(new { role = "user", content = request.Message });

        var http = _httpClientFactory.CreateClient();
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tenant.OpenAiApiKey);

        var body = JsonSerializer.Serialize(new
        {
            model = "gpt-3.5-turbo",
            messages,
            max_tokens = 200,
            temperature = 0.7
        });

        var res = await http.PostAsync("https://api.openai.com/v1/chat/completions",
            new StringContent(body, Encoding.UTF8, "application/json"));

        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "";

        // Parse quick replies from the response
        var quickReplies = new List<string>();
        var qrMatch = System.Text.RegularExpressions.Regex.Match(content, @"QUICK_REPLIES:\[(.*?)\]");
        if (qrMatch.Success)
        {
            content = content[..qrMatch.Index].Trim();
            try
            {
                quickReplies = JsonSerializer.Deserialize<List<string>>("[" + qrMatch.Groups[1].Value + "]") ?? new();
            }
            catch { }
        }
        if (!quickReplies.Any())
            quickReplies = new List<string> { "Book an appointment", "View services", "Help" };

        return new ChatResponse
        {
            Message = content,
            QuickReplies = quickReplies,
            UseElevenLabs = !string.IsNullOrEmpty(tenant.ElevenLabsApiKey),
            ElevenLabsApiKey = tenant.ElevenLabsApiKey,
            ElevenLabsVoiceId = tenant.ElevenLabsVoiceId
        };
    }

    // ── Helpers ────────────────────────────────────────────────────────────────
    private static string? ExtractPin(string input)
    {
        var m = System.Text.RegularExpressions.Regex.Match(input.ToUpperInvariant(), @"[A-Z2-9]{6}");
        return m.Success ? m.Value : null;
    }

    private static ChatResponse Reply(string message, string[]? quickReplies = null, ChatAction? action = null)
        => new()
        {
            Message = message,
            QuickReplies = quickReplies?.ToList() ?? new List<string> { "Book an appointment", "Help" },
            Action = action
        };
}
