namespace HR_Platform.API.Entities;

public class LeaveQuota
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public int Year { get; set; }
    public int TotalDays { get; set; }
    public int UsedDays { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}
