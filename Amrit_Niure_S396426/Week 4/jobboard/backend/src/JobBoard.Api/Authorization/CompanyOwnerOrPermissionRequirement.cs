using Microsoft.AspNetCore.Authorization;

namespace JobBoard.Api.Authorization;

// Resource-based (ABAC) check paired with an AuthorizeAsync(User, companyId, ...) call in a
// controller - a plain [Authorize(Policy = "...")] can't express "must own this specific company"
// because attribute routing has no access to a route/body value as a resource.
public sealed class CompanyOwnerOrPermissionRequirement : IAuthorizationRequirement
{
    public string OverridePermission { get; }

    public CompanyOwnerOrPermissionRequirement(string overridePermission)
    {
        OverridePermission = overridePermission;
    }
}
