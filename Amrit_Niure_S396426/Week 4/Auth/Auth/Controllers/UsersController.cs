using Auth.Auth;
using Auth.Authorization;
using Auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers;

[ApiController]
[Route("users")]
public class UsersController(
    UserManager<ApplicationUser> userManager,
    IAuthorizationService authorizationService) : ControllerBase
{
    public record UpdateRequest(string? Initials, bool? EnableNotifications);
    public record AssignRoleRequest(string Role);

    // List is not a single owned resource, so it's a plain permission check - Admin only
    // (JobSeeker/Employer/Recruiter hold no user:* permission; their own record is reachable via
    // GET /users/{id}).
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

    // Role reassignment (including granting Admin) is deliberately its own Admin-only, permission-
    // gated endpoint rather than something exposed via Update - it changes what a user is *allowed
    // to do*, not their profile data, so it needs a stricter, explicitly audited boundary.
    [HttpPut("{id}/role")]
    [Authorize(Policy = "RequireUsersManageRoles")]
    public async Task<IActionResult> AssignRole(string id, AssignRoleRequest request)
    {
        if (!Roles.All.Contains(request.Role))
        {
            return BadRequest(new { Error = $"Unknown role '{request.Role}'." });
        }

        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            return BadRequest(removeResult.Errors);
        }

        var addResult = await userManager.AddToRoleAsync(user, request.Role);
        return addResult.Succeeded ? NoContent() : BadRequest(addResult.Errors);
    }
}
