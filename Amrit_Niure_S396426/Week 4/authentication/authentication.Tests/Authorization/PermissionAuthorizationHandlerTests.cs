using System.Security.Claims;
using authentication.Authorization;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace authentication.Tests.Authorization;

public class PermissionAuthorizationHandlerTests
{
    private static ClaimsPrincipal PrincipalWithPermissions(params string[] permissions)
    {
        var claims = permissions.Select(p => new Claim(CustomClaimTypes.Permission, p));
        var identity = new ClaimsIdentity(claims, authenticationType: "Test");
        return new ClaimsPrincipal(identity);
    }

    private static async Task<AuthorizationHandlerContext> EvaluateAsync(
        PermissionRequirement requirement,
        ClaimsPrincipal principal)
    {
        var context = new AuthorizationHandlerContext([requirement], principal, resource: null);
        var handler = new PermissionAuthorizationHandler();
        await handler.HandleAsync(context);
        return context;
    }

    [Fact]
    public async Task Any_Succeeds_When_One_Of_Several_Permissions_Present()
    {
        var requirement = new PermissionRequirement(PermissionMatchMode.Any, "user:read", "user:delete");
        var principal = PrincipalWithPermissions("user:read");

        var context = await EvaluateAsync(requirement, principal);

        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task Any_Fails_When_No_Permissions_Present()
    {
        var requirement = new PermissionRequirement(PermissionMatchMode.Any, "user:read", "user:delete");
        var principal = PrincipalWithPermissions("user:update");

        var context = await EvaluateAsync(requirement, principal);

        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task All_Succeeds_Only_When_Every_Permission_Present()
    {
        var requirement = new PermissionRequirement(PermissionMatchMode.All, "user:read", "user:delete");
        var principal = PrincipalWithPermissions("user:read", "user:delete", "user:update");

        var context = await EvaluateAsync(requirement, principal);

        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task All_Fails_When_One_Permission_Missing()
    {
        var requirement = new PermissionRequirement(PermissionMatchMode.All, "user:read", "user:delete");
        var principal = PrincipalWithPermissions("user:read");

        var context = await EvaluateAsync(requirement, principal);

        Assert.False(context.HasSucceeded);
    }
}
