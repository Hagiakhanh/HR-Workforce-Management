namespace HR_Platform.API.Entities;

public class Request
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RequestCode { get; set; } = string.Empty;
    public Guid RequesterId { get; set; }
    public Guid RequestTypeId { get; set; }
    public Guid? HandoverAssigneeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ReasonNotes { get; set; }
    public string? ContactPhone { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? CalculatedDays { get; set; }
    public string? Priority { get; set; }
    public string? CurrentStage { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? SlaDeadlineAt { get; set; }
    public DateTime? SubmittedAt { get; set; }

    // Navigation properties
    public virtual Employee Requester { get; set; } = null!;
    public virtual RequestType RequestType { get; set; } = null!;
    public virtual Employee? HandoverAssignee { get; set; }
    public virtual ICollection<RequestAttachment> RequestAttachments { get; set; } = new List<RequestAttachment>();
    public virtual ICollection<ApprovalStep> ApprovalSteps { get; set; } = new List<ApprovalStep>();
    public virtual ICollection<RequestActivityLog> RequestActivityLogs { get; set; } = new List<RequestActivityLog>();
    public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();
}
