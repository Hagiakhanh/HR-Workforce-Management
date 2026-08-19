using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BBV.HR.Infrastructure.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TimeEntryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TimeEntry>> GetByProjectIdAsync(Guid projectId)
    {
        return await _dbContext.TimeEntries
            .Include(te => te.Timesheet)
                .ThenInclude(t => t.Employee)
            .Where(te => te.ProjectId == projectId)
            .OrderByDescending(te => te.WorkDate)
            .AsNoTracking()
            .ToListAsync();
    }
}
