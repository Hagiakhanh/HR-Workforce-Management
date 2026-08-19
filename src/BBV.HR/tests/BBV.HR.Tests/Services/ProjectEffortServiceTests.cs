using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Services;
using FluentAssertions;
using Moq;

namespace BBV.HR.Tests.Services;

public class ProjectEffortServiceTests
{
    private readonly Mock<ITimeEntryRepository> _timeEntryRepoMock;
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly ProjectEffortService _effortService;

    public ProjectEffortServiceTests()
    {
        _timeEntryRepoMock = new Mock<ITimeEntryRepository>();
        _projectRepoMock = new Mock<IProjectRepository>();

        _effortService = new ProjectEffortService(
            _timeEntryRepoMock.Object,
            _projectRepoMock.Object
        );
    }

    [Fact]
    public async Task GetProjectEffortSummaryAsync_WhenProjectExists_ShouldCalculateTotalHoursAndMembers()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var emp1 = new Employee { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };
        var emp2 = new Employee { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" };

        var project = new Project { Id = projectId, Code = "PRJ-EF", Name = "Effort Test Project" };

        var timeEntries = new List<TimeEntry>
        {
            new TimeEntry
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                WorkDate = new DateOnly(2026, 8, 10),
                Timesheet = new Timesheet { EmployeeId = emp1.Id, Employee = emp1 },
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(17, 0) // 8 hours
            },
            new TimeEntry
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                WorkDate = new DateOnly(2026, 8, 11),
                Timesheet = new Timesheet { EmployeeId = emp2.Id, Employee = emp2 },
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(14, 0) // 4 hours
            }
        };

        _projectRepoMock.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync(project);
        _timeEntryRepoMock.Setup(r => r.GetByProjectIdAsync(projectId)).ReturnsAsync(timeEntries);

        // Act
        var summary = await _effortService.GetProjectEffortSummaryAsync(projectId);

        // Assert
        summary.Should().NotBeNull();
        summary!.ProjectId.Should().Be(projectId);
        summary.TotalLoggedHours.Should().Be(12.0); // 8 + 4
        summary.TotalTimeEntriesCount.Should().Be(2);
        summary.TotalActiveMembersLogged.Should().Be(2);
    }

    [Fact]
    public async Task GetProjectEffortSummaryAsync_WhenProjectNotFound_ShouldReturnNull()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepoMock.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync((Project?)null);

        // Act
        var summary = await _effortService.GetProjectEffortSummaryAsync(projectId);

        // Assert
        summary.Should().BeNull();
    }

    [Fact]
    public async Task GetProjectMemberEffortAsync_ShouldGroupHoursByEmployee()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var emp1 = new Employee { Id = Guid.NewGuid(), EmployeeCode = "EMP01", FirstName = "John", LastName = "Doe" };

        var timeEntries = new List<TimeEntry>
        {
            new TimeEntry
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                WorkDate = new DateOnly(2026, 8, 10),
                Timesheet = new Timesheet { EmployeeId = emp1.Id, Employee = emp1 },
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(13, 0) // 4 hours
            },
            new TimeEntry
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                WorkDate = new DateOnly(2026, 8, 11),
                Timesheet = new Timesheet { EmployeeId = emp1.Id, Employee = emp1 },
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(18, 0) // 4 hours
            }
        };

        _projectRepoMock.Setup(r => r.ExistsAsync(projectId)).ReturnsAsync(true);
        _timeEntryRepoMock.Setup(r => r.GetByProjectIdAsync(projectId)).ReturnsAsync(timeEntries);

        // Act
        var memberEfforts = (await _effortService.GetProjectMemberEffortAsync(projectId)).ToList();

        // Assert
        memberEfforts.Should().NotBeNull();
        memberEfforts.Should().HaveCount(1);
        memberEfforts.First().EmployeeId.Should().Be(emp1.Id);
        memberEfforts.First().TotalLoggedHours.Should().Be(8.0);
        memberEfforts.First().TimeEntriesCount.Should().Be(2);
    }
}
