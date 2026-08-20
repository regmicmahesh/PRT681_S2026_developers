using Microsoft.AspNetCore.Authorization;

namespace authentication.Authorization;

public sealed class SameUserOrPermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public SameUserOrPermissionRequirement(string permission)
    {
        Permission = permission;
    }
}
