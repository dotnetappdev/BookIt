using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class BookingFormResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public string? WelcomeMessage { get; set; }
    public string? ConfirmationMessage { get; set; }
    public bool CollectPhone { get; set; }
    public bool CollectNotes { get; set; }
    public List<BookingFormFieldResponse> Fields { get; set; } = new();
}

public class BookingFormFieldResponse
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public BookingFormFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public string? Placeholder { get; set; }
    public string? OptionsJson { get; set; }
    public List<string> Options => string.IsNullOrEmpty(OptionsJson)
        ? new()
        : System.Text.Json.JsonSerializer.Deserialize<List<string>>(OptionsJson) ?? new();
}

public class CreateBookingFormRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public string? WelcomeMessage { get; set; }
    public string? ConfirmationMessage { get; set; }
    public bool CollectPhone { get; set; } = true;
    public bool CollectNotes { get; set; } = true;
}

public class UpdateBookingFormRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public string? WelcomeMessage { get; set; }
    public string? ConfirmationMessage { get; set; }
    public bool CollectPhone { get; set; }
    public bool CollectNotes { get; set; }
}

public class AddFormFieldRequest
{
    public string Label { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public BookingFormFieldType FieldType { get; set; } = BookingFormFieldType.Text;
    public bool IsRequired { get; set; }
    public string? Placeholder { get; set; }
    public string? OptionsJson { get; set; }
    public int SortOrder { get; set; }
}

public class UpdateFormFieldRequest
{
    public string Label { get; set; } = string.Empty;
    public BookingFormFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public string? Placeholder { get; set; }
    public string? OptionsJson { get; set; }
}

public class ReorderFieldsRequest
{
    public List<Guid> FieldIds { get; set; } = new(); // ordered list
}
