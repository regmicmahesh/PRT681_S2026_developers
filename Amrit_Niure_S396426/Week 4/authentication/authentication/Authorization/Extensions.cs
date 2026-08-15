using authentication.Data;
using Microsoft.AspNetCore.Identity;

namespace authentication.Authorization;

public static class Extensions
{
    public static async Task SeedRolesAndPermissions(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }
        if (!await roleManager.RoleExistsAsync(Roles.Member))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Member));
        }
    }
}
