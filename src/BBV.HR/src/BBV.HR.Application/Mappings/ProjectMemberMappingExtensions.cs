using BBV.HR.Application.DTOs.ProjectMembers;
using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Mappings;

public static class ProjectMemberMappingExtensions
{
    public static ProjectMemberDto ToDto(this ProjectMember pm)
    {
        return new ProjectMemberDto
        {
            Id = pm.Id,
            ProjectId = pm.ProjectId,
            EmployeeId = pm.EmployeeId,
            EmployeeCode = pm.Employee?.EmployeeCode,
            EmployeeName = pm.Employee != null ? $"{pm.Employee.FirstName} {pm.Employee.LastName}" : null,
            ProjectRole = pm.ProjectRole,
            AllocationPct = pm.AllocationPct,
            Status = pm.Status,
            StartDate = pm.StartDate,
            EndDate = pm.EndDate,
            JoinedAt = pm.JoinedAt,
            UpdatedAt = pm.UpdatedAt
        };
    }
}
