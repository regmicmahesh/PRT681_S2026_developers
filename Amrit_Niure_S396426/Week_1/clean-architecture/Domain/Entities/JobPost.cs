using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class JobPost : AggregateRoot<Guid>
{
    private readonly List<string> _tags = [];

    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Requirements { get; private set; } = default!;
    public JobType JobType { get; private set; }
    public WorkMode WorkMode { get; private set; }
    public ExperienceLevel ExperienceLevel { get; private set; }
    public SalaryRange? Salary { get; private set; }
    public Location? Location { get; private set; }
    public JobStatus Status { get; private set; }
    public DateTime? ExpirationDateUtc { get; private set; }
    public Guid CompanyId { get; private set; }
    public Guid CategoryId { get; private set; }

    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

    private JobPost() { } // Required for ORM

    public JobPost(
        Guid id,
        Guid companyId,
        Guid categoryId,
        string title,
        string description,
        string requirements,
        JobType jobType,
        WorkMode workMode,
        ExperienceLevel experienceLevel,
        SalaryRange? salary = null,
        Location? location = null,
        DateTime? expirationDateUtc = null) : base(id)
    {
        if (companyId == Guid.Empty)
            throw new DomainException("Company identifier is required.");

        if (categoryId == Guid.Empty)
            throw new DomainException("Category identifier is required.");

        CompanyId = companyId;
        CategoryId = categoryId;
        Status = JobStatus.Draft;

        UpdateDetails(title, description, requirements, jobType, workMode, experienceLevel, salary, location, expirationDateUtc);
    }

    public static JobPost Create(
        Guid companyId,
        Guid categoryId,
        string title,
        string description,
        string requirements,
        JobType jobType,
        WorkMode workMode,
        ExperienceLevel experienceLevel,
        SalaryRange? salary = null,
        Location? location = null,
        DateTime? expirationDateUtc = null)
    {
        return new JobPost(Guid.NewGuid(), companyId, categoryId, title, description, requirements, jobType, workMode, experienceLevel, salary, location, expirationDateUtc);
    }

    public void UpdateDetails(
        string title,
        string description,
        string requirements,
        JobType jobType,
        WorkMode workMode,
        ExperienceLevel experienceLevel,
        SalaryRange? salary = null,
        Location? location = null,
        DateTime? expirationDateUtc = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Job title is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Job description is required.");

        if (string.IsNullOrWhiteSpace(requirements))
            throw new DomainException("Job requirements are required.");

        if (expirationDateUtc.HasValue && expirationDateUtc.Value <= DateTime.UtcNow)
            throw new DomainException("Expiration date must be in the future.");

        Title = title.Trim();
        Description = description.Trim();
        Requirements = requirements.Trim();
        JobType = jobType;
        WorkMode = workMode;
        ExperienceLevel = experienceLevel;
        Salary = salary;
        Location = location;
        ExpirationDateUtc = expirationDateUtc;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Publish()
    {
        if (Status == JobStatus.Active)
            return;

        if (Status == JobStatus.Closed)
            throw new InvalidJobOperationException("Cannot publish a closed job post.");

        Status = JobStatus.Active;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(new JobPostPublishedEvent(Id, CompanyId, Title, UpdatedAtUtc.Value));
    }

    public void Pause()
    {
        if (Status != JobStatus.Active)
            throw new InvalidJobOperationException("Only active job posts can be paused.");

        Status = JobStatus.Paused;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status == JobStatus.Closed)
            return;

        Status = JobStatus.Closed;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status == JobStatus.Closed || Status == JobStatus.Expired)
            return;

        Status = JobStatus.Expired;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag)) return;
        var cleanTag = tag.Trim().ToLowerInvariant();
        if (!_tags.Contains(cleanTag))
        {
            _tags.Add(cleanTag);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    public void RemoveTag(string tag)
    {
        var cleanTag = tag.Trim().ToLowerInvariant();
        if (_tags.Remove(cleanTag))
        {
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
