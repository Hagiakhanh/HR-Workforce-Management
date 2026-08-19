namespace BBV.HR.Application.DTOs.Projects;

public class UpdateProjectDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }

    public Guid? ManagerId { get; set; }

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
}
