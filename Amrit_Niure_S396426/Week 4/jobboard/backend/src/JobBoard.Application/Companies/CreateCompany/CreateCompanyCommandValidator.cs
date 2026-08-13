using FluentValidation;

namespace JobBoard.Application.Companies.CreateCompany;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.ContactEmail).NotEmpty().EmailAddress();
    }
}
