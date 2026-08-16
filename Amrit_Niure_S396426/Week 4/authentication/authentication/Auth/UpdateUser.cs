using authentication.Authorization;
using authentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace authentication.Auth
{
    public static class UpdateUser
    {
        public record Request(string? Initials, bool? EnableNotifications);

        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/users/{id}", async (string id, Request request, ClaimsPrincipal principal,
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
                return result.Succeeded
                    ? Results.Ok(new { user.Id, user.Email, user.Initials, user.EnableNotifications })
                    : Results.BadRequest(result.Errors);
            }).RequireAuthorization();
        }
    }
}
