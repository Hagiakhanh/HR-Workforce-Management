namespace BBV.HR.Application.DTOs.ProjectMembers;

public class UpdateProjectMemberDto
{
    public string? ProjectRole { get; set; }
    public int? AllocationPct { get; set; }
    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
