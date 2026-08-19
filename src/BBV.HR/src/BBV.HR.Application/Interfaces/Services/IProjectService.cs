using BBV.HR.Application.DTOs.Projects;

namespace BBV.HR.Application.Interfaces.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync(string? search, string? status, Guid? managerId);
    Task<ProjectDto?> GetProjectByIdAsync(Guid projectId);
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto);
    Task<ProjectDto?> UpdateProjectAsync(Guid projectId, UpdateProjectDto dto);
    Task<bool> DeleteProjectAsync(Guid projectId);
}
