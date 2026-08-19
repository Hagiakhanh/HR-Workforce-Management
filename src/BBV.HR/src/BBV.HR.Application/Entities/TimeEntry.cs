namespace BBV.HR.Application.Entities;

public class TimeEntry
{
    public Guid Id { get; set; }

    public Guid TimesheetId { get; set; }
    public Timesheet Timesheet { get; set; } = null!;

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }

    public DateOnly WorkDate { get; set; }
    public string TimeType { get; set; } = string.Empty;

    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    public string? Notes { get; set; }
}
