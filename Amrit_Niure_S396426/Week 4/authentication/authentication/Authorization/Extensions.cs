using authentication.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace authentication.Authorization;

// See Authorization/README.md for the full permission-based authorization convention this seeds into.
public static class Extensions
{
    public static async Task SeedRolesAndPermissions(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var (adminRole, adminCreated) = await EnsureRoleAsync(roleManager, Roles.Admin);
        if (adminCreated)
        {
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersDelete));
        }

        var (memberRole, memberCreated) = await EnsureRoleAsync(roleManager, Roles.Member);
        if (memberCreated)
        {
            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));
            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));
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
