using System.ComponentModel.DataAnnotations;
using BookIt.Core.Enums;

namespace BookIt.Core.DTOs;

public class ClassSessionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentBookings { get; set; }
    public decimal Price { get; set; }
    public SessionStatus Status { get; set; }
    public string StatusLabel => Status switch
    {
        SessionStatus.Scheduled => "Scheduled",
        SessionStatus.InProgress => "In Progress",
        SessionStatus.Completed => "Completed",
        SessionStatus.Cancelled => "Cancelled",
        SessionStatus.Full => "Full",
        _ => "Unknown"
    };
    public string? Location { get; set; }
    public Guid? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public List<string> InstructorIds { get; set; } = new();
}

public class CreateClassSessionRequest
{
    [Required(ErrorMessage = "Service is required.")]
    public Guid ServiceId { get; set; }

    [Required(ErrorMessage = "Class name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Class name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Session date is required.")]
    public DateTime SessionDate { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public string StartTime { get; set; } = string.Empty;

    [Range(5, 1440, ErrorMessage = "Duration must be between 5 and 1440 minutes.")]
    public int DurationMinutes { get; set; }

    [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10,000.")]
    public int MaxCapacity { get; set; }

    [Range(0, 1000000, ErrorMessage = "Price must be between 0 and 1,000,000.")]
    public decimal Price { get; set; }

    [StringLength(200, ErrorMessage = "Location must not exceed 200 characters.")]
    public string? Location { get; set; }

    public List<string> InstructorIds { get; set; } = new();
}

public class UpdateClassSessionRequest
{
    public Guid? ServiceId { get; set; }

    [StringLength(200, MinimumLength = 2, ErrorMessage = "Class name must be between 2 and 200 characters.")]
    public string? Name { get; set; }

    [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters.")]
    public string? Description { get; set; }

    public DateTime? SessionDate { get; set; }
    public string? StartTime { get; set; }

    [Range(5, 1440, ErrorMessage = "Duration must be between 5 and 1440 minutes.")]
    public int? DurationMinutes { get; set; }

    [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10,000.")]
    public int? MaxCapacity { get; set; }

    [Range(0, 1000000, ErrorMessage = "Price must be between 0 and 1,000,000.")]
    public decimal? Price { get; set; }

    [StringLength(200, ErrorMessage = "Location must not exceed 200 characters.")]
    public string? Location { get; set; }

    public List<string>? InstructorIds { get; set; }
}
