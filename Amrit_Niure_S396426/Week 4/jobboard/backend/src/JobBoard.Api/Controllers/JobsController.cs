using JobBoard.Application.Jobs.ApplyToJob;
using JobBoard.Application.Jobs.CloseJob;
using JobBoard.Application.Jobs.CreateJob;
using JobBoard.Application.Jobs.GetJobById;
using JobBoard.Application.Jobs.GetJobs;
using JobBoard.Application.Jobs.PublishJob;
using JobBoard.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers;

public sealed class JobsController : ApiControllerBase
{
    private readonly ISender _sender;

    public JobsController(ISender sender)
    {
        _sender = sender;
    }

    public sealed record CreateJobRequest(
        string Title,
        string Description,
        EmploymentType EmploymentType,
        decimal SalaryMin,
        decimal SalaryMax,
        string SalaryCurrency,
        Guid CompanyId);

    public sealed record ApplyToJobRequest(string CandidateName, string CandidateEmail, string ResumeUrl);

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetJobsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetJobByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateJobCommand(
            request.Title,
            request.Description,
            request.EmploymentType,
            request.SalaryMin,
            request.SalaryMax,
            request.SalaryCurrency,
            request.CompanyId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : HandleFailure(result);
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new PublishJobCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }

    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CloseJobCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }

    [HttpPost("{id:guid}/applications")]
    public async Task<IActionResult> Apply(Guid id, ApplyToJobRequest request, CancellationToken cancellationToken)
    {
        var command = new ApplyToJobCommand(id, request.CandidateName, request.CandidateEmail, request.ResumeUrl);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id }, result.Value)
            : HandleFailure(result);
    }
}
