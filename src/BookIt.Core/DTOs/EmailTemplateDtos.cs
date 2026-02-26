using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class EmailTemplateResponse
{
    public Guid Id { get; set; }
    public EmailTemplateType TemplateType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SubjectLine { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UpsertEmailTemplateRequest
{
    public EmailTemplateType TemplateType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SubjectLine { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
