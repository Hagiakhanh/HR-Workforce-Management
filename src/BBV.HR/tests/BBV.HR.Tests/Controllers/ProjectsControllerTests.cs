using BBV.HR.Api.Controllers;
using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.Projects;
using BBV.HR.Application.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BBV.HR.Tests.Controllers;

public class ProjectsControllerTests
{
    private readonly Mock<IProjectService> _serviceMock;
    private readonly ProjectsController _controller;

    public ProjectsControllerTests()
    {
        _serviceMock = new Mock<IProjectService>();
        _controller = new ProjectsController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithProjects()
    {
        // Arrange
        var projects = new List<ProjectDto> { new() { Id = Guid.NewGuid(), Code = "P1", Name = "Project 1" } };
        _serviceMock.Setup(s => s.GetAllProjectsAsync(null, null, null)).ReturnsAsync(projects);

        // Act
        var result = await _controller.GetAll(null, null, null);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetProjectByIdAsync(id)).ReturnsAsync((ProjectDto?)null);

        // Act
        Func<Task> act = async () => await _controller.GetById(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateProjectDto { Code = "NEW", Name = "New Project", Status = "Active" };
        var createdDto = new ProjectDto { Id = Guid.NewGuid(), Code = "NEW", Name = "New Project" };
        _serviceMock.Setup(s => s.CreateProjectAsync(createDto)).ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Delete_WhenSuccessful_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteProjectAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }
}
