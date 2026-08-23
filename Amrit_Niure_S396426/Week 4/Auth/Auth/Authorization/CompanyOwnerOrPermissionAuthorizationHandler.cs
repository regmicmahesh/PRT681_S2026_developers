using Auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Authorization;

// Succeeds if the caller created the target company (resource = CompanyId), or if they hold the
// requirement's override permission (job:manage-any - an Admin acting across companies). Without
// this, job:create/job:update/job:delete alone would let any Employer or Recruiter act on every
// other company's jobs, since those permissions are granted broadly to the whole role - see
// Authorization/README.md.
public sealed class CompanyOwnerOrPermissionAuthorizationHandler
    : AuthorizationHandler<CompanyOwnerOrPermissionRequirement, Guid>
{
    private readonly ApplicationDbContext _dbContext;

    public CompanyOwnerOrPermissionAuthorizationHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CompanyOwnerOrPermissionRequirement requirement,
        Guid companyId)
    {
        if (context.User.HasClaim(CustomClaimTypes.Permission, requirement.OverridePermission))
        {
            context.Succeed(requirement);
            return;
        }

        var callerId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                       ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (callerId is null)
        {
            return;
        }

        var ownerId = await _dbContext.Companies
            .Where(c => c.Id == companyId)
            .Select(c => c.OwnerId)
            .FirstOrDefaultAsync();

        if (ownerId is not null && string.Equals(ownerId, callerId, StringComparison.Ordinal))
        {
            context.Succeed(requirement);
        }
    }
}
