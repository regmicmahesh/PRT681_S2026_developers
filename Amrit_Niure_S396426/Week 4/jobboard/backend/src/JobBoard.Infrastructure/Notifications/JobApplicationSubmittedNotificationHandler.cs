using JobBoard.Domain.Events;
using JobBoard.Domain.Repositories;
using MediatR;

namespace JobBoard.Infrastructure.Notifications;

internal sealed class JobApplicationSubmittedNotificationHandler : INotificationHandler<JobApplicationSubmittedDomainEvent>
{
    private readonly IJobRepository _jobRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmailSender _emailSender;

    public JobApplicationSubmittedNotificationHandler(
        IJobRepository jobRepository,
        ICompanyRepository companyRepository,
        IEmailSender emailSender)
    {
        _jobRepository = jobRepository;
        _companyRepository = companyRepository;
        _emailSender = emailSender;
    }

    public async Task Handle(JobApplicationSubmittedDomainEvent notification, CancellationToken cancellationToken)
    {
        var job = await _jobRepository.GetByIdAsync(notification.JobId, cancellationToken);
        if (job is null)
            return;

        var company = await _companyRepository.GetByIdAsync(job.CompanyId, cancellationToken);
        if (company is null)
            return;

        await _emailSender.SendAsync(
            company.ContactEmail.Value,
            $"New application for {job.Title}",
            $"{notification.CandidateName} just applied for '{job.Title}'.",
            cancellationToken);
    }
}
