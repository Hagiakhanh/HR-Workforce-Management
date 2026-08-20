using BBV.HR.Api.Controllers;
using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.ProjectEffort;
using BBV.HR.Application.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BBV.HR.Tests.Controllers;

public class ProjectEffortControllerTests
{
    private readonly Mock<IProjectEffortService> _serviceMock;
    private readonly ProjectEffortController _controller;

    public ProjectEffortControllerTests()
    {
        _serviceMock = new Mock<IProjectEffortService>();
        _controller = new ProjectEffortController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetEffortSummary_WhenFound_ShouldReturnOk()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var summary = new ProjectEffortSummaryDto { ProjectId = projectId, TotalLoggedHours = 40 };
        _serviceMock.Setup(s => s.GetProjectEffortSummaryAsync(projectId)).ReturnsAsync(summary);

        // Act
        var result = await _controller.GetEffortSummary(projectId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(summary);
    }

    [Fact]
    public async Task GetEffortSummary_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetProjectEffortSummaryAsync(projectId)).ReturnsAsync((ProjectEffortSummaryDto?)null);

        // Act
        Func<Task> act = async () => await _controller.GetEffortSummary(projectId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetMemberEffort_ShouldReturnOk()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var memberEfforts = new List<MemberEffortDto> { new() { EmployeeId = Guid.NewGuid(), TotalLoggedHours = 20 } };
        _serviceMock.Setup(s => s.GetProjectMemberEffortAsync(projectId)).ReturnsAsync(memberEfforts);

        // Act
        var result = await _controller.GetMemberEffort(projectId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetTimeEntries_ShouldReturnOk()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var entries = new List<ProjectTimeEntryDto> { new() { Id = Guid.NewGuid(), LoggedHours = 8 } };
        _serviceMock.Setup(s => s.GetProjectTimeEntriesAsync(projectId)).ReturnsAsync(entries);

        // Act
        var result = await _controller.GetTimeEntries(projectId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }
}
