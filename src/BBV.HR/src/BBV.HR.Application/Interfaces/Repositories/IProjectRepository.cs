using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync(string? search, string? status, Guid? managerId);
    Task<Project?> GetByIdAsync(Guid projectId);
    Task<bool> ExistsCodeAsync(string code, Guid? excludeProjectId = null);
    Task<bool> ExistsAsync(Guid projectId);
    Task<Project> AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(Project project);
}
