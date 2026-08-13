using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Errors;
using JobBoard.Domain.Repositories;

namespace JobBoard.Application.Jobs.CloseJob;

internal sealed class CloseJobCommandHandler : ICommandHandler<CloseJobCommand>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CloseJobCommand command, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(command.JobId, cancellationToken);
        if (job is null)
            return Result.Failure(DomainErrors.Job.NotFound(command.JobId));

        var result = job.Close();
        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
