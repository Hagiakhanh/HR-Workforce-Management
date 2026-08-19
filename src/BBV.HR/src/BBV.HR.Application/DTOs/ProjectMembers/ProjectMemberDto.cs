namespace BBV.HR.Application.DTOs.ProjectMembers;

public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }

    public string? ProjectRole { get; set; }
    public int? AllocationPct { get; set; }
    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public DateTime? JoinedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
