using JobBoard.Application.Messaging;

namespace JobBoard.Application.Jobs.GetJobs;

public sealed record GetJobsQuery : IQuery<IReadOnlyList<JobResponse>>;
