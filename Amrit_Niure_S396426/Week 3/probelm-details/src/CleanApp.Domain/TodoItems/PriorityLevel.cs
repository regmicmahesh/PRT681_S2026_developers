using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoItems;

/// <summary>Smart-enum value object (not a plain C# enum) so priority carries behaviour and validation.</summary>
public sealed class PriorityLevel : ValueObject
{
    public static readonly PriorityLevel None = new(0, nameof(None));
    public static readonly PriorityLevel Low = new(1, nameof(Low));
    public static readonly PriorityLevel Medium = new(2, nameof(Medium));
    public static readonly PriorityLevel High = new(3, nameof(High));

    private static readonly PriorityLevel[] All = [None, Low, Medium, High];

    private PriorityLevel(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public int Value { get; }

    public string Name { get; }

    public static Result<PriorityLevel> FromValue(int value)
    {
        var match = All.FirstOrDefault(p => p.Value == value);
        return match is not null
            ? Result.Success(match)
            : Result.Failure<PriorityLevel>(TodoItemErrors.InvalidPriority);
    }

    /// <summary>Reconstructs a known-valid instance, e.g. when materializing from the database.</summary>
    public static PriorityLevel FromValueUnsafe(int value) => All.First(p => p.Value == value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Name;
}
