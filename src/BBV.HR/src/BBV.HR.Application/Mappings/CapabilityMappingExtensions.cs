using BBV.HR.Application.DTOs.Capability;
using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Mappings;

public static class CapabilityMappingExtensions
{
    public static CapabilityDto ToDto(this Capability entity) 
    {
        return new CapabilityDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Category = entity.Category,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}