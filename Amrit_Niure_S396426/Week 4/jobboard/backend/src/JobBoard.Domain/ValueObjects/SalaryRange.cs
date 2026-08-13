namespace JobBoard.Domain.ValueObjects;

public sealed record SalaryRange
{
    public decimal Min { get; }
    public decimal Max { get; }
    public string Currency { get; }

    public SalaryRange(decimal min, decimal max, string currency)
    {
        if (min < 0)
            throw new ArgumentException("Minimum salary cannot be negative.", nameof(min));

        if (max < min)
            throw new ArgumentException("Maximum salary cannot be less than minimum salary.", nameof(max));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.", nameof(currency));

        Min = min;
        Max = max;
        Currency = currency;
    }

    public override string ToString() => $"{Min:N0}-{Max:N0} {Currency}";
}
