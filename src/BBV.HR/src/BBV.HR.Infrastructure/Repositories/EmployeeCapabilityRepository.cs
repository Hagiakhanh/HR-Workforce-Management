using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBV.HR.Infrastructure.Repositories;

public class EmployeeCapabilityRepository : IEmployeeCapabilityRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeCapabilityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeCapability>> GetByEmployeeIdAsync(Guid employeeId)
    {
        return await _context.EmployeeCapabilities
            .Include(ec => ec.Capability)
            .AsNoTracking()
            .Where(ec => ec.EmployeeId == employeeId)
            .OrderBy(ec => ec.Capability.Category)
            .ThenBy(ec => ec.Capability.Name)
            .ToListAsync();
    }

    public async Task<EmployeeCapability?> GetByEmployeeAndCapabilityIdAsync(Guid employeeId, Guid capabilityId)
    {
        return await _context.EmployeeCapabilities
            .Include(ec => ec.Capability)
            .FirstOrDefaultAsync(ec => ec.EmployeeId == employeeId && ec.CapabilityId == capabilityId);
    }

    public async Task<EmployeeCapability?> GetByIdAsync(Guid id)
    {
        return await _context.EmployeeCapabilities
            .Include(ec => ec.Capability)
            .AsNoTracking()
            .FirstOrDefaultAsync(ec => ec.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid employeeId, Guid capabilityId)
    {
        return await _context.EmployeeCapabilities
            .AnyAsync(ec => ec.EmployeeId == employeeId && ec.CapabilityId == capabilityId);
    }

    public async Task<EmployeeCapability> AddAsync(EmployeeCapability employeeCapability)
    {
        await _context.EmployeeCapabilities.AddAsync(employeeCapability);
        return employeeCapability;
    }

    public Task UpdateAsync(EmployeeCapability employeeCapability)
    {
        _context.EmployeeCapabilities.Update(employeeCapability);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(EmployeeCapability employeeCapability)
    {
        _context.EmployeeCapabilities.Remove(employeeCapability);
        return Task.CompletedTask;
    }
}
