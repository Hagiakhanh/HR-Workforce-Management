using BBV.HR.Application.DTOs.EmployeeCapability;
using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Mappings;

public static class EmployeeCapabilityMappingExtensions
{
    public static EmployeeCapabilityDto ToDto(this EmployeeCapability entity)
    {
        return new EmployeeCapabilityDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            CapabilityId = entity.CapabilityId,
            CapabilityName = entity.Capability?.Name ?? string.Empty,
            Category = entity.Capability?.Category,
            CapabilityDescription = entity.Capability?.Description,
            ProficiencyLevel = entity.ProficiencyLevel,
            YearsExperience = entity.YearsExperience,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
