using Domain.Common;

namespace Domain.ValueObjects;

public class Location : ValueObject
{
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string? AddressLine { get; }
    public bool IsRemote { get; }

    public Location(string city, string state, string country, bool isRemote = false, string? addressLine = null)
    {
        City = city?.Trim() ?? string.Empty;
        State = state?.Trim() ?? string.Empty;
        Country = country?.Trim() ?? string.Empty;
        IsRemote = isRemote;
        AddressLine = addressLine?.Trim();
    }

    public static Location Remote() => new(string.Empty, string.Empty, string.Empty, isRemote: true);

    public override string ToString() => IsRemote 
        ? "Remote" 
        : string.IsNullOrWhiteSpace(State) 
            ? $"{City}, {Country}" 
            : $"{City}, {State}, {Country}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return City;
        yield return State;
        yield return Country;
        yield return AddressLine;
        yield return IsRemote;
    }
}
