namespace BBV.HR.Application.DTOs.ProjectEffort;

public class MemberEffortDto
{
    public Guid EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }

    public double TotalLoggedHours { get; set; }
    public int TimeEntriesCount { get; set; }
    public DateOnly? LastLoggedDate { get; set; }
}
