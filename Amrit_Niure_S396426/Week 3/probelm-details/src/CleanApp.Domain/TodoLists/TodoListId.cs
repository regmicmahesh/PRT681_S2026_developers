namespace CleanApp.Domain.TodoLists;

public readonly record struct TodoListId(Guid Value)
{
    public static TodoListId New() => new(Guid.NewGuid());

    public static readonly TodoListId Empty = new(Guid.Empty);

    public override string ToString() => Value.ToString();
}
