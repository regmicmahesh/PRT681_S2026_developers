using Microsoft.AspNetCore.Authorization;

namespace JobBoard.Api.Authorization;

public static class PermissionPolicyBuilderExtensions
{
    public static void RequireAnyPermission(
        this AuthorizationPolicyBuilder builder,
        params string[] permissions)
    {
        builder.AddRequirements(new PermissionRequirement(permissions));
    }
}
