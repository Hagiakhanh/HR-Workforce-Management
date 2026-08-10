namespace HR_Platform.API.Entities;

public class ApprovalStep
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RequestId { get; set; }
    public int StepNumber { get; set; }
    public string StepName { get; set; } = string.Empty;
    public Guid? AssignedApproverId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ActionDate { get; set; }

    // Navigation properties
    public virtual Request Request { get; set; } = null!;
    public virtual Employee? AssignedApprover { get; set; }
}
