using JobTrackr.Mvc.Enums;
using JobTrackr.Mvc.Models;

namespace JobTrackr.Mvc.Repositories;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly List<JobApplication> _jobApplications = [];

    public JobApplicationRepository()
    {
        Seed();
    }

    public void Add(JobApplication jobApplication)
    {
        if (_jobApplications.Any(j => j.Id == jobApplication.Id))
            throw new InvalidOperationException("A job application with this id already exists.");

        _jobApplications.Add(jobApplication);
    }

    public JobApplication? GetById(Guid id)
    {
        return _jobApplications.FirstOrDefault(j => j.Id == id);
    }

    public IReadOnlyList<JobApplication> GetAll()
    {
        return _jobApplications.ToList().AsReadOnly();
    }

    public bool Update(JobApplication jobApplication)
    {
        var existing = GetById(jobApplication.Id);

        if (existing is null)
            return false;

        existing.CompanyName = jobApplication.CompanyName;
        existing.JobTitle = jobApplication.JobTitle;
        existing.JobUrl = jobApplication.JobUrl;
        existing.ApplicationStatus = jobApplication.ApplicationStatus;
        existing.DateApplied = jobApplication.DateApplied;
        existing.MinimumSalary = jobApplication.MinimumSalary;
        existing.MaximumSalary = jobApplication.MaximumSalary;
        existing.Currency = jobApplication.Currency;
        existing.SalaryPeriod = jobApplication.SalaryPeriod;
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        return true;
    }

    public bool Delete(Guid id)
    {
        var job = GetById(id);

        if (job is null)
            return false;

        return _jobApplications.Remove(job);
    }

    private void Seed()
    {
        AddSample("Microsoft", "Software Developer", ApplicationStatus.Applied, 75000, 90000, 5);
        AddSample("NT Government", "Graduate Developer", ApplicationStatus.Screening, 78000, 92000, 10);
        AddSample("Amazon", "Backend Developer", ApplicationStatus.Interview, 90000, 110000, 15);
        AddSample("Atlassian", "Junior Software Engineer", ApplicationStatus.Draft, null, null, null);
        AddSample("Canva", "Software Engineer", ApplicationStatus.Rejected, 95000, 120000, 25);
        AddSample("Deloitte", ".NET Developer", ApplicationStatus.Applied, 85000, 105000, 18);
        AddSample("KPMG", "Technology Consultant", ApplicationStatus.Screening, 82000, 100000, 12);
        AddSample("WiseTech Global", "C# Developer", ApplicationStatus.Applied, 88000, 108000, 3);
    }

    private void AddSample(
        string company,
        string title,
        ApplicationStatus status,
        decimal? minimumSalary,
        decimal? maximumSalary,
        int? daysAgo)
    {
        var job = JobApplication.Create(company, title);

        job.ApplicationStatus = status;
        job.MinimumSalary = minimumSalary;
        job.MaximumSalary = maximumSalary;

        if (minimumSalary.HasValue || maximumSalary.HasValue)
        {
            job.Currency = "AUD";
            job.SalaryPeriod = SalaryPeriod.Yearly;
        }

        if (daysAgo.HasValue)
        {
            job.DateApplied =
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-daysAgo.Value));
        }

        Add(job);
    }
}
