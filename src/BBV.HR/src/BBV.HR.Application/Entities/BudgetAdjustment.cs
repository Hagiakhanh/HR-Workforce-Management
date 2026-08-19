namespace BBV.HR.Application.Entities;

public class BudgetAdjustment
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string AdjustmentType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? BudgetCategory { get; set; }

    public string? Reason { get; set; }
    public string Status { get; set; } = string.Empty;

    public Guid? RequestedBy { get; set; }
    public Employee? Requester { get; set; }

    public Guid? ApprovedBy { get; set; }
    public Employee? Approver { get; set; }

    public DateTime? ApprovedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}
