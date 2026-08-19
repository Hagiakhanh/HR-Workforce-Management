using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Interfaces.Repositories;

public interface ITimeEntryRepository
{
    Task<IEnumerable<TimeEntry>> GetByProjectIdAsync(Guid projectId);
}
