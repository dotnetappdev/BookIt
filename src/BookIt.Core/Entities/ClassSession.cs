using BookIt.Core.Enums;

namespace BookIt.Core.Entities;

/// <summary>
/// A scheduled instance of a group class or session (e.g. Monday 9am Yoga, max 20 people)
/// Used by gyms, swimming pools, fitness studios
/// </summary>
public class ClassSession : BaseEntity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public Guid? StaffId { get; set; }
    public Staff? Staff { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime SessionDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public int MaxCapacity { get; set; } = 20;
    public int CurrentBookings { get; set; } = 0;
    public decimal Price { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Scheduled;
    public string? Location { get; set; }
    public ClassType ClassType { get; set; } = ClassType.General;

    /// <summary>JSON array of additional staff/instructor IDs (supports multiple instructors).</summary>
    public string? InstructorIdsJson { get; set; }

    public bool IsFull => CurrentBookings >= MaxCapacity;
    public int SpotsRemaining => Math.Max(0, MaxCapacity - CurrentBookings);

    /// <summary>All instructor Staff IDs (primary + additional).</summary>
    public List<Guid> InstructorIds
    {
        get
        {
            var ids = string.IsNullOrEmpty(InstructorIdsJson)
                ? new List<Guid>()
                : System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(InstructorIdsJson) ?? new();
            if (StaffId.HasValue)
            {
                var set = new HashSet<Guid>(ids);
                if (set.Add(StaffId.Value))
                    ids.Insert(0, StaffId.Value);
            }
            return ids;
        }
    }

    public ICollection<ClassSessionBooking> Bookings { get; set; } = new List<ClassSessionBooking>();
}
