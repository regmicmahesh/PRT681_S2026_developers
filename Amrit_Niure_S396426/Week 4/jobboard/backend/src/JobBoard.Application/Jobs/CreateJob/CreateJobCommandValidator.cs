using FluentValidation;

namespace JobBoard.Application.Jobs.CreateJob;

public sealed class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.CompanyId).NotEmpty();
        RuleFor(c => c.SalaryMin).GreaterThanOrEqualTo(0);
        RuleFor(c => c.SalaryMax).GreaterThanOrEqualTo(c => c.SalaryMin);
        RuleFor(c => c.SalaryCurrency).NotEmpty();
    }
}
