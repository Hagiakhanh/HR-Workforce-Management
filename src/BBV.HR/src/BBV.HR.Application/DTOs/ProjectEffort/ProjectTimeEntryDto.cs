namespace BBV.HR.Application.DTOs.ProjectEffort;

public class ProjectTimeEntryDto
{
    public Guid Id { get; set; }
    public Guid TimesheetId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? EmployeeName { get; set; }

    public DateOnly WorkDate { get; set; }
    public string TimeType { get; set; } = string.Empty;

    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public double LoggedHours { get; set; }

    public string? Notes { get; set; }
}
