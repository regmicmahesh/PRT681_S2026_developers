using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Repositories;

namespace JobBoard.Application.Jobs.GetJobs;

internal sealed class GetJobsQueryHandler : IQueryHandler<GetJobsQuery, IReadOnlyList<JobResponse>>
{
    private readonly IJobRepository _jobRepository;

    public GetJobsQueryHandler(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Result<IReadOnlyList<JobResponse>>> Handle(GetJobsQuery query, CancellationToken cancellationToken)
    {
        var jobs = await _jobRepository.GetAllAsync(cancellationToken);

        IReadOnlyList<JobResponse> response = jobs
            .Select(job => new JobResponse(
                job.Id,
                job.Title,
                job.Description,
                job.EmploymentType.ToString(),
                job.Salary.Min,
                job.Salary.Max,
                job.Salary.Currency,
                job.Status.ToString(),
                job.CompanyId,
                job.Applications.Count))
            .ToList();

        return Result.Success(response);
    }
}
