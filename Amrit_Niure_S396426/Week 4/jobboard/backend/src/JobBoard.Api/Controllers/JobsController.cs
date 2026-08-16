using JobBoard.Api.Authorization;
using JobBoard.Application.Jobs.ApplyToJob;
using JobBoard.Application.Jobs.CloseJob;
using JobBoard.Application.Jobs.CreateJob;
using JobBoard.Application.Jobs.GetJobById;
using JobBoard.Application.Jobs.GetJobs;
using JobBoard.Application.Jobs.PublishJob;
using JobBoard.Domain.Enums;
using JobBoard.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers;

public sealed class JobsController : ApiControllerBase
{
    private readonly ISender _sender;
    private readonly IJobRepository _jobRepository;
    private readonly IAuthorizationService _authorizationService;

    public JobsController(ISender sender, IJobRepository jobRepository, IAuthorizationService authorizationService)
    {
        _sender = sender;
        _jobRepository = jobRepository;
        _authorizationService = authorizationService;
    }

    public sealed record CreateJobRequest(
        string Title,
        string Description,
        EmploymentType EmploymentType,
        decimal SalaryMin,
        decimal SalaryMax,
        string SalaryCurrency,
        PayPeriod PayPeriod,
        Guid CompanyId);

    public sealed record ApplyToJobRequest(string CandidateName, string CandidateEmail, string ResumeUrl);

    // Public listing/detail - a job board's postings are meant to be browsable without an account.
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetJobsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetJobByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    // [Authorize(Policy = "RequireJobCreate")] only confirms the caller CAN create jobs in
    // general (Employer/Recruiter/Admin). Whether they can post to THIS company is a resource
    // check below - job:create is granted to the whole role, not scoped to one company.
    [HttpPost]
    [Authorize(Policy = "RequireJobCreate")]
    public async Task<IActionResult> Create(CreateJobRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authorizationService.AuthorizeAsync(
            User, request.CompanyId, new CompanyOwnerOrPermissionRequirement(Permissions.JobManageAny));
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        var command = new CreateJobCommand(
            request.Title,
            request.Description,
            request.EmploymentType,
            request.SalaryMin,
            request.SalaryMax,
            request.SalaryCurrency,
            request.PayPeriod,
            request.CompanyId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : HandleFailure(result);
    }

    [HttpPost("{id:guid}/publish")]
    [Authorize(Policy = "RequireJobUpdate")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken)
    {
        var forbidden = await AuthorizeAgainstJobOwnerAsync(id, cancellationToken);
        if (forbidden is not null)
        {
            return forbidden;
        }

        var result = await _sender.Send(new PublishJobCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }

    [HttpPost("{id:guid}/close")]
    [Authorize(Policy = "RequireJobUpdate")]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        var forbidden = await AuthorizeAgainstJobOwnerAsync(id, cancellationToken);
        if (forbidden is not null)
        {
            return forbidden;
        }

        var result = await _sender.Send(new CloseJobCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }

    // No ownership scoping here by design - any JobSeeker (job:apply) may apply to any published
    // job, unlike Create/Publish/Close which are scoped to the posting company.
    [HttpPost("{id:guid}/applications")]
    [Authorize(Policy = "RequireJobApply")]
    public async Task<IActionResult> Apply(Guid id, ApplyToJobRequest request, CancellationToken cancellationToken)
    {
        var command = new ApplyToJobCommand(id, request.CandidateName, request.CandidateEmail, request.ResumeUrl);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id }, result.Value)
            : HandleFailure(result);
    }

    // Shared by Publish/Close: both mutate an existing job, so both need to resolve its owning
    // company before the ownership check can run. Returns null when the caller is authorized.
    private async Task<IActionResult?> AuthorizeAgainstJobOwnerAsync(Guid jobId, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(jobId, cancellationToken);
        if (job is null)
        {
            return NotFound();
        }

        var authResult = await _authorizationService.AuthorizeAsync(
            User, job.CompanyId, new CompanyOwnerOrPermissionRequirement(Permissions.JobManageAny));

        return authResult.Succeeded ? null : Forbid();
    }
}
