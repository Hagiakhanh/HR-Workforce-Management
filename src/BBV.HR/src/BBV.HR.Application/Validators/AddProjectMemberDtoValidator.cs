using BBV.HR.Application.DTOs.ProjectMembers;
using FluentValidation;

namespace BBV.HR.Application.Validators;

public class AddProjectMemberDtoValidator : AbstractValidator<AddProjectMemberDto>
{
    public AddProjectMemberDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Employee ID must be a valid GUID.");

        RuleFor(x => x.ProjectRole)
            .NotEmpty().WithMessage("Project role is required.")
            .MaximumLength(100).WithMessage("Project role must not exceed 100 characters.");

        RuleFor(x => x.AllocationPct)
            .InclusiveBetween(1, 100).WithMessage("Allocation percentage must be between 1% and 100%.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date.");
    }
}
