using authentication.Authorization;
using authentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace authentication.Auth
{
    public static class UsersApi
    {
        public record UserResponse(string Id, string? Email, string Initials, bool EnableNotifications);
        public record UpdateRequest(string? Initials, bool? EnableNotifications);

        private static UserResponse ToResponse(ApplicationUser user) =>
            new(user.Id, user.Email, user.Initials, user.EnableNotifications);

        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            // List is not a single owned resource, so it's a plain permission check - Admin only
            // (Members hold no user:* permission; their own record is reachable via GET /users/{id}).
            app.MapGet("/users", async (UserManager<ApplicationUser> userManager) =>
            {
                var users = await userManager.Users.ToListAsync();
                return Results.Ok(users.Select(ToResponse));
            }).RequireAuthorization(policy => policy.RequireAnyPermission(Permissions.UsersRead));

            app.MapGet("/users/{id}", async (string id, ClaimsPrincipal principal,
                UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService) =>
            {
                var authResult = await authorizationService.AuthorizeAsync(
                    principal, id, new SameUserOrPermissionRequirement(Permissions.UsersRead));
                if (!authResult.Succeeded)
                {
                    return Results.Forbid();
                }

                var user = await userManager.FindByIdAsync(id);
                return user is null ? Results.NotFound() : Results.Ok(ToResponse(user));
            }).RequireAuthorization();

            app.MapPut("/users/{id}", async (string id, UpdateRequest request, ClaimsPrincipal principal,
                UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService) =>
            {
                // RequireAuthorization() below only enforces authentication at the routing layer -
                // minimal API route metadata has no access to the "{id}" route value as a resource.
                // The actual per-resource ownership decision happens here, via the explicit
                // AuthorizeAsync(principal, resource, requirement) call.
                var authResult = await authorizationService.AuthorizeAsync(
                    principal, id, new SameUserOrPermissionRequirement(Permissions.UsersUpdate));
                if (!authResult.Succeeded)
                {
                    return Results.Forbid();
                }

                var user = await userManager.FindByIdAsync(id);
                if (user is null)
                {
                    return Results.NotFound();
                }

                if (request.Initials is not null)
                {
                    user.Initials = request.Initials;
                }
                if (request.EnableNotifications is not null)
                {
                    user.EnableNotifications = request.EnableNotifications.Value;
                }

                var result = await userManager.UpdateAsync(user);
                return result.Succeeded ? Results.Ok(ToResponse(user)) : Results.BadRequest(result.Errors);
            }).RequireAuthorization();

            // Deleting another account is sensitive enough that it stays Admin-only (permission-gated),
            // not self-serviceable via ownership - unlike read/update, there's no "delete yourself" path here.
            app.MapDelete("/users/{id}", async (string id, UserManager<ApplicationUser> userManager) =>
            {
                var user = await userManager.FindByIdAsync(id);
                if (user is null)
                {
                    return Results.NotFound();
                }

                var result = await userManager.DeleteAsync(user);
                return result.Succeeded ? Results.NoContent() : Results.BadRequest(result.Errors);
            }).RequireAuthorization(policy => policy.RequireAnyPermission(Permissions.UsersDelete));
        }
    }
}
