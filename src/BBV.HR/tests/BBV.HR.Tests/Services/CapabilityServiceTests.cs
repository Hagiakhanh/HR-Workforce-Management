using BBV.HR.Application.DTOs.Capability;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Services;
using BBV.HR.Application.Validators.Capabilities;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BBV.HR.Tests.Services;

public class CapabilityServiceTests
{
    private readonly Mock<ICapabilityRepository> _capabilityRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CapabilityService _capabilityService;

    public CapabilityServiceTests()
    {
        _capabilityRepoMock = new Mock<ICapabilityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _capabilityService = new CapabilityService(
            _capabilityRepoMock.Object,
            _unitOfWorkMock.Object,
            new CreateCapabilityDtoValidator(),
            new UpdateCapabilityDtoValidator()
        );
    }

    [Fact]
    public async Task GetAllCapabilitiesAsync_ShouldReturnCapabilityDtos()
    {
        // Arrange
        var capabilities = new List<Capability>
        {
            new Capability { Id = Guid.NewGuid(), Name = "C#", Category = "Backend", Description = "C# Programming" },
            new Capability { Id = Guid.NewGuid(), Name = "React", Category = "Frontend", Description = "React UI" }
        };

        _capabilityRepoMock
            .Setup(r => r.GetAllAsync(It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(capabilities);

        // Act
        var result = await _capabilityService.GetAllCapabilitiesAsync(null, null);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(c => c.Name).Should().Contain(new[] { "C#", "React" });
    }

    [Fact]
    public async Task GetCapabilityByIdAsync_WhenFound_ShouldReturnCapabilityDto()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        var capability = new Capability { Id = capabilityId, Name = "SQL", Category = "Database" };

        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync(capability);

        // Act
        var result = await _capabilityService.GetCapabilityByIdAsync(capabilityId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(capabilityId);
        result.Name.Should().Be("SQL");
    }

    [Fact]
    public async Task GetCapabilityByIdAsync_WhenNotFound_ShouldReturnNull()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync((Capability?)null);

        // Act
        var result = await _capabilityService.GetCapabilityByIdAsync(capabilityId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCapabilityAsync_WhenValidInput_ShouldCreateAndReturnDto()
    {
        // Arrange
        var createDto = new CreateCapabilityDto
        {
            Name = "Docker",
            Category = "DevOps",
            Description = "Containerization"
        };

        _capabilityRepoMock.Setup(r => r.ExistsNameAsync(createDto.Name, null)).ReturnsAsync(false);
        _capabilityRepoMock.Setup(r => r.AddAsync(It.IsAny<Capability>())).ReturnsAsync((Capability c) => c);

        // Act
        var result = await _capabilityService.CreateCapabilityAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Docker");
        result.Category.Should().Be("DevOps");
        _capabilityRepoMock.Verify(r => r.AddAsync(It.IsAny<Capability>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateCapabilityAsync_WhenNameAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var createDto = new CreateCapabilityDto { Name = "ExistingCapability" };
        _capabilityRepoMock.Setup(r => r.ExistsNameAsync(createDto.Name, null)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _capabilityService.CreateCapabilityAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
        _capabilityRepoMock.Verify(r => r.AddAsync(It.IsAny<Capability>()), Times.Never);
    }

    [Fact]
    public async Task CreateCapabilityAsync_WhenInvalidInput_ShouldThrowValidationException()
    {
        // Arrange: Name is empty
        var createDto = new CreateCapabilityDto { Name = "" };

        // Act
        Func<Task> act = async () => await _capabilityService.CreateCapabilityAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Name is required*");
    }

    [Fact]
    public async Task UpdateCapabilityAsync_WhenCapabilityNotFound_ShouldReturnNull()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        var updateDto = new UpdateCapabilityDto { Name = "Updated Name" };
        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync((Capability?)null);

        // Act
        var result = await _capabilityService.UpdateCapabilityAsync(capabilityId, updateDto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCapabilityAsync_WhenValidInput_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        var existing = new Capability { Id = capabilityId, Name = "OldName", Category = "OldCat" };
        var updateDto = new UpdateCapabilityDto { Name = "NewName", Category = "NewCat" };

        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync(existing);
        _capabilityRepoMock.Setup(r => r.ExistsNameAsync("NewName", capabilityId)).ReturnsAsync(false);
        _capabilityRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Capability>())).Returns(Task.CompletedTask);

        // Act
        var result = await _capabilityService.UpdateCapabilityAsync(capabilityId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("NewName");
        result.Category.Should().Be("NewCat");
        _capabilityRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Capability>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCapabilityAsync_WhenNameConflict_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        var existing = new Capability { Id = capabilityId, Name = "OldName" };
        var updateDto = new UpdateCapabilityDto { Name = "ConflictingName" };

        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync(existing);
        _capabilityRepoMock.Setup(r => r.ExistsNameAsync("ConflictingName", capabilityId)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _capabilityService.UpdateCapabilityAsync(capabilityId, updateDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }

    [Fact]
    public async Task DeleteCapabilityAsync_WhenCapabilityNotFound_ShouldReturnFalse()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync((Capability?)null);

        // Act
        var result = await _capabilityService.DeleteCapabilityAsync(capabilityId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCapabilityAsync_WhenCapabilityInUse_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        var existing = new Capability { Id = capabilityId, Name = "InUse" };

        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync(existing);
        _capabilityRepoMock.Setup(r => r.IsInUseAsync(capabilityId)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _capabilityService.DeleteCapabilityAsync(capabilityId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*currently in use*");
        _capabilityRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Capability>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCapabilityAsync_WhenValidAndNotInUse_ShouldReturnTrue()
    {
        // Arrange
        var capabilityId = Guid.NewGuid();
        var existing = new Capability { Id = capabilityId, Name = "NotInUse" };

        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync(existing);
        _capabilityRepoMock.Setup(r => r.IsInUseAsync(capabilityId)).ReturnsAsync(false);
        _capabilityRepoMock.Setup(r => r.DeleteAsync(existing)).Returns(Task.CompletedTask);

        // Act
        var result = await _capabilityService.DeleteCapabilityAsync(capabilityId);

        // Assert
        result.Should().BeTrue();
        _capabilityRepoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
