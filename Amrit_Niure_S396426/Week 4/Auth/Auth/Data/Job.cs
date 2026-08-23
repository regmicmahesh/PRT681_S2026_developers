namespace Auth.Data;

public sealed class Job
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EmploymentType EmploymentType { get; set; }
    public decimal SalaryMin { get; set; }
    public decimal SalaryMax { get; set; }
    public string SalaryCurrency { get; set; } = "USD";
    public PayPeriod PayPeriod { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Draft;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<JobApplication> Applications { get; set; } = [];
}
