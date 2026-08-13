using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Errors;
using JobBoard.Domain.Repositories;

namespace JobBoard.Application.Jobs.PublishJob;

internal sealed class PublishJobCommandHandler : ICommandHandler<PublishJobCommand>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PublishJobCommand command, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(command.JobId, cancellationToken);
        if (job is null)
            return Result.Failure(DomainErrors.Job.NotFound(command.JobId));

        var result = job.Publish();
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
