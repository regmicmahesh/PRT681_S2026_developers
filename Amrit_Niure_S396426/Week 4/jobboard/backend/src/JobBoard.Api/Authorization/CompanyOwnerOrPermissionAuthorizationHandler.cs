using JobBoard.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JobBoard.Api.Authorization;

// Succeeds if the caller created the target company (resource = CompanyId), or if they hold the
// requirement's override permission (job:manage-any - an Admin acting across companies). Without
// this, job:create/job:update/job:delete alone would let any Employer or Recruiter act on every
// other company's jobs, since those permissions are granted broadly to the whole role.
public sealed class CompanyOwnerOrPermissionAuthorizationHandler
    : AuthorizationHandler<CompanyOwnerOrPermissionRequirement, Guid>
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyOwnerOrPermissionAuthorizationHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
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

        var company = await _companyRepository.GetByIdAsync(companyId);
        if (company is not null && string.Equals(company.OwnerId, callerId, StringComparison.Ordinal))
        {
            context.Succeed(requirement);
        }
    }
}
