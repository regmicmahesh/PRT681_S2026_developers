using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Errors;
using JobBoard.Domain.Repositories;

namespace JobBoard.Application.Jobs.GetJobById;

internal sealed class GetJobByIdQueryHandler : IQueryHandler<GetJobByIdQuery, JobResponse>
{
    private readonly IJobRepository _jobRepository;

    public GetJobByIdQueryHandler(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Result<JobResponse>> Handle(GetJobByIdQuery query, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(query.JobId, cancellationToken);
        if (job is null)
            return Result.Failure<JobResponse>(DomainErrors.Job.NotFound(query.JobId));

        var response = new JobResponse(
            job.Id,
            job.Title,
            job.Description,
            job.EmploymentType.ToString(),
            job.Salary.Min,
            job.Salary.Max,
            job.Salary.Currency.Code,
            job.Salary.PayPeriod.ToString(),
            job.Status.ToString(),
            job.CompanyId,
            job.Applications.Count);

        return response;
    }
}
