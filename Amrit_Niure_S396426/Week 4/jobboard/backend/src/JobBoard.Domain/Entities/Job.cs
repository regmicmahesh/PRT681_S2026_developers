using JobBoard.Domain.Common;
using JobBoard.Domain.Enums;
using JobBoard.Domain.Errors;
using JobBoard.Domain.Events;
using JobBoard.Domain.ValueObjects;

namespace JobBoard.Domain.Entities;

public class Job : AggregateRoot
{
    private readonly List<JobApplication> _applications = new();

    public string Title { get; private set; }
    public string Description { get; private set; }
    public EmploymentType EmploymentType { get; private set; }
    public SalaryRange Salary { get; private set; }
    public JobStatus Status { get; private set; }
    public Guid CompanyId { get; private set; }
    public IReadOnlyCollection<JobApplication> Applications => _applications.AsReadOnly();

    public Job(Guid id, string title, string description, EmploymentType employmentType, SalaryRange salary, Guid companyId)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId is required.", nameof(companyId));

        Title = title;
        Description = description;
        EmploymentType = employmentType;
        Salary = salary ?? throw new ArgumentNullException(nameof(salary));
        CompanyId = companyId;
        Status = JobStatus.Draft;
    }

    public Result Publish()
    {
        if (Status != JobStatus.Draft)
            return Result.Failure(DomainErrors.Job.AlreadyPublished(Status));

        Status = JobStatus.Published;
        RaiseDomainEvent(new JobPublishedDomainEvent(Id));
        return Result.Success();
    }

    public Result Close()
    {
        if (Status != JobStatus.Published)
            return Result.Failure(DomainErrors.Job.AlreadyClosed(Status));

        Status = JobStatus.Closed;
        RaiseDomainEvent(new JobClosedDomainEvent(Id));
        return Result.Success();
    }

    public Result<JobApplication> Apply(Guid applicationId, string candidateName, Email candidateEmail, string resumeUrl)
    {
        if (Status != JobStatus.Published)
            return Result.Failure<JobApplication>(DomainErrors.Job.NotPublished(Status));

        var application = new JobApplication(applicationId, Id, candidateName, candidateEmail, resumeUrl);
        _applications.Add(application);
        RaiseDomainEvent(new JobApplicationSubmittedDomainEvent(Id, application.Id, candidateName));
        return application;
    }

    public Result Shortlist(Guid applicationId)
    {
        var application = FindApplication(applicationId);
        if (application is null)
            return Result.Failure(DomainErrors.JobApplication.NotFound(applicationId));

        var result = application.Shortlist();
        if (result.IsSuccess)
            RaiseDomainEvent(new JobApplicationShortlistedDomainEvent(Id, applicationId));

        return result;
    }

    public Result Reject(Guid applicationId)
    {
        var application = FindApplication(applicationId);
        if (application is null)
            return Result.Failure(DomainErrors.JobApplication.NotFound(applicationId));

        var result = application.Reject();
        if (result.IsSuccess)
            RaiseDomainEvent(new JobApplicationRejectedDomainEvent(Id, applicationId));

        return result;
    }

    private JobApplication? FindApplication(Guid applicationId) =>
        _applications.FirstOrDefault(a => a.Id == applicationId);
}
