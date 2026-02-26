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
    public Guid ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int MaxCapacity { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public List<string> InstructorIds { get; set; } = new();
}

public class UpdateClassSessionRequest
{
    public Guid? ServiceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? SessionDate { get; set; }
    public string? StartTime { get; set; }
    public int? DurationMinutes { get; set; }
    public int? MaxCapacity { get; set; }
    public decimal? Price { get; set; }
    public string? Location { get; set; }
    public List<string>? InstructorIds { get; set; }
}
