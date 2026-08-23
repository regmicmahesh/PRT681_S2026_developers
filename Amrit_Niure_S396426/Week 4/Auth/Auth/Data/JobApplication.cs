namespace Auth.Data;

public sealed class JobApplication
{
    public Guid Id { get; set; }

    public Guid JobId { get; set; }
    public Job Job { get; set; } = default!;

    // The ApplicantUserId is the ownership boundary application:read-own is scoped to - a
    // JobSeeker's "my applications" list. Candidate name/email/initials are read from the linked
    // ApplicationUser at response time rather than duplicated here, since Identity is local.
    public string ApplicantUserId { get; set; } = string.Empty;
    public ApplicationUser Applicant { get; set; } = default!;

    public string ResumeUrl { get; set; } = string.Empty;
    public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Submitted;
    public DateTime AppliedAtUtc { get; set; }
}
