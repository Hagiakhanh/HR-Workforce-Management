using BBV.HR.Application.DTOs.EmployeeCapability;
using FluentValidation;

namespace BBV.HR.Application.Validators.EmployeeCapabilities;

public class AddEmployeeCapabilityDtoValidator : AbstractValidator<AddEmployeeCapabilityDto>
{
    public AddEmployeeCapabilityDtoValidator()
    {
        RuleFor(x => x.CapabilityId)
            .NotEmpty().WithMessage("CapabilityId is required.");

        RuleFor(x => x.ProficiencyLevel)
            .InclusiveBetween(1, 5)
            .When(x => x.ProficiencyLevel.HasValue)
            .WithMessage("ProficiencyLevel must be between 1 and 5.");

        RuleFor(x => x.YearsExperience)
            .InclusiveBetween(0m, 100m)
            .When(x => x.YearsExperience.HasValue)
            .WithMessage("YearsExperience must be between 0 and 100.");
    }
}

public class UpdateEmployeeCapabilityDtoValidator : AbstractValidator<UpdateEmployeeCapabilityDto>
{
    public UpdateEmployeeCapabilityDtoValidator()
    {
        RuleFor(x => x.ProficiencyLevel)
            .InclusiveBetween(1, 5)
            .When(x => x.ProficiencyLevel.HasValue)
            .WithMessage("ProficiencyLevel must be between 1 and 5.");

        RuleFor(x => x.YearsExperience)
            .InclusiveBetween(0m, 100m)
            .When(x => x.YearsExperience.HasValue)
            .WithMessage("YearsExperience must be between 0 and 100.");
    }
}
