namespace BBV.HR.Application.DTOs.Projects;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;

    public Guid? ManagerId { get; set; }
    public string? ManagerName { get; set; }

    public Guid? CreatedBy { get; set; }
    public string? CreatorName { get; set; }

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
}
