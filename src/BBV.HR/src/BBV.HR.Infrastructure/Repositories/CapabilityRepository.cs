using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBV.HR.Infrastructure.Repositories;

public class CapabilityRepository : ICapabilityRepository
{
    private readonly ApplicationDbContext _context;

    public CapabilityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Capability>> GetAllAsync(string? search, string? category)
    {
        var query = _context.Capabilities
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchLower) ||
                (c.Description != null && c.Description.ToLower().Contains(searchLower)));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(c => c.Category != null && c.Category.ToLower() == category.ToLower());
        }

        return await query
            .OrderBy(c => c.Category)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Capability?> GetByIdAsync(Guid id)
    {
        return await _context.Capabilities
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsNameAsync(string name, Guid? excludeId = null)
    {
        var nameLower = name.ToLower();
        return await _context.Capabilities
            .AnyAsync(c =>
                c.Name.ToLower() == nameLower &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
    }

    public async Task<bool> IsInUseAsync(Guid id)
    {
        var usedByEmployee = await _context.EmployeeCapabilities
            .AnyAsync(ec => ec.CapabilityId == id);

        var usedByProject = await _context.ProjectRequiredCapabilities
            .AnyAsync(prc => prc.CapabilityId == id);

        return usedByEmployee || usedByProject;
    }

    public async Task<Capability> AddAsync(Capability capability)
    {
        await _context.Capabilities.AddAsync(capability);
        return capability;
    }

    public Task UpdateAsync(Capability capability)
    {
        _context.Capabilities.Update(capability);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Capability capability)
    {
        _context.Capabilities.Remove(capability);
        return Task.CompletedTask;
    }
}