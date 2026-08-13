using System.Text.RegularExpressions;

namespace JobBoard.Domain.ValueObjects;

public sealed partial record Currency
{
    public string Code { get; }

    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code is required.", nameof(code));

        var normalized = code.Trim().ToUpperInvariant();

        if (!CurrencyCodeRegex().IsMatch(normalized))
            throw new ArgumentException("Currency code must be a 3-letter ISO 4217 code (e.g. USD).", nameof(code));

        Code = normalized;
    }

    public override string ToString() => Code;

    [GeneratedRegex("^[A-Z]{3}$")]
    private static partial Regex CurrencyCodeRegex();
}
