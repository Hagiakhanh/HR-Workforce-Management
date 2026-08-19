namespace BBV.HR.Application.Entities;

public class Timesheet
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateOnly WeekStartDate { get; set; }
    public string Status { get; set; } = string.Empty;

    public DateTime? SubmittedAt { get; set; }

    public Guid? ReviewedBy { get; set; }
    public Employee? Reviewer { get; set; }

    public DateTime? ReviewedAt { get; set; }
    public string? ReviewerNotes { get; set; }

    // Navigation properties
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
}
