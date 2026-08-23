using Auth.Data;

namespace Auth.Auth;

public record JobApplicationResponse(
    Guid Id,
    Guid JobId,
    string ApplicantUserId,
    string? ApplicantEmail,
    string? ApplicantInitials,
    string ResumeUrl,
    string Status,
    DateTime AppliedAtUtc)
{
    public static JobApplicationResponse FromApplication(JobApplication application) => new(
        application.Id,
        application.JobId,
        application.ApplicantUserId,
        application.Applicant?.Email,
        application.Applicant?.Initials,
        application.ResumeUrl,
        application.Status.ToString(),
        application.AppliedAtUtc);
}
