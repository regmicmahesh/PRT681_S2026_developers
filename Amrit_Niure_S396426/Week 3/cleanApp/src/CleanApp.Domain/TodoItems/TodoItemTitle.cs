using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoItems;

public sealed class TodoItemTitle : ValueObject
{
    public const int MaxLength = 200;

    private TodoItemTitle(string value) => Value = value;

    public string Value { get; }

    public static Result<TodoItemTitle> Create(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<TodoItemTitle>(TodoItemErrors.TitleEmpty);

        var trimmed = title.Trim();
        if (trimmed.Length > MaxLength)
            return Result.Failure<TodoItemTitle>(TodoItemErrors.TitleTooLong);

        return Result.Success(new TodoItemTitle(trimmed));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
