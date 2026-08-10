namespace HR_Platform.API.Entities;

public class ReportingLine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public Guid ManagerId { get; set; }
    public string? ReportingType { get; set; }
    public DateOnly? EffectiveDate { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual Employee Manager { get; set; } = null!;
}
