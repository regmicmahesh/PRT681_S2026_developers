using JobTrackr.Mvc.Enums;

namespace JobTrackr.Mvc.Models;

public class JobApplication
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string? JobUrl { get; set; }

    public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Draft;

    public DateOnly? DateApplied { get; set; }

    public decimal? MinimumSalary { get; set; }

    public decimal? MaximumSalary { get; set; }

    public string? Currency { get; set; }

    public SalaryPeriod? SalaryPeriod { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    private JobApplication()
    {
    }

    public static JobApplication Create(string companyName, string jobTitle)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required.");

        if (string.IsNullOrWhiteSpace(jobTitle))
            throw new ArgumentException("Job title is required.");

        var now = DateTimeOffset.UtcNow;

        return new JobApplication
        {
            Id = Guid.NewGuid(),
            CompanyName = companyName.Trim(),
            JobTitle = jobTitle.Trim(),
            ApplicationStatus = ApplicationStatus.Draft,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
