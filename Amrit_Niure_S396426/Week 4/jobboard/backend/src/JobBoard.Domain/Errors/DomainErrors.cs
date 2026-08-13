using JobBoard.Domain.Common;
using JobBoard.Domain.Enums;

namespace JobBoard.Domain.Errors;

public static class DomainErrors
{
    public static class Job
    {
        public static readonly Func<Guid, Error> NotFound = id =>
            new("Job.NotFound", $"The job with Id '{id}' was not found.");

        public static readonly Func<JobStatus, Error> AlreadyPublished = status =>
            new("Job.AlreadyPublished", $"Cannot publish a job from status '{status}'. Only a draft job can be published.");

        public static readonly Func<JobStatus, Error> AlreadyClosed = status =>
            new("Job.AlreadyClosed", $"Cannot close a job from status '{status}'. Only a published job can be closed.");

        public static readonly Func<JobStatus, Error> NotPublished = status =>
            new("Job.NotPublished", $"Cannot apply to a job from status '{status}'. Only a published job accepts applications.");
    }

    public static class JobApplication
    {
        public static readonly Func<Guid, Error> NotFound = id =>
            new("JobApplication.NotFound", $"No application with Id '{id}' exists for this job.");

        public static readonly Func<JobApplicationStatus, Error> CannotShortlist = status =>
            new("JobApplication.CannotShortlist", $"Cannot shortlist an application with status '{status}'. Only submitted applications can be shortlisted.");

        public static readonly Func<JobApplicationStatus, Error> CannotAccept = status =>
            new("JobApplication.CannotAccept", $"Cannot accept an application with status '{status}'. Only shortlisted applications can be accepted.");

        public static readonly Error AlreadyRejected = new(
            "JobApplication.AlreadyRejected",
            "The application is already rejected.");
    }

    public static class Company
    {
        public static readonly Func<Guid, Error> NotFound = id =>
            new("Company.NotFound", $"The company with Id '{id}' was not found.");
    }
}
