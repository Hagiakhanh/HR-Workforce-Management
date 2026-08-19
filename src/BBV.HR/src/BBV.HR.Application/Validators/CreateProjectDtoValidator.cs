using BBV.HR.Application.DTOs.Projects;
using FluentValidation;

namespace BBV.HR.Application.Validators;

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Project code is required.")
            .MaximumLength(50).WithMessage("Project code must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Project code can only contain letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name must not exceed 200 characters.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Project status is required.");

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

        RuleFor(x => x.LaborBudget)
            .LessThanOrEqualTo(x => x.TotalBudget!.Value)
            .When(x => x.TotalBudget.HasValue && x.LaborBudget.HasValue)
            .WithMessage("Labor budget cannot exceed total budget.");

        RuleFor(x => x.BudgetWarningThreshold)
            .InclusiveBetween(1, 100)
            .When(x => x.BudgetWarningThreshold.HasValue)
            .WithMessage("Budget warning threshold must be between 1% and 100%.");

        RuleFor(x => x.ExpectedTeamSizeFte)
            .GreaterThan(0)
            .When(x => x.ExpectedTeamSizeFte.HasValue)
            .WithMessage("Expected team size must be greater than 0.");

        RuleFor(x => x.DefaultWeeklyCapacityH)
            .InclusiveBetween(1, 168)
            .When(x => x.DefaultWeeklyCapacityH.HasValue)
            .WithMessage("Default weekly capacity must be between 1 and 168 hours.");
    }
}
