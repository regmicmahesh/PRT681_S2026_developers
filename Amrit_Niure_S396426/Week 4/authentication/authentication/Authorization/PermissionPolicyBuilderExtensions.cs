using Microsoft.AspNetCore.Authorization;

namespace authentication.Authorization;

public static class PermissionPolicyBuilderExtensions
{
    public static void RequireAnyPermission(
        this AuthorizationPolicyBuilder builder,
        params string[] permissions)
    {
        builder.AddRequirements(new PermissionRequirement(PermissionMatchMode.Any, permissions));
    }

    public static void RequireAllPermissions(
        this AuthorizationPolicyBuilder builder,
        params string[] permissions)
    {
        builder.AddRequirements(new PermissionRequirement(PermissionMatchMode.All, permissions));
    }

    public static void RequirePermission(
        this AuthorizationPolicyBuilder builder,
        params string[] permissions)
    {
        builder.RequireAnyPermission(permissions);
    }
}
