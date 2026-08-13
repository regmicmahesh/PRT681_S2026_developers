using FluentValidation;

namespace JobBoard.Application.Jobs.ApplyToJob;

public sealed class ApplyToJobCommandValidator : AbstractValidator<ApplyToJobCommand>
{
    public ApplyToJobCommandValidator()
    {
        RuleFor(c => c.JobId).NotEmpty();
        RuleFor(c => c.CandidateName).NotEmpty();
        RuleFor(c => c.CandidateEmail).NotEmpty().EmailAddress();
        RuleFor(c => c.ResumeUrl).NotEmpty();
    }
}
