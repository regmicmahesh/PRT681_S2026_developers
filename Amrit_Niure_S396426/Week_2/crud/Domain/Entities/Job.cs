using Domain.Common;
using Domain.Enums;
using Domain.Errors;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain
{
    public class Job : AggregateRoot
    {
        private readonly List<JobApplication> _applications = new();

        public string Title { get; private set; }
        public SalaryRange Salary { get; private set; }
        public JobStatus Status { get; private set; }
        public Guid CompanyId { get; private set; }
        //public IReadOnlyCollection<JobApplication> Applications => _applications.AsReadOnly();

        public Job(Guid id, string title, SalaryRange salary, Guid companyId) : base(id)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));

            if (companyId == Guid.Empty)
                throw new ArgumentException("CompanyId is required.", nameof(companyId));

            Title = title;
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
            return Result.Success();
        }

        public Result<JobApplication> Apply(Guid applicationId, string candidateName, string resumeUrl)
        {
            if (Status != JobStatus.Published)
                return Result.Failure<JobApplication>(DomainErrors.Job.NotPublished(Status));

            var application = new JobApplication(applicationId, Id, candidateName, resumeUrl);
            _applications.Add(application);
            RaiseDomainEvent(new JobApplicationSubmittedDomainEvent(Id, application.Id, candidateName));
            return application;
        }

        public Result Shortlist(Guid applicationId)
        {
            var application = FindApplication(applicationId);

            if (application is null)
            {
                return Result.Failure(DomainErrors.JobApplication.NotFound(applicationId));
            }

            application.Shortlist();
            return Result.Success();
        }

        public Result Reject(Guid applicationId)
        {
            var application = FindApplication(applicationId);

            if (application is null)
            {
                return Result.Failure(DomainErrors.JobApplication.NotFound(applicationId));
            }

            application.Reject();
            return Result.Success();
        }

        private JobApplication? FindApplication(Guid applicationId)
        {
            return _applications.FirstOrDefault(a => a.Id == applicationId);
        }
    }
}
