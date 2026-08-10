namespace HR_Platform.API.Entities;

public class SalaryHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public decimal? OldSalary { get; set; }
    public decimal? NewSalary { get; set; }
    public string? ChangeReason { get; set; }
    public DateOnly? EffectiveDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual User? ApprovedByUser { get; set; }
}
