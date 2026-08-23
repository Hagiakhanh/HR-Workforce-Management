using BBV.HR.Application.Common.Constants;
using BBV.HR.Application.Entities;
using BBV.HR.Application.Interfaces.Repositories;
using BBV.HR.Infrastructure.Auth;
using FluentAssertions;
using Moq;

namespace BBV.HR.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _authService = new AuthService(_employeeRepositoryMock.Object);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_WhenValidCredentials_ShouldReturnEmployee()
    {
        // Arrange
        var password = "CorrectPassword123!";
        var passwordHash = _authService.HashPassword(password);
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Email = "test@bbv.vn",
            PasswordHash = passwordHash,
            Role = Roles.Admin,
            FirstName = "Test",
            LastName = "User"
        };

        _employeeRepositoryMock.Setup(r => r.GetByEmailAsync("test@bbv.vn"))
            .ReturnsAsync(employee);

        // Act
        var result = await _authService.ValidateCredentialsAsync("test@bbv.vn", password);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test@bbv.vn");
        result.Role.Should().Be(Roles.Admin);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_WhenWrongPassword_ShouldReturnNull()
    {
        // Arrange
        var passwordHash = _authService.HashPassword("CorrectPassword123!");
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Email = "test@bbv.vn",
            PasswordHash = passwordHash,
            Role = Roles.Employee
        };

        _employeeRepositoryMock.Setup(r => r.GetByEmailAsync("test@bbv.vn"))
            .ReturnsAsync(employee);

        // Act
        var result = await _authService.ValidateCredentialsAsync("test@bbv.vn", "WrongPassword");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ValidateCredentialsAsync_WhenEmailNotFound_ShouldReturnNull()
    {
        // Arrange
        _employeeRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee?)null);

        // Act
        var result = await _authService.ValidateCredentialsAsync("notfound@bbv.vn", "Password123!");

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("", "Password123!")]
    [InlineData("test@bbv.vn", "")]
    [InlineData("   ", "   ")]
    public async Task ValidateCredentialsAsync_WhenEmptyInputs_ShouldReturnNull(string email, string password)
    {
        // Act
        var result = await _authService.ValidateCredentialsAsync(email, password);

        // Assert
        result.Should().BeNull();
        _employeeRepositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void HashPassword_And_VerifyPassword_ShouldWorkProperly()
    {
        // Arrange
        var password = "MySecurePassword!";

        // Act
        var hash = _authService.HashPassword(password);
        var isValid = _authService.VerifyPassword(password, hash);
        var isInvalid = _authService.VerifyPassword("WrongPassword", hash);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        isValid.Should().BeTrue();
        isInvalid.Should().BeFalse();
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ShouldDelegateToRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var employee = new Employee { Id = id, Email = "emp@bbv.vn" };
        _employeeRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(employee);

        // Act
        var result = await _authService.GetEmployeeByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        _employeeRepositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
