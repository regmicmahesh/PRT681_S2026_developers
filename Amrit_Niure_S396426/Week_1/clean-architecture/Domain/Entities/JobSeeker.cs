using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class JobSeeker : AggregateRoot<Guid>
{
    private readonly List<string> _skills = [];

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public ContactInfo ContactInfo { get; private set; } = default!;
    public string? Headline { get; private set; }
    public string? Bio { get; private set; }
    public string? ResumeUrl { get; private set; }
    public Location? PreferredLocation { get; private set; }
    public IReadOnlyCollection<string> Skills => _skills.AsReadOnly();

    public string FullName => $"{FirstName} {LastName}";

    private JobSeeker() { } // Required for ORM

    public JobSeeker(
        Guid id, 
        string firstName, 
        string lastName, 
        ContactInfo contactInfo, 
        string? headline = null,
        string? bio = null,
        string? resumeUrl = null,
        Location? preferredLocation = null) : base(id)
    {
        UpdateProfile(firstName, lastName, contactInfo, headline, bio, resumeUrl, preferredLocation);
    }

    public static JobSeeker Create(
        string firstName, 
        string lastName, 
        ContactInfo contactInfo, 
        string? headline = null,
        string? bio = null,
        string? resumeUrl = null,
        Location? preferredLocation = null)
    {
        return new JobSeeker(Guid.NewGuid(), firstName, lastName, contactInfo, headline, bio, resumeUrl, preferredLocation);
    }

    public void UpdateProfile(
        string firstName, 
        string lastName, 
        ContactInfo contactInfo, 
        string? headline = null,
        string? bio = null,
        string? resumeUrl = null,
        Location? preferredLocation = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        ContactInfo = contactInfo ?? throw new DomainException("Contact info is required.");
        Headline = headline?.Trim();
        Bio = bio?.Trim();
        ResumeUrl = resumeUrl?.Trim();
        PreferredLocation = preferredLocation;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddSkill(string skill)
    {
        if (string.IsNullOrWhiteSpace(skill))
            return;

        var normalizedSkill = skill.Trim();
        if (!_skills.Contains(normalizedSkill, StringComparer.OrdinalIgnoreCase))
        {
            _skills.Add(normalizedSkill);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    public void RemoveSkill(string skill)
    {
        var existingSkill = _skills.FirstOrDefault(s => s.Equals(skill.Trim(), StringComparison.OrdinalIgnoreCase));
        if (existingSkill is not null)
        {
            _skills.Remove(existingSkill);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
