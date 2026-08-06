using CleanApp.Application.Auth;
using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Persistence.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanApp.Infrastructure.Identity;

internal sealed class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<Result<AuthenticatedUser>> RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var existing = await userManager.FindByEmailAsync(email);
        if (existing is not null)
            return Result.Failure<AuthenticatedUser>(AuthErrors.EmailAlreadyExists);

        var user = new ApplicationUser { UserName = email, Email = email };
        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            var message = string.Join(" ", createResult.Errors.Select(e => e.Description));
            return Result.Failure<AuthenticatedUser>(Error.Validation("Auth.RegistrationFailed", message));
        }

        await userManager.AddToRoleAsync(user, Roles.User);

        return Result.Success(new AuthenticatedUser(new UserId(user.Id), [Roles.User]));
    }

    public async Task<Result<AuthenticatedUser>> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<AuthenticatedUser>(AuthErrors.InvalidCredentials);

        var passwordValid = await userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            return Result.Failure<AuthenticatedUser>(AuthErrors.InvalidCredentials);

        var roles = await userManager.GetRolesAsync(user);

        return Result.Success(new AuthenticatedUser(new UserId(user.Id), roles.ToArray()));
    }
}
