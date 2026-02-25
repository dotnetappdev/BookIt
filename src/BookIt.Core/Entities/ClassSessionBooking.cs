namespace BookIt.Core.Entities;

/// <summary>
/// Links an appointment to a class session (many-to-many)
/// </summary>
public class ClassSessionBooking
{
    public Guid ClassSessionId { get; set; }
    public ClassSession ClassSession { get; set; } = null!;
    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public int ParticipantCount { get; set; } = 1;
}
