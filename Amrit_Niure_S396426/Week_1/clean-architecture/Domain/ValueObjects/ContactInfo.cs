using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public class ContactInfo : ValueObject
{
    public string Email { get; }
    public string? PhoneNumber { get; }
    public string? WebsiteUrl { get; }

    public ContactInfo(string email, string? phoneNumber = null, string? websiteUrl = null)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("A valid email address is required.");

        Email = email.Trim().ToLowerInvariant();
        PhoneNumber = phoneNumber?.Trim();
        WebsiteUrl = websiteUrl?.Trim();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
        yield return PhoneNumber;
        yield return WebsiteUrl;
    }
}
