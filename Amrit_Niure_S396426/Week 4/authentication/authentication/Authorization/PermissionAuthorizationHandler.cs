using Microsoft.AspNetCore.Authorization;

namespace authentication.Authorization;

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

        var satisfied = requirement.MatchMode == PermissionMatchMode.Any
            ? requirement.Permissions.Any(granted.Contains)
            : requirement.Permissions.All(granted.Contains);

        if (satisfied)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
