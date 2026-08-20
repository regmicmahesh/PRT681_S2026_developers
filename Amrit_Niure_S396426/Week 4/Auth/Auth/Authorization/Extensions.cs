using Auth.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Auth.Authorization;

// See Authorization/README.md for the full permission-based authorization convention this seeds into.
public static class Extensions
{
    // Role -> permission matrix. Deliberately excludes user:read/user:update for every non-Admin
    // role - a caller's access to their own user record comes from SameUserOrPermissionRequirement
    // (ownership), not a permission grant. See Authorization/README.md.
    private static readonly IReadOnlyDictionary<string, string[]> RolePermissions = new Dictionary<string, string[]>
    {
        // Full control: user administration plus every job-board capability.
        [Roles.Admin] =
        [
            Permissions.UsersRead,
            Permissions.UsersUpdate,
            Permissions.UsersDelete,
            Permissions.UsersManageRoles,
            Permissions.JobCreate,
            Permissions.JobRead,
            Permissions.JobUpdate,
            Permissions.JobDelete,
            Permissions.JobManageAny,
            Permissions.ApplicationReadAny,
            Permissions.ApplicationManage,
            Permissions.CandidateSearch,
        ],

        // Browses jobs, applies, and reviews their own application history.
        [Roles.JobSeeker] =
        [
            Permissions.JobRead,
            Permissions.JobApply,
            Permissions.ApplicationReadOwn,
        ],

        // Owns job postings end to end and manages applicants against them.
        [Roles.Employer] =
        [
            Permissions.JobCreate,
            Permissions.JobRead,
            Permissions.JobUpdate,
            Permissions.JobDelete,
            Permissions.ApplicationReadAny,
            Permissions.ApplicationManage,
        ],

        // Posts/manages jobs on behalf of clients and additionally searches the candidate pool -
        // the capability that distinguishes it from Employer. No job:delete: a recruiter manages
        // postings but doesn't own them the way the posting Employer does.
        [Roles.Recruiter] =
        [
            Permissions.JobCreate,
            Permissions.JobRead,
            Permissions.JobUpdate,
            Permissions.ApplicationReadAny,
            Permissions.ApplicationManage,
            Permissions.CandidateSearch,
        ],
    };

    public static async Task SeedRolesAndPermissions(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var (roleName, permissions) in RolePermissions)
        {
            var (role, created) = await EnsureRoleAsync(roleManager, roleName);
            if (!created)
            {
                continue;
            }

            foreach (var permission in permissions)
            {
                await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }
        }
    }

    // Race-safe: if another app instance creates the role concurrently, CreateAsync fails cleanly
    // on Identity's unique role-name index and we re-fetch the winner's row instead of throwing or
    // double-adding claims. Claims are only added by the caller when `created` is true.
    private static async Task<(IdentityRole Role, bool Created)> EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is not null)
        {
            return (role, false);
        }

        var candidate = new IdentityRole(roleName);
        var createResult = await roleManager.CreateAsync(candidate);
        if (createResult.Succeeded)
        {
            return (candidate, true);
        }

        role = await roleManager.FindByNameAsync(roleName);
        if (role is null)
        {
            throw new InvalidOperationException(
                $"Failed to create or find role '{roleName}': {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }

        return (role, false);
    }
}
