namespace BBV.HR.Application.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;

    public Guid? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public Guid? CreatedBy { get; set; }
    public Employee? Creator { get; set; }

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public decimal? TotalBudget { get; set; }
    public decimal? LaborBudget { get; set; }
    public int? BudgetWarningThreshold { get; set; }

    public int? ExpectedTeamSizeFte { get; set; }
    public int? DefaultWeeklyCapacityH { get; set; }

    public string? Client { get; set; }
    public string? Priority { get; set; }
    public string? TechStack { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
    public ICollection<ProjectRequiredCapability> ProjectRequiredCapabilities { get; set; } = new List<ProjectRequiredCapability>();
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    public ICollection<BudgetAdjustment> BudgetAdjustments { get; set; } = new List<BudgetAdjustment>();
    public ICollection<ProductivitySession> ProductivitySessions { get; set; } = new List<ProductivitySession>();
}
