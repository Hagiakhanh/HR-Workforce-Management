using BBV.HR.Application.Entities;
using BBV.HR.Application.Mappings;
using FluentAssertions;

namespace BBV.HR.Tests.Mappings;

public class MappingExtensionsTests
{
    [Fact]
    public void CapabilityMapping_ToDto_ShouldMapFieldsCorrectly()
    {
        // Arrange
        var capability = new Capability
        {
            Id = Guid.NewGuid(),
            Name = "Python",
            Category = "Data Science",
            Description = "Python programming language",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = capability.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(capability.Id);
        dto.Name.Should().Be("Python");
        dto.Category.Should().Be("Data Science");
        dto.Description.Should().Be("Python programming language");
    }

    [Fact]
    public void ProjectMapping_ToDto_ShouldMapFieldsAndNavigationProperties()
    {
        // Arrange
        var manager = new Employee { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith" };
        var creator = new Employee { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Jones" };
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Code = "PRJ-99",
            Name = "Titan",
            Status = "Active",
            ManagerId = manager.Id,
            Manager = manager,
            CreatedBy = creator.Id,
            Creator = creator
        };

        // Act
        var dto = project.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Code.Should().Be("PRJ-99");
        dto.Name.Should().Be("Titan");
        dto.ManagerName.Should().Be("Alice Smith");
        dto.CreatorName.Should().Be("Bob Jones");
    }

    [Fact]
    public void ProjectMemberMapping_ToDto_ShouldMapEmployeeDetails()
    {
        // Arrange
        var employee = new Employee { Id = Guid.NewGuid(), EmployeeCode = "EMP001", FirstName = "Charlie", LastName = "Brown" };
        var member = new ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            EmployeeId = employee.Id,
            Employee = employee,
            ProjectRole = "Frontend Architect",
            AllocationPct = 80
        };

        // Act
        var dto = member.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.EmployeeCode.Should().Be("EMP001");
        dto.EmployeeName.Should().Be("Charlie Brown");
        dto.ProjectRole.Should().Be("Frontend Architect");
        dto.AllocationPct.Should().Be(80);
    }

    [Fact]
    public void ProjectEffortMapping_CalculateHours_WhenStartAndEndProvided_ShouldReturnDiffHours()
    {
        // Arrange
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(17, 30); // 8.5 hours

        // Act
        var hours = ProjectEffortMappingExtensions.CalculateHours(start, end);

        // Assert
        hours.Should().Be(8.5);
    }

    [Fact]
    public void ProjectEffortMapping_CalculateHours_WhenStartOrEndNull_ShouldReturnDefault8Hours()
    {
        // Act & Assert
        ProjectEffortMappingExtensions.CalculateHours(null, new TimeOnly(17, 0)).Should().Be(8.0);
        ProjectEffortMappingExtensions.CalculateHours(new TimeOnly(9, 0), null).Should().Be(8.0);
    }
}
