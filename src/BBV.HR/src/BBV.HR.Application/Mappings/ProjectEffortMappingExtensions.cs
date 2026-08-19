using BBV.HR.Application.DTOs.ProjectEffort;
using BBV.HR.Application.Entities;

namespace BBV.HR.Application.Mappings;

public static class ProjectEffortMappingExtensions
{
    public static ProjectTimeEntryDto ToTimeEntryDto(this TimeEntry te)
    {
        return new ProjectTimeEntryDto
        {
            Id = te.Id,
            TimesheetId = te.TimesheetId,
            EmployeeId = te.Timesheet.EmployeeId,
            EmployeeName = te.Timesheet.Employee != null ? $"{te.Timesheet.Employee.FirstName} {te.Timesheet.Employee.LastName}" : null,
            WorkDate = te.WorkDate,
            TimeType = te.TimeType,
            StartTime = te.StartTime,
            EndTime = te.EndTime,
            LoggedHours = CalculateHours(te.StartTime, te.EndTime),
            Notes = te.Notes
        };
    }

    public static double CalculateHours(TimeOnly? start, TimeOnly? end)
    {
        if (start.HasValue && end.HasValue)
        {
            var duration = end.Value - start.Value;
            return Math.Max(0, duration.TotalHours);
        }
        return 8.0;
    }
}
