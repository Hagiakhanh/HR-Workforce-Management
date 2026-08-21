using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.EmployeeCapability;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Application.Services;
using BBV.HR.Application.Validators.EmployeeCapabilities;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BBV.HR.Tests.Services;

public class EmployeeCapabilityServiceTests
{
    private readonly Mock<IEmployeeCapabilityRepository> _employeeCapabilityRepoMock;
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly Mock<ICapabilityRepository> _capabilityRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly EmployeeCapabilityService _service;

    public EmployeeCapabilityServiceTests()
    {
        _employeeCapabilityRepoMock = new Mock<IEmployeeCapabilityRepository>();
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _capabilityRepoMock = new Mock<ICapabilityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _service = new EmployeeCapabilityService(
            _employeeCapabilityRepoMock.Object,
            _employeeRepoMock.Object,
            _capabilityRepoMock.Object,
            _unitOfWorkMock.Object,
            new AddEmployeeCapabilityDtoValidator(),
            new UpdateEmployeeCapabilityDtoValidator()
        );
    }

    [Fact]
    public async Task GetEmployeeCapabilitiesAsync_WhenEmployeeExists_ShouldReturnDtos()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capability = new Capability { Id = Guid.NewGuid(), Name = "C#", Category = "Backend" };
        var employeeCapabilities = new List<EmployeeCapability>
        {
            new EmployeeCapability
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                CapabilityId = capability.Id,
                Capability = capability,
                ProficiencyLevel = 4,
                YearsExperience = 3.5m
            }
        };

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _employeeCapabilityRepoMock.Setup(r => r.GetByEmployeeIdAsync(employeeId)).ReturnsAsync(employeeCapabilities);

        // Act
        var result = await _service.GetEmployeeCapabilitiesAsync(employeeId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var dto = result.First();
        dto.CapabilityName.Should().Be("C#");
        dto.ProficiencyLevel.Should().Be(4);
    }

    [Fact]
    public async Task GetEmployeeCapabilitiesAsync_WhenEmployeeNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _service.GetEmployeeCapabilitiesAsync(employeeId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddEmployeeCapabilityAsync_WhenValid_ShouldAddAndReturnDto()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capabilityId = Guid.NewGuid();
        var dto = new AddEmployeeCapabilityDto
        {
            CapabilityId = capabilityId,
            ProficiencyLevel = 5,
            YearsExperience = 4.0m
        };

        var capability = new Capability { Id = capabilityId, Name = ".NET", Category = "Backend" };

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync(capability);
        _employeeCapabilityRepoMock.Setup(r => r.ExistsAsync(employeeId, capabilityId)).ReturnsAsync(false);

        _employeeCapabilityRepoMock
            .Setup(r => r.AddAsync(It.IsAny<EmployeeCapability>()))
            .ReturnsAsync((EmployeeCapability ec) => ec);

        _employeeCapabilityRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => new EmployeeCapability
            {
                Id = id,
                EmployeeId = employeeId,
                CapabilityId = capabilityId,
                Capability = capability,
                ProficiencyLevel = 5,
                YearsExperience = 4.0m
            });

        // Act
        var result = await _service.AddEmployeeCapabilityAsync(employeeId, dto);

        // Assert
        result.Should().NotBeNull();
        result.CapabilityName.Should().Be(".NET");
        result.ProficiencyLevel.Should().Be(5);
        _employeeCapabilityRepoMock.Verify(r => r.AddAsync(It.IsAny<EmployeeCapability>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddEmployeeCapabilityAsync_WhenCapabilityNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capabilityId = Guid.NewGuid();
        var dto = new AddEmployeeCapabilityDto { CapabilityId = capabilityId };

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync((Capability?)null);

        // Act
        Func<Task> act = async () => await _service.AddEmployeeCapabilityAsync(employeeId, dto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddEmployeeCapabilityAsync_WhenDuplicate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capabilityId = Guid.NewGuid();
        var dto = new AddEmployeeCapabilityDto { CapabilityId = capabilityId };
        var capability = new Capability { Id = capabilityId, Name = "SQL" };

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _capabilityRepoMock.Setup(r => r.GetByIdAsync(capabilityId)).ReturnsAsync(capability);
        _employeeCapabilityRepoMock.Setup(r => r.ExistsAsync(employeeId, capabilityId)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _service.AddEmployeeCapabilityAsync(employeeId, dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already has this capability*");
    }

    [Fact]
    public async Task UpdateEmployeeCapabilityAsync_WhenFound_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capabilityId = Guid.NewGuid();
        var updateDto = new UpdateEmployeeCapabilityDto { ProficiencyLevel = 3, YearsExperience = 2.0m };
        var capability = new Capability { Id = capabilityId, Name = "Docker" };

        var existingEntity = new EmployeeCapability
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            CapabilityId = capabilityId,
            Capability = capability,
            ProficiencyLevel = 1,
            YearsExperience = 0.5m
        };

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _employeeCapabilityRepoMock.Setup(r => r.GetByEmployeeAndCapabilityIdAsync(employeeId, capabilityId))
            .ReturnsAsync(existingEntity);
        _employeeCapabilityRepoMock.Setup(r => r.GetByIdAsync(existingEntity.Id))
            .ReturnsAsync(existingEntity);

        // Act
        var result = await _service.UpdateEmployeeCapabilityAsync(employeeId, capabilityId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.ProficiencyLevel.Should().Be(3);
        result.YearsExperience.Should().Be(2.0m);
        _employeeCapabilityRepoMock.Verify(r => r.UpdateAsync(existingEntity), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeCapabilityAsync_WhenNotFound_ShouldReturnNull()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capabilityId = Guid.NewGuid();
        var updateDto = new UpdateEmployeeCapabilityDto { ProficiencyLevel = 3 };

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _employeeCapabilityRepoMock.Setup(r => r.GetByEmployeeAndCapabilityIdAsync(employeeId, capabilityId))
            .ReturnsAsync((EmployeeCapability?)null);

        // Act
        var result = await _service.UpdateEmployeeCapabilityAsync(employeeId, capabilityId, updateDto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveEmployeeCapabilityAsync_WhenFound_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capabilityId = Guid.NewGuid();
        var existingEntity = new EmployeeCapability
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            CapabilityId = capabilityId
        };

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _employeeCapabilityRepoMock.Setup(r => r.GetByEmployeeAndCapabilityIdAsync(employeeId, capabilityId))
            .ReturnsAsync(existingEntity);

        // Act
        var result = await _service.RemoveEmployeeCapabilityAsync(employeeId, capabilityId);

        // Assert
        result.Should().BeTrue();
        _employeeCapabilityRepoMock.Verify(r => r.DeleteAsync(existingEntity), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveEmployeeCapabilityAsync_WhenNotFound_ShouldReturnFalse()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var capabilityId = Guid.NewGuid();

        _employeeRepoMock.Setup(r => r.ExistsAsync(employeeId)).ReturnsAsync(true);
        _employeeCapabilityRepoMock.Setup(r => r.GetByEmployeeAndCapabilityIdAsync(employeeId, capabilityId))
            .ReturnsAsync((EmployeeCapability?)null);

        // Act
        var result = await _service.RemoveEmployeeCapabilityAsync(employeeId, capabilityId);

        // Assert
        result.Should().BeFalse();
    }
}
