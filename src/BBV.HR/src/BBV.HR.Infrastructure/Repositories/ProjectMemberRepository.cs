using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBV.HR.Infrastructure.Repositories;

public class ProjectMemberRepository : IProjectMemberRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectMemberRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProjectMember>> GetByProjectIdAsync(Guid projectId)
    {
        return await _dbContext.ProjectMembers
            .Include(pm => pm.Employee)
            .Where(pm => pm.ProjectId == projectId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ProjectMember?> GetByIdAsync(Guid projectId, Guid memberId)
    {
        return await _dbContext.ProjectMembers
            .Include(pm => pm.Employee)
            .AsNoTracking()
            .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.Id == memberId);
    }

    public async Task<bool> IsMemberAsync(Guid projectId, Guid employeeId)
    {
        return await _dbContext.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == projectId && pm.EmployeeId == employeeId);
    }

    public async Task<ProjectMember> AddAsync(ProjectMember member)
    {
        await _dbContext.ProjectMembers.AddAsync(member);
        return member;
    }

    public Task UpdateAsync(ProjectMember member)
    {
        _dbContext.ProjectMembers.Update(member);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ProjectMember member)
    {
        _dbContext.ProjectMembers.Remove(member);
        return Task.CompletedTask;
    }
}
