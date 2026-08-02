using Domain.Common;
using Domain.Enums;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Job
    {
        public static readonly Func<Guid, Error> NotFound = id => new("Job.NotFound", $"The job with Id '{id}' was not found.");
        public static readonly Func<JobStatus, Error> AlreadyPublished = Status => new("Job.AlreadyPublished", $"Cannot publish a job from status '{Status}'. Only a draft job can be published.");
        public static readonly Func<JobStatus, Error> AlreadyClosed = Status => new("Job.AlreadyClosed", $"Cannot close a job from status '{Status}'. Only a published job can be closed.");
        public static readonly Func<JobStatus, Error> NotPublished = Status => new("Job.NotPublished", $"Cannot apply to a job from status '{Status}'. Only a published job accepts applications.");
    }

    public static class JobApplication
    {
        public static readonly Func<Guid, Error> NotFound = id => new("Job.NotFound", $"No application with Id '{id}' exists for this job.");
        public static readonly Func<JobApplicationStatus, Error> CannotShortlist = status =>
            new("JobApplication.CannotShortlist", $"Cannot shortlist an application with status '{status}'. Only submitted applications can be shortlisted.");

        public static readonly Error AlreadyRejected = new(
            "JobApplication.AlreadyRejected",
            "The application is already rejected.");
    }
}
