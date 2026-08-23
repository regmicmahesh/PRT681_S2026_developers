using Auth.Data;

namespace Auth.Auth;

public record JobResponse(
    Guid Id,
    string Title,
    string Description,
    string EmploymentType,
    decimal SalaryMin,
    decimal SalaryMax,
    string SalaryCurrency,
    string PayPeriod,
    string Status,
    Guid CompanyId,
    DateTime CreatedAtUtc,
    int ApplicationCount)
{
    public static JobResponse FromJob(Job job) => new(
        job.Id,
        job.Title,
        job.Description,
        job.EmploymentType.ToString(),
        job.SalaryMin,
        job.SalaryMax,
        job.SalaryCurrency,
        job.PayPeriod.ToString(),
        job.Status.ToString(),
        job.CompanyId,
        job.CreatedAtUtc,
        job.Applications.Count);
}
