using BBV.HR.Application.DTOs.Projects;
using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Mappings;

public static class ProjectMappingExtensions
{
    public static ProjectDto ToDto(this Project p)
    {
        return new ProjectDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status,
            ManagerId = p.ManagerId,
            ManagerName = p.Manager != null ? $"{p.Manager.FirstName} {p.Manager.LastName}" : null,
            CreatedBy = p.CreatedBy,
            CreatorName = p.Creator != null ? $"{p.Creator.FirstName} {p.Creator.LastName}" : null,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            TotalBudget = p.TotalBudget,
            LaborBudget = p.LaborBudget,
            BudgetWarningThreshold = p.BudgetWarningThreshold,
            ExpectedTeamSizeFte = p.ExpectedTeamSizeFte,
            DefaultWeeklyCapacityH = p.DefaultWeeklyCapacityH,
            Client = p.Client,
            Priority = p.Priority,
            TechStack = p.TechStack,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        };
    }
}
