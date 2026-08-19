namespace BBV.HR.Application.DTOs.ProjectEffort;

public class ProjectEffortSummaryDto
{
    public Guid ProjectId { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;

    public double TotalLoggedHours { get; set; }
    public int TotalTimeEntriesCount { get; set; }
    public int TotalActiveMembersLogged { get; set; }
}
