using Microsoft.AspNetCore.Authorization;

namespace authentication.Authorization;

public enum PermissionMatchMode
{
    Any,
    All
}

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public IReadOnlyCollection<string> Permissions { get; }
    public PermissionMatchMode MatchMode { get; }

    public PermissionRequirement(PermissionMatchMode matchMode, params string[] permissions)
    {
        if (permissions is null || permissions.Length == 0)
        {
            throw new ArgumentException("At least one permission must be specified.", nameof(permissions));
        }

        MatchMode = matchMode;
        Permissions = permissions;
    }
}
