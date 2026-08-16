using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using authentication.Authorization;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace authentication.Tests.Authorization;

public class SameUserOrPermissionAuthorizationHandlerTests
{
    private const string Permission = "user:update";

    private static ClaimsPrincipal PrincipalFor(string userId, params string[] permissions)
    {
        List<Claim> claims = [
            new(JwtRegisteredClaimNames.Sub, userId),
            ..permissions.Select(p => new Claim(CustomClaimTypes.Permission, p))
        ];
        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Test"));
    }

    private static async Task<AuthorizationHandlerContext> EvaluateAsync(ClaimsPrincipal principal, string resourceUserId)
    {
        var requirement = new SameUserOrPermissionRequirement(Permission);
        var context = new AuthorizationHandlerContext([requirement], principal, resourceUserId);
        var handler = new SameUserOrPermissionAuthorizationHandler();
        await handler.HandleAsync(context);
        return context;
    }

    [Fact]
    public async Task Succeeds_When_Caller_Is_The_Target_User_Without_The_Permission()
    {
        var principal = PrincipalFor("user-1");

        var context = await EvaluateAsync(principal, "user-1");

        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task Fails_When_Different_User_Without_The_Permission()
    {
        var principal = PrincipalFor("user-1");

        var context = await EvaluateAsync(principal, "user-2");

        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task Succeeds_When_Different_User_But_Has_The_Permission()
    {
        var principal = PrincipalFor("user-1", Permission);

        var context = await EvaluateAsync(principal, "user-2");

        Assert.True(context.HasSucceeded);
    }
}
