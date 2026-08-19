using BBV.HR.Application.DTOs.Capability;
using FluentValidation;

namespace BBV.HR.Application.Validators.Capabilities;

public class UpdateCapabilityDtoValidator : AbstractValidator<UpdateCapabilityDto>
{
    public UpdateCapabilityDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("Category must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Category));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
