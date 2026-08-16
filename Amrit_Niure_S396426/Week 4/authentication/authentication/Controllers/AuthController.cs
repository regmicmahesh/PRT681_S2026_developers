using authentication.Auth;
using authentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace authentication.Controllers;

[ApiController]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ApplicationDbContext dbContext,
    RefreshTokenService refreshTokenService) : ControllerBase
{
    public record RegisterRequest(string Email, string Initials, string Password, bool EnableNotifications = false);
    public record LoginRequest(string Email, string Password);
    public record RefreshRequest(string RefreshToken);
    public record LogoutRequest(string RefreshToken);

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync();
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            Initials = request.Initials,
            EnableNotifications = request.EnableNotifications
        };

        var identityResult = await userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            return BadRequest(identityResult.Errors);
        }

        var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Member);
        if (!addToRoleResult.Succeeded)
        {
            return BadRequest(addToRoleResult.Errors);
        }

        await transaction.CommitAsync();
        return Ok(UserResponse.FromUser(user));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);
        var permissions = await PermissionResolver.GetPermissionsForRolesAsync(dbContext, roles);

        var accessToken = JwtTokenFactory.CreateAccessToken(user, roles, permissions, configuration);
        var (_, refreshToken) = await refreshTokenService.IssueAsync(user.Id);

        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken, User = UserResponse.FromUser(user) });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var validation = await refreshTokenService.ValidateAsync(request.RefreshToken);
        if (!validation.Success || validation.UserId is null)
        {
            return Unauthorized();
        }

        var user = await userManager.FindByIdAsync(validation.UserId);
        if (user is null)
        {
            return Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);
        var permissions = await PermissionResolver.GetPermissionsForRolesAsync(dbContext, roles);
        var accessToken = JwtTokenFactory.CreateAccessToken(user, roles, permissions, configuration);

        return Ok(new { AccessToken = accessToken });
    }

    // No [Authorize] here: the access token TTL is intentionally short (minutes), so by the time a
    // client logs out its access token may already be expired. Possession of the (single-use,
    // server-validated, hashed) refresh token is itself the credential for this endpoint - the same
    // trust model an OAuth revocation endpoint uses.
    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(LogoutRequest request)
    {
        await refreshTokenService.RevokeAsync(request.RefreshToken);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue(JwtRegisteredClaimNames.Email);
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Roles = roles,
        });
    }
}
