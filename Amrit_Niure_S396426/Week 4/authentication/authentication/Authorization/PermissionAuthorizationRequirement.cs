using Microsoft.AspNetCore.Authorization;

namespace authentication.Authorization;

public class PermissionAuthorizationRequirement(params string[] allowedPermissions)
    : AuthorizationHandler<PermissionAuthorizationRequirement>, IAuthorizationRequirement
{
    public string[] AllowedPermissions { get; } = allowedPermissions;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        foreach (var permission in requirement.AllowedPermissions)
        {
            bool found = context.User.FindFirst(c =>
                c.Type == CustomClaimTypes.Permission &&
                c.Value == permission) is not null;

            if (found)
            {
                context.Succeed(requirement);
                break;
            }
        }
        return Task.CompletedTask;
    }
}


public static class PermissionExtensions
{
    public static void RequirePermission(
        this AuthorizationPolicyBuilder builder,
        params string[] allowedPermissions)
    {
        builder.AddRequirements(new PermissionAuthorizationRequirement(allowedPermissions));
    }
}
