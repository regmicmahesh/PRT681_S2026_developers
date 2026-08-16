using authentication.Authorization;
using authentication.Data;
using Microsoft.EntityFrameworkCore;

namespace authentication.Auth;

public static class PermissionResolver
{
    public static Task<string[]> GetPermissionsForRolesAsync(ApplicationDbContext dbContext, IEnumerable<string> roles)
    {
        return (from role in dbContext.Roles
                join claim in dbContext.RoleClaims
                on role.Id equals claim.RoleId
                where roles.Contains(role.Name!)
                && claim.ClaimType == CustomClaimTypes.Permission
                select claim.ClaimValue)
                .Distinct()
                .ToArrayAsync();
    }
}
