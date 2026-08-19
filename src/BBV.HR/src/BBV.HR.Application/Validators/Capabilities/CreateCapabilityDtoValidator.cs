using BBV.HR.Application.DTOs.Capability;
using FluentValidation;

namespace BBV.HR.Application.Validators.Capabilities;

public class CreateCapabilityDtoValidator : AbstractValidator<CreateCapabilityDto>
{
    public CreateCapabilityDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Category)
            .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
    }
}