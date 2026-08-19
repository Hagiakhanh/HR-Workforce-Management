using BBV.HR.Application.DTOs.Projects;
using FluentValidation;

namespace BBV.HR.Application.Validators;

public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectDtoValidator()
    {
        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Project code must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9_-]+$").When(x => !string.IsNullOrEmpty(x.Code))
            .WithMessage("Project code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Name)
            .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage("Project name must not exceed 200 characters.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date.");

        RuleFor(x => x.TotalBudget)
            .GreaterThanOrEqualTo(0)
            .When(x => x.TotalBudget.HasValue)
            .WithMessage("Total budget must be greater than or equal to 0.");

        RuleFor(x => x.LaborBudget)
            .GreaterThanOrEqualTo(0)
            .When(x => x.LaborBudget.HasValue)
            .WithMessage("Labor budget must be greater than or equal to 0.");

        RuleFor(x => x.BudgetWarningThreshold)
            .InclusiveBetween(1, 100)
            .When(x => x.BudgetWarningThreshold.HasValue)
            .WithMessage("Budget warning threshold must be between 1% and 100%.");
    }
}
