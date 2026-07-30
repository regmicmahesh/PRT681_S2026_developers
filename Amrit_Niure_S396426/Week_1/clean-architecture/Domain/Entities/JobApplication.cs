using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;

namespace Domain.Entities;

public class JobApplication : AggregateRoot<Guid>
{
    public Guid JobPostId { get; private set; }
    public Guid JobSeekerId { get; private set; }
    public string CoverLetter { get; private set; } = default!;
    public string? ResumeUrl { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public DateTime AppliedAtUtc { get; private set; }

    private JobApplication() { } // Required for ORM

    public JobApplication(
        Guid id, 
        Guid jobPostId, 
        Guid jobSeekerId, 
        string coverLetter, 
        string? resumeUrl = null) : base(id)
    {
        if (jobPostId == Guid.Empty)
            throw new DomainException("Job post identifier is required.");

        if (jobSeekerId == Guid.Empty)
            throw new DomainException("Job seeker identifier is required.");

        if (string.IsNullOrWhiteSpace(coverLetter))
            throw new DomainException("Cover letter is required.");

        JobPostId = jobPostId;
        JobSeekerId = jobSeekerId;
        CoverLetter = coverLetter.Trim();
        ResumeUrl = resumeUrl?.Trim();
        Status = ApplicationStatus.Submitted;
        AppliedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new JobApplicationSubmittedEvent(Id, JobPostId, JobSeekerId, AppliedAtUtc));
    }

    public static JobApplication Create(
        Guid jobPostId, 
        Guid jobSeekerId, 
        string coverLetter, 
        string? resumeUrl = null)
    {
        return new JobApplication(Guid.NewGuid(), jobPostId, jobSeekerId, coverLetter, resumeUrl);
    }

    public void UpdateStatus(ApplicationStatus newStatus)
    {
        if (Status == newStatus)
            return;

        if (Status == ApplicationStatus.Withdrawn || Status == ApplicationStatus.Rejected)
            throw new InvalidJobOperationException($"Cannot update status of an application that is already '{Status}'.");

        var oldStatus = Status;
        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new ApplicationStatusChangedEvent(Id, oldStatus, newStatus, UpdatedAtUtc.Value));
    }

    public void Withdraw()
    {
        UpdateStatus(ApplicationStatus.Withdrawn);
    }

    public void Shortlist()
    {
        UpdateStatus(ApplicationStatus.Shortlisted);
    }

    public void Reject()
    {
        UpdateStatus(ApplicationStatus.Rejected);
    }
}
