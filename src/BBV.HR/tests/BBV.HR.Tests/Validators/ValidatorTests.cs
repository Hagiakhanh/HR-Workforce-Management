using BBV.HR.Application.DTOs.Capability;
using BBV.HR.Application.DTOs.ProjectMembers;
using BBV.HR.Application.DTOs.Projects;
using BBV.HR.Application.Validators;
using BBV.HR.Application.Validators.Capabilities;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BBV.HR.Tests.Validators;

public class ValidatorTests
{
    private readonly CreateCapabilityDtoValidator _createCapabilityValidator = new();
    private readonly UpdateCapabilityDtoValidator _updateCapabilityValidator = new();
    private readonly CreateProjectDtoValidator _createProjectValidator = new();
    private readonly UpdateProjectDtoValidator _updateProjectValidator = new();
    private readonly AddProjectMemberDtoValidator _addMemberValidator = new();
    private readonly UpdateMemberAllocationDtoValidator _updateAllocationValidator = new();

    #region Capability Validators

    [Fact]
    public void CreateCapabilityDtoValidator_WhenNameIsEmpty_ShouldHaveValidationError()
    {
        var dto = new CreateCapabilityDto { Name = "" };
        var result = _createCapabilityValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateCapabilityDtoValidator_WhenNameExceeds100Chars_ShouldHaveValidationError()
    {
        var dto = new CreateCapabilityDto { Name = new string('A', 101) };
        var result = _createCapabilityValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateCapabilityDtoValidator_WhenValid_ShouldNotHaveValidationError()
    {
        var dto = new CreateCapabilityDto { Name = "C#", Category = "Backend", Description = "Programming language" };
        var result = _createCapabilityValidator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdateCapabilityDtoValidator_WhenNameExceeds100Chars_ShouldHaveValidationError()
    {
        var dto = new UpdateCapabilityDto { Name = new string('B', 101) };
        var result = _updateCapabilityValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Project Validators

    [Fact]
    public void CreateProjectDtoValidator_WhenCodeAndNameEmpty_ShouldHaveValidationErrors()
    {
        var dto = new CreateProjectDto { Code = "", Name = "", Status = "" };
        var result = _createProjectValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Code);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public void CreateProjectDtoValidator_WhenEndDateLessThanStartDate_ShouldHaveValidationError()
    {
        var dto = new CreateProjectDto
        {
            Code = "PRJ-01",
            Name = "Project",
            Status = "Active",
            StartDate = new DateOnly(2026, 6, 1),
            EndDate = new DateOnly(2026, 5, 1)
        };
        var result = _createProjectValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void CreateProjectDtoValidator_WhenLaborBudgetExceedsTotalBudget_ShouldHaveValidationError()
    {
        var dto = new CreateProjectDto
        {
            Code = "PRJ-01",
            Name = "Project",
            Status = "Active",
            TotalBudget = 1000,
            LaborBudget = 1500
        };
        var result = _createProjectValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LaborBudget);
    }

    [Fact]
    public void UpdateProjectDtoValidator_WhenCodeHasInvalidCharacters_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto { Code = "PRJ@#$" };
        var result = _updateProjectValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    #endregion

    #region Project Member Validators

    [Fact]
    public void AddProjectMemberDtoValidator_WhenEmployeeIdIsEmpty_ShouldHaveValidationError()
    {
        var dto = new AddProjectMemberDto { EmployeeId = Guid.Empty, ProjectRole = "Dev", AllocationPct = 100 };
        var result = _addMemberValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.EmployeeId);
    }

    [Fact]
    public void AddProjectMemberDtoValidator_WhenAllocationPctOutOfRange_ShouldHaveValidationError()
    {
        var dto = new AddProjectMemberDto { EmployeeId = Guid.NewGuid(), ProjectRole = "Dev", AllocationPct = 150 };
        var result = _addMemberValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.AllocationPct);
    }

    [Fact]
    public void UpdateMemberAllocationDtoValidator_WhenAllocationPctIsZero_ShouldHaveValidationError()
    {
        var dto = new UpdateMemberAllocationDto { AllocationPct = 0 };
        var result = _updateAllocationValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.AllocationPct);
    }

    [Fact]
    public void UpdateMemberAllocationDtoValidator_WhenAllocationPctIsValid_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMemberAllocationDto { AllocationPct = 50 };
        var result = _updateAllocationValidator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion
}
