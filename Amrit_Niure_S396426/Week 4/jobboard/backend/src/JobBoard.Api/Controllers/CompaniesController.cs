using JobBoard.Application.Companies.CreateCompany;
using JobBoard.Application.Companies.GetCompanies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JobBoard.Api.Controllers;

public sealed class CompaniesController : ApiControllerBase
{
    private readonly ISender _sender;

    public CompaniesController(ISender sender)
    {
        _sender = sender;
    }

    public sealed record CreateCompanyRequest(string Name, string ContactEmail);

    // Public listing - a job board's companies are meant to be browsable without an account.
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCompaniesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    // Any authenticated Employer/Recruiter/Admin (job:create) can create a company - they become
    // its owner, which is the boundary CompanyOwnerOrPermissionRequirement scopes job management to.
    [HttpPost]
    [Authorize(Policy = "RequireJobCreate")]
    public async Task<IActionResult> Create(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                      ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new CreateCompanyCommand(request.Name, request.ContactEmail, ownerId);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value)
            : HandleFailure(result);
    }
}
