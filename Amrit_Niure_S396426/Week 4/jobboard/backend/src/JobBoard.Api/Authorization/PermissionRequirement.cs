using Microsoft.AspNetCore.Authorization;

namespace JobBoard.Api.Authorization;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public IReadOnlyCollection<string> Permissions { get; }

    public PermissionRequirement(params string[] permissions)
    {
        if (permissions is null || permissions.Length == 0)
        {
            throw new ArgumentException("At least one permission must be specified.", nameof(permissions));
        }

        Permissions = permissions;
    }
}
