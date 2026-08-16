using authentication.Auth;
using authentication.Authorization;
using authentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authentication.Controllers;

[ApiController]
[Route("users")]
public class UsersController(
    UserManager<ApplicationUser> userManager,
    IAuthorizationService authorizationService) : ControllerBase
{
    public record UpdateRequest(string? Initials, bool? EnableNotifications);

    // List is not a single owned resource, so it's a plain permission check - Admin only
    // (Members hold no user:* permission; their own record is reachable via GET /users/{id}).
    [HttpGet]
    [Authorize(Policy = "RequireUsersRead")]
    public async Task<IActionResult> GetAll()
    {
        var users = await userManager.Users.ToListAsync();
        return Ok(users.Select(UserResponse.FromUser));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(string id)
    {
        var authResult = await authorizationService.AuthorizeAsync(
            User, id, new SameUserOrPermissionRequirement(Permissions.UsersRead));
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        var user = await userManager.FindByIdAsync(id);
        return user is null ? NotFound() : Ok(UserResponse.FromUser(user));
    }

    // [Authorize] here only enforces authentication at the routing layer - attribute routing has no
    // access to the "{id}" route value as a resource. The actual per-resource ownership decision
    // happens below, via the explicit AuthorizeAsync(User, resource, requirement) call.
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, UpdateRequest request)
    {
        var authResult = await authorizationService.AuthorizeAsync(
            User, id, new SameUserOrPermissionRequirement(Permissions.UsersUpdate));
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
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
        return result.Succeeded ? Ok(UserResponse.FromUser(user)) : BadRequest(result.Errors);
    }

    // Deleting another account is sensitive enough that it stays Admin-only (permission-gated),
    // not self-serviceable via ownership - unlike read/update, there's no "delete yourself" path here.
    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireUsersDelete")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded ? NoContent() : BadRequest(result.Errors);
    }
}
