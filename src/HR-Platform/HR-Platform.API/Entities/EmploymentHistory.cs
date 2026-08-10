namespace HR_Platform.API.Entities;

public class EmploymentHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public Guid? RequestId { get; set; }
    public string? Category { get; set; }
    public string? EventType { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateOnly? EffectiveDate { get; set; }
    public Guid? PerformedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual Request? Request { get; set; }
    public virtual User? PerformedByUser { get; set; }
}
