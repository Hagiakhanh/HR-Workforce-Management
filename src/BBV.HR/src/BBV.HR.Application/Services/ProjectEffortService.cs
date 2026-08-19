using BBV.HR.Application.DTOs.ProjectEffort;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Interfaces.Services;
using BBV.HR.Application.Mappings;

namespace BBV.HR.Application.Services;

public class ProjectEffortService : IProjectEffortService
{
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IProjectRepository _projectRepository;

    public ProjectEffortService(
        ITimeEntryRepository timeEntryRepository,
        IProjectRepository projectRepository)
    {
        _timeEntryRepository = timeEntryRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ProjectEffortSummaryDto?> GetProjectEffortSummaryAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null) return null;

        var timeEntries = (await _timeEntryRepository.GetByProjectIdAsync(projectId)).ToList();

        double totalHours = 0;
        foreach (var entry in timeEntries)
        {
            totalHours += ProjectEffortMappingExtensions.CalculateHours(entry.StartTime, entry.EndTime);
        }

        var activeMembersLoggedCount = timeEntries
            .Select(te => te.Timesheet.EmployeeId)
            .Distinct()
            .Count();

        return new ProjectEffortSummaryDto
        {
            ProjectId = project.Id,
            ProjectCode = project.Code,
            ProjectName = project.Name,
            TotalLoggedHours = totalHours,
            TotalTimeEntriesCount = timeEntries.Count,
            TotalActiveMembersLogged = activeMembersLoggedCount
        };
    }

    public async Task<IEnumerable<MemberEffortDto>> GetProjectMemberEffortAsync(Guid projectId)
    {
        var projectExists = await _projectRepository.ExistsAsync(projectId);
        if (!projectExists) return Enumerable.Empty<MemberEffortDto>();

        var timeEntries = await _timeEntryRepository.GetByProjectIdAsync(projectId);

        var grouped = timeEntries.GroupBy(te => te.Timesheet.EmployeeId);

        var result = new List<MemberEffortDto>();
        foreach (var group in grouped)
        {
            var employee = group.First().Timesheet.Employee;
            double hours = group.Sum(e => ProjectEffortMappingExtensions.CalculateHours(e.StartTime, e.EndTime));
            var lastLoggedDate = group.Max(e => e.WorkDate);

            result.Add(new MemberEffortDto
            {
                EmployeeId = group.Key,
                EmployeeCode = employee?.EmployeeCode,
                EmployeeName = employee != null ? $"{employee.FirstName} {employee.LastName}" : null,
                TotalLoggedHours = hours,
                TimeEntriesCount = group.Count(),
                LastLoggedDate = lastLoggedDate
            });
        }

        return result;
    }

    public async Task<IEnumerable<ProjectTimeEntryDto>> GetProjectTimeEntriesAsync(Guid projectId)
    {
        var timeEntries = await _timeEntryRepository.GetByProjectIdAsync(projectId);

        return timeEntries.Select(te => te.ToTimeEntryDto());
    }
}
