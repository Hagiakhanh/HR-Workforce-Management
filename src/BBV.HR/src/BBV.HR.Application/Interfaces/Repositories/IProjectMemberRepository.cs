using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Interfaces.Repositories;

public interface IProjectMemberRepository
{
    Task<IEnumerable<ProjectMember>> GetByProjectIdAsync(Guid projectId);
    Task<ProjectMember?> GetByIdAsync(Guid projectId, Guid memberId);
    Task<bool> IsMemberAsync(Guid projectId, Guid employeeId);
    Task<ProjectMember> AddAsync(ProjectMember member);
    Task UpdateAsync(ProjectMember member);
    Task DeleteAsync(ProjectMember member);
}
