using Domain.Common;
using Domain.Enums;
using Domain.Errors;

namespace Domain
{
    public class JobApplication : Entity
    {
        public Guid JobId { get; private set; }
        public string CandidateName { get; private set; }
        public string ResumeUrl { get; private set; }
        public JobApplicationStatus Status { get; private set; }

        internal JobApplication(Guid id, Guid jobId, string candidateName, string resumeUrl) : base(id)
        {
            if (jobId == Guid.Empty)
                throw new ArgumentException("JobId is required.", nameof(jobId));

            if (string.IsNullOrWhiteSpace(candidateName))
                throw new ArgumentException("Candidate name is required.", nameof(candidateName));

            JobId = jobId;
            CandidateName = candidateName;
            ResumeUrl = resumeUrl;
            Status = JobApplicationStatus.Submitted;
        }

        internal Result Shortlist()
        {
            if (Status != JobApplicationStatus.Submitted)
            {
                return Result.Failure(DomainErrors.JobApplication.CannotShortlist(Status));
            }

            Status = JobApplicationStatus.Shortlisted;
            return Result.Success();
        }

        internal Result Reject()
        {
            if (Status == JobApplicationStatus.Rejected)
            {
                return Result.Failure(DomainErrors.JobApplication.AlreadyRejected);
            }

            Status = JobApplicationStatus.Rejected;
            return Result.Success();
        }
    }
}
