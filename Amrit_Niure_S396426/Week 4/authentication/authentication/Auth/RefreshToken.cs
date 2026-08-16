using authentication.Data;
using Microsoft.AspNetCore.Identity;

namespace authentication.Auth
{
    public static class RefreshTokenEndpoint
    {
        public record Request(string RefreshToken);

        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/refresh", async (Request request, RefreshTokenService refreshTokenService, UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext dbContext) =>
            {
                var rotation = await refreshTokenService.ValidateAndRotateAsync(request.RefreshToken);
                if (!rotation.Success || rotation.UserId is null || rotation.NewRawToken is null)
                {
                    return Results.Unauthorized();
                }

                var user = await userManager.FindByIdAsync(rotation.UserId);
                if (user is null)
                {
                    return Results.Unauthorized();
                }

                var roles = await userManager.GetRolesAsync(user);
                var permissions = await PermissionResolver.GetPermissionsForRolesAsync(dbContext, roles);
                var accessToken = JwtTokenFactory.CreateAccessToken(user, roles, permissions, configuration);

                return Results.Ok(new { AccessToken = accessToken, RefreshToken = rotation.NewRawToken });
            });
        }
    }
}
