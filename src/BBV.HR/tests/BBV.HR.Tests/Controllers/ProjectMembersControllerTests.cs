using BBV.HR.Api.Controllers;
using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.ProjectMembers;
using BBV.HR.Application.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BBV.HR.Tests.Controllers;

public class ProjectMembersControllerTests
{
    private readonly Mock<IProjectMemberService> _serviceMock;
    private readonly ProjectMembersController _controller;

    public ProjectMembersControllerTests()
    {
        _serviceMock = new Mock<IProjectMemberService>();
        _controller = new ProjectMembersController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetMembers_ShouldReturnOkWithMembers()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var members = new List<ProjectMemberDto> { new() { Id = Guid.NewGuid(), ProjectRole = "Dev" } };
        _serviceMock.Setup(s => s.GetProjectMembersAsync(projectId)).ReturnsAsync(members);

        // Act
        var result = await _controller.GetMembers(projectId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetMemberById_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetProjectMemberByIdAsync(projectId, memberId)).ReturnsAsync((ProjectMemberDto?)null);

        // Act
        Func<Task> act = async () => await _controller.GetMemberById(projectId, memberId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddMember_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var addDto = new AddProjectMemberDto { EmployeeId = Guid.NewGuid(), ProjectRole = "Dev", AllocationPct = 100 };
        var createdDto = new ProjectMemberDto { Id = Guid.NewGuid(), ProjectId = projectId, ProjectRole = "Dev" };

        _serviceMock.Setup(s => s.AddProjectMemberAsync(projectId, addDto)).ReturnsAsync(createdDto);

        // Act
        var result = await _controller.AddMember(projectId, addDto);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task RemoveMember_WhenSuccessful_ShouldReturnNoContent()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        _serviceMock.Setup(s => s.RemoveProjectMemberAsync(projectId, memberId)).ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveMember(projectId, memberId);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }
}
