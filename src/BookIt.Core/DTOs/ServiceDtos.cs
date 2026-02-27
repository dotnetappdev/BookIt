using System.ComponentModel.DataAnnotations;
using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public int BufferMinutes { get; set; }
    public string? CategoryName { get; set; }
    public bool AllowOnlineBooking { get; set; }
    public MeetingType DefaultMeetingType { get; set; }
}

public class CreateServiceRequest
{
    [Required(ErrorMessage = "Service name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Service name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Slug must not exceed 200 characters.")]
    public string? Slug { get; set; }

    [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters.")]
    public string? Description { get; set; }

    [Url(ErrorMessage = "Image URL must be a valid URL.")]
    [StringLength(2000, ErrorMessage = "Image URL must not exceed 2000 characters.")]
    public string? ImageUrl { get; set; }

    [Range(0, 1000000, ErrorMessage = "Price must be between 0 and 1,000,000.")]
    public decimal Price { get; set; }

    [Range(5, 1440, ErrorMessage = "Duration must be between 5 and 1440 minutes.")]
    public int DurationMinutes { get; set; } = 60;

    [Range(0, 480, ErrorMessage = "Buffer minutes must be between 0 and 480.")]
    public int BufferMinutes { get; set; } = 0;

    public Guid? CategoryId { get; set; }
    public bool AllowOnlineBooking { get; set; } = true;
    public MeetingType DefaultMeetingType { get; set; } = MeetingType.InPerson;
}
