using FluentValidation;
using WetSeasonBackend.Api.Dtos;

namespace WetSeasonBackend.Api.Validators;

public class CreateIncidentRequestValidator : AbstractValidator<CreateIncidentRequestDto>
{
    public CreateIncidentRequestValidator()
    {
        RuleFor(x => x.CommunityId)
            .GreaterThan(0).WithMessage("A community must be selected.");

        RuleFor(x => x.Severity)
            .InclusiveBetween(1, 4).WithMessage("Severity must be between 1 and 4.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000);

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Unknown incident type.");
        
    }   
}