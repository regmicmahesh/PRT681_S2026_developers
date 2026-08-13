using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories;
using JobBoard.Domain.ValueObjects;

namespace JobBoard.Application.Jobs.CreateJob;

internal sealed class CreateJobCommandHandler : ICommandHandler<CreateJobCommand, Guid>
{
    private readonly IJobRepository _jobRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var currency = new Currency(command.SalaryCurrency);
        var salary = new SalaryRange(command.SalaryMin, command.SalaryMax, currency, command.PayPeriod);

        var job = new Job(
            Guid.NewGuid(),
            command.Title,
            command.Description,
            command.EmploymentType,
            salary,
            command.CompanyId);

        _jobRepository.Add(job);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
