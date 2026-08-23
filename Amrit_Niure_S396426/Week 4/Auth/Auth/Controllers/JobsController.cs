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
[Route("jobs")]
public class JobsController(
    ApplicationDbContext dbContext,
    IAuthorizationService authorizationService) : ControllerBase
{
    public record CreateJobRequest(
        string Title,
        string Description,
        EmploymentType EmploymentType,
        decimal SalaryMin,
        decimal SalaryMax,
        string SalaryCurrency,
        PayPeriod PayPeriod,
        Guid CompanyId);

    public record UpdateJobRequest(
        string Title,
        string Description,
        EmploymentType EmploymentType,
        decimal SalaryMin,
        decimal SalaryMax,
        string SalaryCurrency,
        PayPeriod PayPeriod);

    public record ApplyToJobRequest(string ResumeUrl);

    // Public listing - only Published jobs are anyone's business to browse. A company's Draft/
    // Closed jobs are visible to its owner via GET /companies/{id}/jobs instead.
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await dbContext.Jobs
            .Include(j => j.Applications)
            .AsNoTracking()
            .Where(j => j.Status == JobStatus.Published)
            .ToListAsync();

        return Ok(jobs.Select(JobResponse.FromJob));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var job = await dbContext.Jobs
            .Include(j => j.Applications)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id);

        return job is null ? NotFound() : Ok(JobResponse.FromJob(job));
    }

    // [Authorize(Policy = "RequireJobCreate")] only confirms the caller CAN create jobs in
    // general (Employer/Recruiter/Admin). Whether they can post to THIS company is a resource
    // check below - job:create is granted to the whole role, not scoped to one company.
    [HttpPost]
    [Authorize(Policy = "RequireJobCreate")]
    public async Task<IActionResult> Create(CreateJobRequest request)
    {
        var authResult = await authorizationService.AuthorizeAsync(
            User, request.CompanyId, new CompanyOwnerOrPermissionRequirement(Permissions.JobManageAny));
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        if (request.SalaryMax < request.SalaryMin)
        {
            return BadRequest(new { Error = "SalaryMax cannot be less than SalaryMin." });
        }

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            EmploymentType = request.EmploymentType,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            SalaryCurrency = request.SalaryCurrency,
            PayPeriod = request.PayPeriod,
            Status = JobStatus.Draft,
            CompanyId = request.CompanyId,
            CreatedAtUtc = DateTime.UtcNow,
        };

        dbContext.Jobs.Add(job);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = job.Id }, JobResponse.FromJob(job));
    }

    // Closed jobs are historical record - editing one after the fact would rewrite what
    // candidates actually applied to. Draft and Published jobs can still be corrected/adjusted.
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireJobUpdate")]
    public async Task<IActionResult> Update(Guid id, UpdateJobRequest request)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job is null)
        {
            return NotFound();
        }

        var forbidden = await AuthorizeAgainstJobOwnerAsync(job.CompanyId);
        if (forbidden is not null)
        {
            return forbidden;
        }

        if (job.Status == JobStatus.Closed)
        {
            return BadRequest(new { Error = "Closed jobs cannot be edited." });
        }

        if (request.SalaryMax < request.SalaryMin)
        {
            return BadRequest(new { Error = "SalaryMax cannot be less than SalaryMin." });
        }

        job.Title = request.Title;
        job.Description = request.Description;
        job.EmploymentType = request.EmploymentType;
        job.SalaryMin = request.SalaryMin;
        job.SalaryMax = request.SalaryMax;
        job.SalaryCurrency = request.SalaryCurrency;
        job.PayPeriod = request.PayPeriod;
        await dbContext.SaveChangesAsync();

        return Ok(JobResponse.FromJob(job));
    }

    // Draft-only - once a job has been Published it may have live applications, so deleting it
    // outright would silently destroy candidates' application history. Close it instead.
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireJobDelete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job is null)
        {
            return NotFound();
        }

        var forbidden = await AuthorizeAgainstJobOwnerAsync(job.CompanyId);
        if (forbidden is not null)
        {
            return forbidden;
        }

        if (job.Status != JobStatus.Draft)
        {
            return BadRequest(new { Error = "Only a draft job can be deleted. Close a published job instead." });
        }

        dbContext.Jobs.Remove(job);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id:guid}/publish")]
    [Authorize(Policy = "RequireJobUpdate")]
    public async Task<IActionResult> Publish(Guid id)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job is null)
        {
            return NotFound();
        }

        var forbidden = await AuthorizeAgainstJobOwnerAsync(job.CompanyId);
        if (forbidden is not null)
        {
            return forbidden;
        }

        if (job.Status != JobStatus.Draft)
        {
            return BadRequest(new { Error = $"Cannot publish a job from status '{job.Status}'. Only a draft job can be published." });
        }

        job.Status = JobStatus.Published;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id:guid}/close")]
    [Authorize(Policy = "RequireJobUpdate")]
    public async Task<IActionResult> Close(Guid id)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job is null)
        {
            return NotFound();
        }

        var forbidden = await AuthorizeAgainstJobOwnerAsync(job.CompanyId);
        if (forbidden is not null)
        {
            return forbidden;
        }

        if (job.Status != JobStatus.Published)
        {
            return BadRequest(new { Error = $"Cannot close a job from status '{job.Status}'. Only a published job can be closed." });
        }

        job.Status = JobStatus.Closed;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    // No ownership scoping here by design - any JobSeeker (job:apply) may apply to any published
    // job, unlike Create/Update/Delete/Publish/Close which are scoped to the posting company.
    // ApplicantUserId is bound to the caller's own JWT sub, never client-supplied - otherwise one
    // JobSeeker could submit an application "as" another.
    [HttpPost("{id:guid}/applications")]
    [Authorize(Policy = "RequireJobApply")]
    public async Task<IActionResult> Apply(Guid id, ApplyToJobRequest request)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job is null)
        {
            return NotFound();
        }

        if (job.Status != JobStatus.Published)
        {
            return BadRequest(new { Error = $"Cannot apply to a job from status '{job.Status}'. Only a published job accepts applications." });
        }

        var applicantId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                          ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var alreadyApplied = await dbContext.JobApplications
            .AnyAsync(a => a.JobId == id && a.ApplicantUserId == applicantId);
        if (alreadyApplied)
        {
            return BadRequest(new { Error = "You have already applied to this job." });
        }

        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            JobId = id,
            ApplicantUserId = applicantId,
            ResumeUrl = request.ResumeUrl,
            Status = JobApplicationStatus.Submitted,
            AppliedAtUtc = DateTime.UtcNow,
        };

        dbContext.JobApplications.Add(application);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id }, JobApplicationResponse.FromApplication(application));
    }

    // JobSeeker's own application history - application:read-own, scoped to the caller by
    // construction (no ownership check needed; the filter below IS the scoping).
    [HttpGet("applications/mine")]
    [Authorize(Policy = "RequireApplicationReadOwn")]
    public async Task<IActionResult> GetMyApplications()
    {
        var applicantId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                          ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var applications = await dbContext.JobApplications
            .Include(a => a.Applicant)
            .AsNoTracking()
            .Where(a => a.ApplicantUserId == applicantId)
            .ToListAsync();

        return Ok(applications.Select(JobApplicationResponse.FromApplication));
    }

    // The owning company's view of who applied - application:read-any/manage, plus ownership
    // (an Employer only sees applicants to their own postings, not every company's).
    [HttpGet("{id:guid}/applications")]
    [Authorize(Policy = "RequireApplicationReadAny")]
    public async Task<IActionResult> GetApplications(Guid id)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job is null)
        {
            return NotFound();
        }

        var forbidden = await AuthorizeAgainstJobOwnerAsync(job.CompanyId);
        if (forbidden is not null)
        {
            return forbidden;
        }

        var applications = await dbContext.JobApplications
            .Include(a => a.Applicant)
            .AsNoTracking()
            .Where(a => a.JobId == id)
            .ToListAsync();

        return Ok(applications.Select(JobApplicationResponse.FromApplication));
    }

    [HttpPost("{id:guid}/applications/{applicationId:guid}/shortlist")]
    [Authorize(Policy = "RequireApplicationManage")]
    public Task<IActionResult> Shortlist(Guid id, Guid applicationId) =>
        TransitionApplicationAsync(id, applicationId, JobApplicationStatus.Submitted, JobApplicationStatus.Shortlisted);

    [HttpPost("{id:guid}/applications/{applicationId:guid}/reject")]
    [Authorize(Policy = "RequireApplicationManage")]
    public async Task<IActionResult> Reject(Guid id, Guid applicationId)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job is null)
        {
            return NotFound();
        }

        var forbidden = await AuthorizeAgainstJobOwnerAsync(job.CompanyId);
        if (forbidden is not null)
        {
            return forbidden;
        }

        var application = await dbContext.JobApplications
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.JobId == id);
        if (application is null)
        {
            return NotFound();
        }

        // Submitted or Shortlisted may be rejected; Accepted/already-Rejected may not.
        if (application.Status is not (JobApplicationStatus.Submitted or JobApplicationStatus.Shortlisted))
        {
            return BadRequest(new { Error = $"Cannot reject an application with status '{application.Status}'." });
        }

        application.Status = JobApplicationStatus.Rejected;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id:guid}/applications/{applicationId:guid}/accept")]
    [Authorize(Policy = "RequireApplicationManage")]
    public Task<IActionResult> Accept(Guid id, Guid applicationId) =>
        TransitionApplicationAsync(id, applicationId, JobApplicationStatus.Shortlisted, JobApplicationStatus.Accepted);

    // Shared by Publish/Close/Update/Delete/GetApplications: all mutate or read data scoped to an
    // existing job's owning company, so all need the same ownership check. Returns null when the
    // caller is authorized.
    private async Task<IActionResult?> AuthorizeAgainstJobOwnerAsync(Guid companyId)
    {
        var authResult = await authorizationService.AuthorizeAsync(
            User, companyId, new CompanyOwnerOrPermissionRequirement(Permissions.JobManageAny));

        return authResult.Succeeded ? null : Forbid();
    }

    private async Task<IActionResult> TransitionApplicationAsync(
        Guid jobId, Guid applicationId, JobApplicationStatus from, JobApplicationStatus to)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
        if (job is null)
        {
            return NotFound();
        }

        var forbidden = await AuthorizeAgainstJobOwnerAsync(job.CompanyId);
        if (forbidden is not null)
        {
            return forbidden;
        }

        var application = await dbContext.JobApplications
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.JobId == jobId);
        if (application is null)
        {
            return NotFound();
        }

        if (application.Status != from)
        {
            return BadRequest(new { Error = $"Cannot move an application from status '{application.Status}' to '{to}'. Expected '{from}'." });
        }

        application.Status = to;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
