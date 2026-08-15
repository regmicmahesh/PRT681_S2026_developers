using authentication.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace authentication.Authorization;

public static class Extensions
{
    public static async Task SeedRolesAndPermissions(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
        if (adminRole is null)
        {
            await roleManager.CreateAsync(adminRole = new IdentityRole(Roles.Admin));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersDelete));
        }
        var memberRole = await roleManager.FindByNameAsync(Roles.Member);
        if (memberRole is null)
        {
            await roleManager.CreateAsync(memberRole = new IdentityRole(Roles.Admin));
            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));
            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));
        }
    }
}
