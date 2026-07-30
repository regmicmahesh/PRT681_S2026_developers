using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

/// <summary>
/// Wraps a raw string so the "a title must be 1-200 characters" invariant can never be
/// bypassed — any TodoTitle instance in memory is guaranteed valid by construction.
/// </summary>
public sealed class TodoTitle : ValueObject
{
    public const int MaxLength = 200;

    public string Value { get; }

    public TodoTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Todo title cannot be empty.");

        if (value.Length > MaxLength)
            throw new DomainException($"Todo title cannot exceed {MaxLength} characters.");

        Value = value.Trim();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
