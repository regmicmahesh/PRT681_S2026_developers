using JobBoard.Domain.Common;
using JobBoard.Domain.Enums;
using JobBoard.Domain.Errors;
using JobBoard.Domain.ValueObjects;

namespace JobBoard.Domain.Entities;

public class JobApplication : Entity
{
    public Guid JobId { get; private set; }
    public string CandidateName { get; private set; } = null!;
    public Email CandidateEmail { get; private set; } = null!;
    public string ResumeUrl { get; private set; } = null!;
    public JobApplicationStatus Status { get; private set; }

    // Reserved for EF Core materialization.
    private JobApplication()
    {
    }

    internal JobApplication(Guid id, Guid jobId, string candidateName, Email candidateEmail, string resumeUrl) : base(id)
    {
        if (jobId == Guid.Empty)
            throw new ArgumentException("JobId is required.", nameof(jobId));

        if (string.IsNullOrWhiteSpace(candidateName))
            throw new ArgumentException("Candidate name is required.", nameof(candidateName));

        if (string.IsNullOrWhiteSpace(resumeUrl))
            throw new ArgumentException("Resume URL is required.", nameof(resumeUrl));

        JobId = jobId;
        CandidateName = candidateName;
        CandidateEmail = candidateEmail ?? throw new ArgumentNullException(nameof(candidateEmail));
        ResumeUrl = resumeUrl;
        Status = JobApplicationStatus.Submitted;
    }

    internal Result Shortlist()
    {
        if (Status != JobApplicationStatus.Submitted)
            return Result.Failure(DomainErrors.JobApplication.CannotShortlist(Status));

        Status = JobApplicationStatus.Shortlisted;
        return Result.Success();
    }

    internal Result Accept()
    {
        if (Status != JobApplicationStatus.Shortlisted)
            return Result.Failure(DomainErrors.JobApplication.CannotAccept(Status));

        Status = JobApplicationStatus.Accepted;
        return Result.Success();
    }

    internal Result Reject()
    {
        if (Status == JobApplicationStatus.Rejected)
            return Result.Failure(DomainErrors.JobApplication.AlreadyRejected);

        Status = JobApplicationStatus.Rejected;
        return Result.Success();
    }
}
