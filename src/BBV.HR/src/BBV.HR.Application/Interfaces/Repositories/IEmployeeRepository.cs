using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<bool> ExistsAsync(Guid employeeId);
    Task<Employee?> GetByIdAsync(Guid employeeId);
}
