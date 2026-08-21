using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Interfaces.Repositories;

public interface IEmployeeCapabilityRepository
{
    Task<IEnumerable<EmployeeCapability>> GetByEmployeeIdAsync(Guid employeeId);
    Task<EmployeeCapability?> GetByEmployeeAndCapabilityIdAsync(Guid employeeId, Guid capabilityId);
    Task<EmployeeCapability?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid employeeId, Guid capabilityId);
    Task<EmployeeCapability> AddAsync(EmployeeCapability employeeCapability);
    Task UpdateAsync(EmployeeCapability employeeCapability);
    Task DeleteAsync(EmployeeCapability employeeCapability);
}
