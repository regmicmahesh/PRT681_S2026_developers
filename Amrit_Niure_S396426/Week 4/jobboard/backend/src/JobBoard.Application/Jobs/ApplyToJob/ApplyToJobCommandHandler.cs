using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Errors;
using JobBoard.Domain.Repositories;
using JobBoard.Domain.ValueObjects;

namespace JobBoard.Application.Jobs.ApplyToJob;

internal sealed class ApplyToJobCommandHandler : ICommandHandler<ApplyToJobCommand, Guid>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApplyToJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ApplyToJobCommand command, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(command.JobId, cancellationToken);
        if (job is null)
            return Result.Failure<Guid>(DomainErrors.Job.NotFound(command.JobId));

        var candidateEmail = new Email(command.CandidateEmail);
        var result = job.Apply(Guid.NewGuid(), command.CandidateName, candidateEmail, command.ResumeUrl);
        if (result.IsFailure)
            return Result.Failure<Guid>(result.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result.Value.Id;
    }
}
