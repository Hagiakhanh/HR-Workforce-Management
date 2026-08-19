using BBV.HR.Application.DTOs.Projects;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Services;
using BBV.HR.Application.Validators;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BBV.HR.Tests.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _projectRepoMock = new Mock<IProjectRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _projectService = new ProjectService(
            _projectRepoMock.Object,
            _unitOfWorkMock.Object,
            new CreateProjectDtoValidator(),
            new UpdateProjectDtoValidator()
        );
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnProjectsList()
    {
        // Arrange
        var mockProjects = new List<Project>
        {
            new Project { Id = Guid.NewGuid(), Code = "PRJ-01", Name = "Alpha", Status = "Active" },
            new Project { Id = Guid.NewGuid(), Code = "PRJ-02", Name = "Beta", Status = "Planning" }
        };

        _projectRepoMock
            .Setup(r => r.GetAllAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<Guid?>()))
            .ReturnsAsync(mockProjects);

        // Act
        var result = await _projectService.GetAllProjectsAsync(null, null, null);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(p => p.Code).Should().Contain(new[] { "PRJ-01", "PRJ-02" });
    }

    [Fact]
    public async Task GetProjectByIdAsync_WhenProjectExists_ShouldReturnProjectDto()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var mockProject = new Project { Id = projectId, Code = "PRJ-01", Name = "Alpha", Status = "Active" };

        _projectRepoMock
            .Setup(r => r.GetByIdAsync(projectId))
            .ReturnsAsync(mockProject);

        // Act
        var result = await _projectService.GetProjectByIdAsync(projectId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(projectId);
        result.Code.Should().Be("PRJ-01");
    }

    [Fact]
    public async Task GetProjectByIdAsync_WhenProjectDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepoMock
            .Setup(r => r.GetByIdAsync(projectId))
            .ReturnsAsync((Project?)null);

        // Act
        var result = await _projectService.GetProjectByIdAsync(projectId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateProjectAsync_WhenValidInput_ShouldCreateAndReturnProject()
    {
        // Arrange
        var createDto = new CreateProjectDto
        {
            Code = "NEW-01",
            Name = "New Project",
            Status = "Planning"
        };

        _projectRepoMock
            .Setup(r => r.ExistsCodeAsync(createDto.Code, null))
            .ReturnsAsync(false);

        _projectRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Project>()))
            .ReturnsAsync((Project p) => p);

        _projectRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => new Project { Id = id, Code = createDto.Code, Name = createDto.Name, Status = createDto.Status });

        // Act
        var result = await _projectService.CreateProjectAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("NEW-01");
        result.Name.Should().Be("New Project");
        _projectRepoMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateProjectAsync_WhenInvalidDates_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateProjectDto
        {
            Code = "PRJ-DATE",
            Name = "Date Validation Project",
            Status = "Planning",
            StartDate = new DateOnly(2026, 12, 31),
            EndDate = new DateOnly(2026, 1, 1) // Invalid: EndDate < StartDate
        };

        // Act
        Func<Task> act = async () => await _projectService.CreateProjectAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*End date must be greater than or equal to start date*");
    }

    [Fact]
    public async Task CreateProjectAsync_WhenDuplicateCode_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var createDto = new CreateProjectDto
        {
            Code = "EXISTING-CODE",
            Name = "Duplicate Code Project",
            Status = "Planning"
        };

        _projectRepoMock
            .Setup(r => r.ExistsCodeAsync(createDto.Code, null))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _projectService.CreateProjectAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
        _projectRepoMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenProjectExists_ShouldUpdateFields()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var existingProject = new Project { Id = projectId, Code = "OLD-CODE", Name = "Old Name", Status = "Planning" };

        _projectRepoMock.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync(existingProject);
        _projectRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);

        var updateDto = new UpdateProjectDto { Name = "Updated Name", Status = "Active" };

        // Act
        var result = await _projectService.UpdateProjectAsync(projectId, updateDto);

        // Assert
        _projectRepoMock.Verify(r => r.UpdateAsync(It.Is<Project>(p => p.Name == "Updated Name" && p.Status == "Active")), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectAsync_WhenProjectExists_ShouldReturnTrue()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var existingProject = new Project { Id = projectId, Code = "DEL-01", Name = "Delete Me" };

        _projectRepoMock.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync(existingProject);
        _projectRepoMock.Setup(r => r.DeleteAsync(existingProject)).Returns(Task.CompletedTask);

        // Act
        var result = await _projectService.DeleteProjectAsync(projectId);

        // Assert
        result.Should().BeTrue();
        _projectRepoMock.Verify(r => r.DeleteAsync(existingProject), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
