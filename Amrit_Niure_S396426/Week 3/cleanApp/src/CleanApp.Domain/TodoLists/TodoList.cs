using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists.Events;

namespace CleanApp.Domain.TodoLists;

public sealed class TodoList : AggregateRoot<TodoListId>
{
    private TodoList()
    {
        // Required by EF Core.
    }

    private TodoList(TodoListId id, UserId ownerId, TodoListTitle title, Colour colour) : base(id)
    {
        OwnerId = ownerId;
        Title = title;
        Colour = colour;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public UserId OwnerId { get; private set; }

    public TodoListTitle Title { get; private set; } = null!;

    public Colour Colour { get; private set; } = null!;

    public DateTime CreatedOnUtc { get; private set; }

    public static Result<TodoList> Create(UserId ownerId, string title, string? colourCode = null)
    {
        var titleResult = TodoListTitle.Create(title);
        if (titleResult.IsFailure)
            return Result.Failure<TodoList>(titleResult.Error);

        var colourResult = colourCode is null
            ? Result.Success(Colour.White)
            : Colour.Create(colourCode);
        if (colourResult.IsFailure)
            return Result.Failure<TodoList>(colourResult.Error);

        var list = new TodoList(TodoListId.New(), ownerId, titleResult.Value, colourResult.Value);
        list.RaiseDomainEvent(new TodoListCreatedEvent(list.Id, list.Title.Value));

        return Result.Success(list);
    }

    public Result Rename(string title)
    {
        var titleResult = TodoListTitle.Create(title);
        if (titleResult.IsFailure)
            return Result.Failure(titleResult.Error);

        if (titleResult.Value == Title)
            return Result.Success();

        Title = titleResult.Value;
        RaiseDomainEvent(new TodoListRenamedEvent(Id, Title.Value));

        return Result.Success();
    }

    public Result ChangeColour(string colourCode)
    {
        var colourResult = Colour.Create(colourCode);
        if (colourResult.IsFailure)
            return Result.Failure(colourResult.Error);

        Colour = colourResult.Value;
        return Result.Success();
    }
}
