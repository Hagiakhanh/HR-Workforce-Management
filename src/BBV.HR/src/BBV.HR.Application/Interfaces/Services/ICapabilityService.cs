using BBV.HR.Application.DTOs.Capability;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBV.HR.Application.Interfaces.Services
{
    public interface ICapabilityService
    {
        Task<IEnumerable<CapabilityDto>> GetAllCapabilitiesAsync(string? search, string? category);
        Task<CapabilityDto?> GetCapabilityByIdAsync(Guid id);
        Task<CapabilityDto> CreateCapabilityAsync(CreateCapabilityDto dto);
        Task<CapabilityDto?> UpdateCapabilityAsync(Guid id, UpdateCapabilityDto dto);
        Task<bool> DeleteCapabilityAsync(Guid id);
    }
}
