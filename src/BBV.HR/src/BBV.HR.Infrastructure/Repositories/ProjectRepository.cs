using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBV.HR.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(string? search, string? status, Guid? managerId)
    {
        var query = _dbContext.Projects
            .Include(p => p.Manager)
            .Include(p => p.Creator)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchLower) || p.Code.ToLower().Contains(searchLower));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(p => p.Status.ToLower() == status.ToLower());
        }

        if (managerId.HasValue)
        {
            query = query.Where(p => p.ManagerId == managerId.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await _dbContext.Projects
            .Include(p => p.Manager)
            .Include(p => p.Creator)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<bool> ExistsCodeAsync(string code, Guid? excludeProjectId = null)
    {
        var codeLower = code.ToLower();
        return await _dbContext.Projects
            .AnyAsync(p => p.Code.ToLower() == codeLower && (!excludeProjectId.HasValue || p.Id != excludeProjectId.Value));
    }

    public async Task<bool> ExistsAsync(Guid projectId)
    {
        return await _dbContext.Projects.AnyAsync(p => p.Id == projectId);
    }

    public async Task<Project> AddAsync(Project project)
    {
        await _dbContext.Projects.AddAsync(project);
        return project;
    }

    public Task UpdateAsync(Project project)
    {
        _dbContext.Projects.Update(project);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Project project)
    {
        _dbContext.Projects.Remove(project);
        return Task.CompletedTask;
    }
}
