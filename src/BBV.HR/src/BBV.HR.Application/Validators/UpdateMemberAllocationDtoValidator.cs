using BBV.HR.Application.DTOs.ProjectMembers;
using FluentValidation;

namespace BBV.HR.Application.Validators;

public class UpdateMemberAllocationDtoValidator : AbstractValidator<UpdateMemberAllocationDto>
{
    public UpdateMemberAllocationDtoValidator()
    {
        RuleFor(x => x.AllocationPct)
            .InclusiveBetween(1, 100).WithMessage("Allocation percentage must be between 1% and 100%.");
    }
}
