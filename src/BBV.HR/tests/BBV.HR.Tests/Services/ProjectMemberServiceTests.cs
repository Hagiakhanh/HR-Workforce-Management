using BBV.HR.Application.DTOs.ProjectMembers;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Services;
using BBV.HR.Application.Validators;
using FluentAssertions;
using Moq;

namespace BBV.HR.Tests.Services;

public class ProjectMemberServiceTests
{
    private readonly Mock<IProjectMemberRepository> _memberRepoMock;
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProjectMemberService _memberService;

    public ProjectMemberServiceTests()
    {
        _memberRepoMock = new Mock<IProjectMemberRepository>();
        _projectRepoMock = new Mock<IProjectRepository>();
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _memberService = new ProjectMemberService(
            _memberRepoMock.Object,
            _projectRepoMock.Object,
            _employeeRepoMock.Object,
            _unitOfWorkMock.Object,
            new AddProjectMemberDtoValidator(),
            new UpdateMemberAllocationDtoValidator()
        );
    }

    [Fact]
    public async Task GetProjectMembersAsync_ShouldReturnMembersList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var mockMembers = new List<ProjectMember>
        {
            new ProjectMember { Id = Guid.NewGuid(), ProjectId = projectId, EmployeeId = Guid.NewGuid(), ProjectRole = "Backend Lead" },
            new ProjectMember { Id = Guid.NewGuid(), ProjectId = projectId, EmployeeId = Guid.NewGuid(), ProjectRole = "Frontend Dev" }
        };

        _memberRepoMock.Setup(r => r.GetByProjectIdAsync(projectId)).ReturnsAsync(mockMembers);

        // Act
        var result = await _memberService.GetProjectMembersAsync(projectId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(m => m.ProjectRole).Should().Contain(new[] { "Backend Lead", "Frontend Dev" });
    }

    [Fact]
    public async Task AddProjectMemberAsync_WhenValidInput_ShouldAddMember()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var addDto = new AddProjectMemberDto
        {
            EmployeeId = employeeId,
            ProjectRole = "Fullstack Dev",
            AllocationPct = 100
        };

        _projectRepoMock.Setup(r => r.ExistsAsync(projectId)).ReturnsAsync(true);
        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _memberRepoMock.Setup(r => r.IsMemberAsync(projectId, employeeId)).ReturnsAsync(false);
        _memberRepoMock.Setup(r => r.AddAsync(It.IsAny<ProjectMember>())).ReturnsAsync((ProjectMember pm) => pm);
        _memberRepoMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<Guid>()))
            .ReturnsAsync((Guid pId, Guid mId) => new ProjectMember
            {
                Id = mId,
                ProjectId = pId,
                EmployeeId = employeeId,
                ProjectRole = addDto.ProjectRole,
                AllocationPct = addDto.AllocationPct
            });

        // Act
        var result = await _memberService.AddProjectMemberAsync(projectId, addDto);

        // Assert
        result.Should().NotBeNull();
        result.ProjectId.Should().Be(projectId);
        result.EmployeeId.Should().Be(employeeId);
        result.ProjectRole.Should().Be("Fullstack Dev");
        _memberRepoMock.Verify(r => r.AddAsync(It.IsAny<ProjectMember>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddProjectMemberAsync_WhenProjectNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var addDto = new AddProjectMemberDto { EmployeeId = Guid.NewGuid(), ProjectRole = "Developer", AllocationPct = 100 };

        _projectRepoMock.Setup(r => r.ExistsAsync(projectId)).ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _memberService.AddProjectMemberAsync(projectId, addDto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*Project with ID*not found*");
    }

    [Fact]
    public async Task AddProjectMemberAsync_WhenAlreadyMember_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var addDto = new AddProjectMemberDto { EmployeeId = employeeId, ProjectRole = "Developer", AllocationPct = 100 };

        _projectRepoMock.Setup(r => r.ExistsAsync(projectId)).ReturnsAsync(true);
        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _memberRepoMock.Setup(r => r.IsMemberAsync(projectId, employeeId)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _memberService.AddProjectMemberAsync(projectId, addDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already a member*");
    }

    [Fact]
    public async Task UpdateMemberAllocationAsync_WhenMemberExists_ShouldUpdateAllocation()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var existingMember = new ProjectMember { Id = memberId, ProjectId = projectId, AllocationPct = 50 };

        _memberRepoMock.Setup(r => r.GetByIdAsync(projectId, memberId)).ReturnsAsync(existingMember);
        _memberRepoMock.Setup(r => r.UpdateAsync(It.IsAny<ProjectMember>())).Returns(Task.CompletedTask);

        var allocationDto = new UpdateMemberAllocationDto { AllocationPct = 80 };

        // Act
        var result = await _memberService.UpdateMemberAllocationAsync(projectId, memberId, allocationDto);

        // Assert
        _memberRepoMock.Verify(r => r.UpdateAsync(It.Is<ProjectMember>(m => m.AllocationPct == 80)), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
