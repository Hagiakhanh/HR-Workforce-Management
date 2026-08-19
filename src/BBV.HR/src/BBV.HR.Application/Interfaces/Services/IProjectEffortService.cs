using BBV.HR.Application.DTOs.ProjectEffort;

namespace BBV.HR.Application.Interfaces.Services;

public interface IProjectEffortService
{
    Task<ProjectEffortSummaryDto?> GetProjectEffortSummaryAsync(Guid projectId);
    Task<IEnumerable<MemberEffortDto>> GetProjectMemberEffortAsync(Guid projectId);
    Task<IEnumerable<ProjectTimeEntryDto>> GetProjectTimeEntriesAsync(Guid projectId);
}
