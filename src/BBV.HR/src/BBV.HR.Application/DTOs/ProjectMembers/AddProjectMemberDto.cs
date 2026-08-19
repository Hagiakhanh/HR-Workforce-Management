namespace BBV.HR.Application.DTOs.ProjectMembers;

public class AddProjectMemberDto
{
    public Guid EmployeeId { get; set; }
    public string? ProjectRole { get; set; }
    public int? AllocationPct { get; set; }
    public string? Status { get; set; } = "Active";

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
