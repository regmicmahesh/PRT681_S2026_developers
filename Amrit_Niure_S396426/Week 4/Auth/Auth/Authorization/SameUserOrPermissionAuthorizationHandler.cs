using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Authorization;

// Resource-based (ABAC) check: succeeds if the caller IS the target user (resource = target user id),
// or if they hold the given permission (e.g. an Admin acting on someone else's record).
public sealed class SameUserOrPermissionAuthorizationHandler
    : AuthorizationHandler<SameUserOrPermissionRequirement, string>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SameUserOrPermissionRequirement requirement,
        string resource)
    {
        var callerId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                       ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (callerId is not null && string.Equals(callerId, resource, StringComparison.Ordinal))
        {
            context.Succeed(requirement);
        }
        else if (context.User.HasClaim(CustomClaimTypes.Permission, requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
