using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoLists;

public sealed class TodoListTitle : ValueObject
{
    public const int MaxLength = 200;

    private TodoListTitle(string value) => Value = value;

    public string Value { get; }

    public static Result<TodoListTitle> Create(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<TodoListTitle>(TodoListErrors.TitleEmpty);

        var trimmed = title.Trim();
        if (trimmed.Length > MaxLength)
            return Result.Failure<TodoListTitle>(TodoListErrors.TitleTooLong);

        return Result.Success(new TodoListTitle(trimmed));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
