using CleanApp.Application.Auth;
using CleanApp.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CleanApp.Infrastructure.Identity;

/// <summary>
/// Ensures the "Admin"/"User" roles and a seeded admin account exist. Called once on
/// startup. In a real deployment this - like the automatic migration it usually runs
/// alongside - belongs in an explicit release/init step rather than every app start.
/// </summary>
public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminSettings = serviceProvider.GetRequiredService<IOptions<AdminUserSettings>>().Value;

        foreach (var role in new[] { Roles.Admin, Roles.User })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        if (string.IsNullOrWhiteSpace(adminSettings.Email) || string.IsNullOrWhiteSpace(adminSettings.Password))
            return;

        if (await userManager.FindByEmailAsync(adminSettings.Email) is not null)
            return;

        var admin = new ApplicationUser
        {
            UserName = adminSettings.Email,
            Email = adminSettings.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminSettings.Password);
        if (result.Succeeded)
            await userManager.AddToRolesAsync(admin, [Roles.Admin, Roles.User]);
    }
}
