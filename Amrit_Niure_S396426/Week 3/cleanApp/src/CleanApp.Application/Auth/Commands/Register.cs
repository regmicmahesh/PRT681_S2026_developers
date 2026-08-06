using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.Auth.Commands;

public sealed record RegisterCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
    }
}

internal sealed class RegisterCommandHandler(IIdentityService identityService, IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.RegisterAsync(request.Email, request.Password, cancellationToken);
        if (result.IsFailure)
            return Result.Failure<AuthResponse>(result.Error);

        var (token, expiresAtUtc) = tokenGenerator.GenerateToken(result.Value.UserId, request.Email, result.Value.Roles);

        return Result.Success(new AuthResponse(result.Value.UserId.Value, request.Email, token, expiresAtUtc));
    }
}
