namespace CleanApp.Domain.TodoItems;

public readonly record struct TodoItemId(Guid Value)
{
    public static TodoItemId New() => new(Guid.NewGuid());

    public static readonly TodoItemId Empty = new(Guid.Empty);

    public override string ToString() => Value.ToString();
}
