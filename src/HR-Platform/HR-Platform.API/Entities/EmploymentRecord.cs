namespace HR_Platform.API.Entities;

public class EmploymentRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? PositionId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid? WorkLocationId { get; set; }
    public string? EmploymentType { get; set; }
    public string? WorkerType { get; set; }
    public string? JobLevel { get; set; }
    public string? WorkMode { get; set; }
    public int? WeeklyHours { get; set; }
    public string? TimeZone { get; set; }
    public string? PayFrequency { get; set; }
    public string? Currency { get; set; }
    public decimal? SalaryAmount { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual Department? Department { get; set; }
    public virtual Position? Position { get; set; }
    public virtual Team? Team { get; set; }
    public virtual Employee? Manager { get; set; }
    public virtual WorkLocation? WorkLocation { get; set; }
}
