using CleanApp.Domain.Common;

namespace CleanApp.Application.Auth;

public static class AuthErrors
{
    public static readonly Error EmailAlreadyExists =
        Error.Conflict("Auth.EmailAlreadyExists", "An account with this email already exists.");

    public static readonly Error InvalidCredentials =
        Error.Unauthorized("Auth.InvalidCredentials", "Email or password is incorrect.");

    public static readonly Error RegistrationFailed =
        Error.Failure("Auth.RegistrationFailed", "Could not create the account.");
}
