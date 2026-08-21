using BBV.HR.Application.DTOs.EmployeeCapability;

namespace BBV.HR.Application.Interfaces.Services;

public interface IEmployeeCapabilityService
{
    Task<IEnumerable<EmployeeCapabilityDto>> GetEmployeeCapabilitiesAsync(Guid employeeId);
    Task<EmployeeCapabilityDto> AddEmployeeCapabilityAsync(Guid employeeId, AddEmployeeCapabilityDto dto);
    Task<EmployeeCapabilityDto?> UpdateEmployeeCapabilityAsync(Guid employeeId, Guid capabilityId, UpdateEmployeeCapabilityDto dto);
    Task<bool> RemoveEmployeeCapabilityAsync(Guid employeeId, Guid capabilityId);
}
