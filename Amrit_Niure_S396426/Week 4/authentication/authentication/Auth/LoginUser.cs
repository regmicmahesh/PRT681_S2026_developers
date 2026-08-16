using authentication.Data;
using Microsoft.AspNetCore.Identity;

namespace authentication.Auth
{
    public static class LoginUser
    {
        public record Request(string Email, string Password);
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (Request request, UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext dbContext, RefreshTokenService refreshTokenService) =>
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Results.Unauthorized();
                }
                var roles = await userManager.GetRolesAsync(user);
                var permissions = await PermissionResolver.GetPermissionsForRolesAsync(dbContext, roles);

                var accessToken = JwtTokenFactory.CreateAccessToken(user, roles, permissions, configuration);
                var (_, refreshToken) = await refreshTokenService.IssueAsync(user.Id);

                return Results.Ok(new { AccessToken = accessToken, RefreshToken = refreshToken, User = user });
            });
        }
    }
}
