using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.Auth.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty();
    }
}

internal sealed class LoginCommandHandler(IIdentityService identityService, IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.ValidateCredentialsAsync(request.Email, request.Password, cancellationToken);
        if (result.IsFailure)
            return Result.Failure<AuthResponse>(result.Error);

        var (token, expiresAtUtc) = tokenGenerator.GenerateToken(result.Value.UserId, request.Email, result.Value.Roles);

        return Result.Success(new AuthResponse(result.Value.UserId.Value, request.Email, token, expiresAtUtc));
    }
}
