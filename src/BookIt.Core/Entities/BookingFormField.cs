using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

public class BookingFormField : BaseEntity
{
    public Guid BookingFormId { get; set; }
    public BookingForm BookingForm { get; set; } = null!;
    public string Label { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public BookingFormFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public string? Placeholder { get; set; }
    public string? OptionsJson { get; set; } // For select/checkbox options
    public string? ValidationRegex { get; set; }
    public string? ValidationMessage { get; set; }
}
