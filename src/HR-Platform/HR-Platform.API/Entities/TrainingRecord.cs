namespace HR_Platform.API.Entities;

public class TrainingRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public DateOnly? StartDate { get; set; }
    public int? DurationHours { get; set; }
    public string? Result { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}
