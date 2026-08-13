using JobBoard.Application.Companies.CreateCompany;
using JobBoard.Application.Companies.GetCompanies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers;

public sealed class CompaniesController : ApiControllerBase
{
    private readonly ISender _sender;

    public CompaniesController(ISender sender)
    {
        _sender = sender;
    }

    public sealed record CreateCompanyRequest(string Name, string ContactEmail);

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCompaniesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCompanyCommand(request.Name, request.ContactEmail);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value)
            : HandleFailure(result);
    }
}
