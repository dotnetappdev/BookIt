namespace BookIt.Core.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public DateTime? EditedAt { get; set; }
    public string? EditedBy { get; set; }

    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
