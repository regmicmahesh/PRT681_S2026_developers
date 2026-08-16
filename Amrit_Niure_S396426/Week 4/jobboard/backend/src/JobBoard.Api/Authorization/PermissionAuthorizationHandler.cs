using Microsoft.AspNetCore.Authorization;

namespace JobBoard.Api.Authorization;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var granted = context.User
            .FindAll(CustomClaimTypes.Permission)
            .Select(c => c.Value)
            .ToHashSet();

        if (requirement.Permissions.Any(granted.Contains))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
