using JobBoard.Domain.Enums;

namespace JobBoard.Domain.ValueObjects;

public sealed record SalaryRange
{
    public decimal Min { get; }
    public decimal Max { get; }
    public Currency Currency { get; }
    public PayPeriod PayPeriod { get; }

    public SalaryRange(decimal min, decimal max, Currency currency, PayPeriod payPeriod)
    {
        if (min < 0)
            throw new ArgumentException("Minimum salary cannot be negative.", nameof(min));

        if (max < min)
            throw new ArgumentException("Maximum salary cannot be less than minimum salary.", nameof(max));

        Min = min;
        Max = max;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        PayPeriod = payPeriod;
    }

    public override string ToString() => $"{Min:N0}-{Max:N0} {Currency} / {PayPeriod}";
}
