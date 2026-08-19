namespace BBV.HR.Application.Entities;

public class ProjectMember
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public string? ProjectRole { get; set; }
    public int? AllocationPct { get; set; }
    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public DateTime? JoinedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
