using Auth.Auth;
using Auth.Authorization;
using Auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.Controllers;

[ApiController]
[Route("companies")]
public class CompaniesController(
    ApplicationDbContext dbContext,
    IAuthorizationService authorizationService) : ControllerBase
{
    public record CreateCompanyRequest(string Name, string ContactEmail);
    public record UpdateCompanyRequest(string Name, string ContactEmail);

    // Public listing - a job board's companies are meant to be browsable without an account.
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var companies = await dbContext.Companies.AsNoTracking().ToListAsync();
        return Ok(companies.Select(CompanyResponse.FromCompany));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var company = await dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return company is null ? NotFound() : Ok(CompanyResponse.FromCompany(company));
    }

    // Any authenticated Employer/Recruiter/Admin (job:create) can register a company - they become
    // its owner, which is the boundary CompanyOwnerOrPermissionRequirement scopes job management to.
    [HttpPost]
    [Authorize(Policy = "RequireJobCreate")]
    public async Task<IActionResult> Create(CreateCompanyRequest request)
    {
        var ownerId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                      ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ContactEmail = request.ContactEmail,
            OwnerId = ownerId,
            CreatedAtUtc = DateTime.UtcNow,
        };

        dbContext.Companies.Add(company);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = company.Id }, CompanyResponse.FromCompany(company));
    }

    // [Authorize] here only enforces authentication at the routing layer - the real ownership
    // decision happens below, via the explicit AuthorizeAsync(User, resource, requirement) call,
    // since attribute policies can't see the "{id}" route value as a resource.
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, UpdateCompanyRequest request)
    {
        var authResult = await authorizationService.AuthorizeAsync(
            User, id, new CompanyOwnerOrPermissionRequirement(Permissions.JobManageAny));
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        var company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
        {
            return NotFound();
        }

        company.Name = request.Name;
        company.ContactEmail = request.ContactEmail;
        await dbContext.SaveChangesAsync();

        return Ok(CompanyResponse.FromCompany(company));
    }

    // The owner's dashboard view - every status, unlike GET /jobs (public listing), which only
    // shows Published jobs. See JobsController.GetAll.
    [HttpGet("{id:guid}/jobs")]
    [Authorize]
    public async Task<IActionResult> GetJobs(Guid id)
    {
        var authResult = await authorizationService.AuthorizeAsync(
            User, id, new CompanyOwnerOrPermissionRequirement(Permissions.JobManageAny));
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        var jobs = await dbContext.Jobs
            .Include(j => j.Applications)
            .AsNoTracking()
            .Where(j => j.CompanyId == id)
            .ToListAsync();

        return Ok(jobs.Select(JobResponse.FromJob));
    }
}
