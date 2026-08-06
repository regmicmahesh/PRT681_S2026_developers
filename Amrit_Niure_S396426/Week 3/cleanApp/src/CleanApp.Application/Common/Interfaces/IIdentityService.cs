using CleanApp.Application.Auth;
using CleanApp.Domain.Common;

namespace CleanApp.Application.Common.Interfaces;

/// <summary>
/// Application-facing abstraction over the user store (ASP.NET Core Identity in
/// Infrastructure). Keeps Identity's concrete types out of the Application layer, matching
/// how IEmailSender/IBackgroundJobService keep other external concerns out.
/// </summary>
public interface IIdentityService
{
    Task<Result<AuthenticatedUser>> RegisterAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<Result<AuthenticatedUser>> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
}
