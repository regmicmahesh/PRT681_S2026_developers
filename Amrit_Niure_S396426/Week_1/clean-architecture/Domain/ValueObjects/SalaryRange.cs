using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public enum SalaryPeriod
{
    Hourly = 1,
    Monthly = 2,
    Yearly = 3
}

public class SalaryRange : ValueObject
{
    public decimal Minimum { get; }
    public decimal Maximum { get; }
    public string Currency { get; }
    public SalaryPeriod Period { get; }

    public SalaryRange(decimal minimum, decimal maximum, string currency = "USD", SalaryPeriod period = SalaryPeriod.Yearly)
    {
        if (minimum < 0)
            throw new DomainException("Minimum salary cannot be negative.");

        if (maximum < minimum)
            throw new DomainException("Maximum salary cannot be less than minimum salary.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        Minimum = minimum;
        Maximum = maximum;
        Currency = currency.ToUpperInvariant();
        Period = period;
    }

    public bool Includes(decimal amount) => amount >= Minimum && amount <= Maximum;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Minimum;
        yield return Maximum;
        yield return Currency;
        yield return Period;
    }
}
