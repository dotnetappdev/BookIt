using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class BookingForm : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public FormPublishStatus PublishStatus { get; set; } = FormPublishStatus.Draft;
    public string? WelcomeMessage { get; set; }
    public string? ConfirmationMessage { get; set; }
    public bool CollectPhone { get; set; } = true;
    public bool CollectNotes { get; set; } = true;
    public bool RequirePhoneVerification { get; set; }
    public ICollection<BookingFormField> Fields { get; set; } = new List<BookingFormField>();
}
