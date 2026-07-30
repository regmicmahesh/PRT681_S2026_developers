namespace Domain.Entities;

public class JobApplication
{
    public Guid Id { get; private set; }
    public Guid JobPostingId { get; private set; }
    public string ApplicantName { get; private set; }
    public string ApplicantEmail { get; private set; }
    public string ResumeUrl { get; private set; }
    public DateTime AppliedOnUtc { get; private set; }

    private JobApplication() { }

    public JobApplication(
        Guid id,
        Guid jobPostingId,
        string applicantName,
        string applicantEmail,
        string resumeUrl,
        DateTime appliedOnUtc)
    {
        Id = id;
        JobPostingId = jobPostingId;
        ApplicantName = applicantName;
        ApplicantEmail = applicantEmail;
        ResumeUrl = resumeUrl;
        AppliedOnUtc = appliedOnUtc;
    }
}
