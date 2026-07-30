using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Company : AggregateRoot<Guid>
{
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Industry { get; private set; } = default!;
    public ContactInfo ContactInfo { get; private set; } = default!;
    public Location? HeadquarterLocation { get; private set; }
    public string? LogoUrl { get; private set; }
    public bool IsVerified { get; private set; }

    private Company() { } // Required for ORM

    public Company(
        Guid id, 
        string name, 
        string description, 
        string industry, 
        ContactInfo contactInfo, 
        Location? headquarterLocation = null,
        string? logoUrl = null) : base(id)
    {
        UpdateProfile(name, description, industry, contactInfo, headquarterLocation, logoUrl);
    }

    public static Company Create(
        string name, 
        string description, 
        string industry, 
        ContactInfo contactInfo, 
        Location? headquarterLocation = null,
        string? logoUrl = null)
    {
        return new Company(Guid.NewGuid(), name, description, industry, contactInfo, headquarterLocation, logoUrl);
    }

    public void UpdateProfile(
        string name, 
        string description, 
        string industry, 
        ContactInfo contactInfo, 
        Location? headquarterLocation = null,
        string? logoUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Company name is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Company description is required.");

        if (string.IsNullOrWhiteSpace(industry))
            throw new DomainException("Industry is required.");

        Name = name.Trim();
        Description = description.Trim();
        Industry = industry.Trim();
        ContactInfo = contactInfo ?? throw new DomainException("Contact info is required.");
        HeadquarterLocation = headquarterLocation;
        LogoUrl = logoUrl?.Trim();
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Verify()
    {
        IsVerified = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RevokeVerification()
    {
        IsVerified = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
