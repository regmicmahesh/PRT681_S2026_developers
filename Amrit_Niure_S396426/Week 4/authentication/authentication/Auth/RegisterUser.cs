using authentication.Data;
using Microsoft.AspNetCore.Identity;

namespace authentication.Auth
{
    public static class RegisterUser
    {
        public record Request(string Email, string Initials, string Password, bool EnableNotifications = false);

        public static void MapEndPoint(IEndpointRouteBuilder app)
        {
            app.MapPost("register", async (Request request, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext) =>
            {
                using var transaction = await dbContext.Database.BeginTransactionAsync();
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    Initials = request.Initials,
                    EnableNotifications = request.EnableNotifications
                };
                IdentityResult identityResult = await userManager.CreateAsync(user, request.Password);
                if (!identityResult.Succeeded)
                {
                    return Results.BadRequest(identityResult.Errors);
                }
                IdentityResult addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Member);
                if (!addToRoleResult.Succeeded)
                {
                    return Results.BadRequest(addToRoleResult.Errors);
                }
                await transaction.CommitAsync();
                return Results.Ok(user);
            });
        }
    }
}
