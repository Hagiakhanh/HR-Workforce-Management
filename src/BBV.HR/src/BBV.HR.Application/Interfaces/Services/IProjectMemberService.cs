using BBV.HR.Application.DTOs.ProjectMembers;

namespace BBV.HR.Application.Interfaces.Services;

public interface IProjectMemberService
{
    Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(Guid projectId);
    Task<ProjectMemberDto?> GetProjectMemberByIdAsync(Guid projectId, Guid memberId);
    Task<ProjectMemberDto> AddProjectMemberAsync(Guid projectId, AddProjectMemberDto dto);
    Task<ProjectMemberDto?> UpdateProjectMemberAsync(Guid projectId, Guid memberId, UpdateProjectMemberDto dto);
    Task<bool> RemoveProjectMemberAsync(Guid projectId, Guid memberId);
    Task<ProjectMemberDto?> UpdateMemberAllocationAsync(Guid projectId, Guid memberId, UpdateMemberAllocationDto dto);
}
