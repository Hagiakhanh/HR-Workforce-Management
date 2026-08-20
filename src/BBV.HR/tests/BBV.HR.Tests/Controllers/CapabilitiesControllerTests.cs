using BBV.HR.Api.Controllers;
using BBV.HR.Application.Common.Exceptions;
using BBV.HR.Application.DTOs.Capability;
using BBV.HR.Application.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BBV.HR.Tests.Controllers;

public class CapabilitiesControllerTests
{
    private readonly Mock<ICapabilityService> _serviceMock;
    private readonly CapabilitiesController _controller;

    public CapabilitiesControllerTests()
    {
        _serviceMock = new Mock<ICapabilityService>();
        _controller = new CapabilitiesController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithCapabilities()
    {
        // Arrange
        var capabilities = new List<CapabilityDto> { new() { Id = Guid.NewGuid(), Name = "C#" } };
        _serviceMock.Setup(s => s.GetAllCapabilitiesAsync(null, null)).ReturnsAsync(capabilities);

        // Act
        var actionResult = await _controller.GetAll(null, null);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(capabilities);
    }

    [Fact]
    public async Task GetById_WhenFound_ShouldReturnOkWithCapability()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new CapabilityDto { Id = id, Name = "Docker" };
        _serviceMock.Setup(s => s.GetCapabilityByIdAsync(id)).ReturnsAsync(dto);

        // Act
        var actionResult = await _controller.GetById(id);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetCapabilityByIdAsync(id)).ReturnsAsync((CapabilityDto?)null);

        // Act
        Func<Task> act = async () => await _controller.GetById(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateCapabilityDto { Name = "Kubernetes" };
        var createdDto = new CapabilityDto { Id = Guid.NewGuid(), Name = "Kubernetes" };
        _serviceMock.Setup(s => s.CreateCapabilityAsync(createDto)).ReturnsAsync(createdDto);

        // Act
        var actionResult = await _controller.Create(createDto);

        // Assert
        var createdResult = actionResult.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(createdDto);
    }

    [Fact]
    public async Task Update_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateCapabilityDto { Name = "Updated" };
        _serviceMock.Setup(s => s.UpdateCapabilityAsync(id, dto)).ReturnsAsync((CapabilityDto?)null);

        // Act
        Func<Task> act = async () => await _controller.Update(id, dto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Delete_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteCapabilityAsync(id)).ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _controller.Delete(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Delete_WhenSuccessful_ShouldReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteCapabilityAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204); // 204
    }
}
