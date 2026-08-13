using JobBoard.Application.Messaging;

namespace JobBoard.Application.Jobs.GetJobById;

public sealed record GetJobByIdQuery(Guid JobId) : IQuery<JobResponse>;
