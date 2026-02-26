using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

/// <summary>
/// Stores a tenant's custom email template for a specific notification type.
/// Placeholders use double-curly syntax: {{CustomerName}}, {{ServiceName}}, etc.
/// </summary>
public class EmailTemplate : BaseEntity
{
    public Guid TenantId { get; set; }
    public EmailTemplateType TemplateType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SubjectLine { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public Tenant Tenant { get; set; } = null!;
}
