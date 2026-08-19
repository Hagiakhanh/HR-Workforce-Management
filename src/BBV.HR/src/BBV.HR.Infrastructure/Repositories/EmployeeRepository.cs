using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BBV.HR.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsAsync(Guid employeeId)
    {
        return await _dbContext.Employees.AnyAsync(e => e.Id == employeeId);
    }

    public async Task<Employee?> GetByIdAsync(Guid employeeId)
    {
        return await _dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == employeeId);
    }
}
