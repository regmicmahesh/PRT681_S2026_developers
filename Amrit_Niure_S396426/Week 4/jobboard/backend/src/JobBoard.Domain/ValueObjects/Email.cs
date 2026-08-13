using System.Text.RegularExpressions;

namespace JobBoard.Domain.ValueObjects;

public sealed partial record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.", nameof(value));

        if (!EmailRegex().IsMatch(value))
            throw new ArgumentException("Email is not in a valid format.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
