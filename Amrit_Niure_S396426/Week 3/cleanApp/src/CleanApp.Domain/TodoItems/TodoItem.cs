using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems.Events;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.TodoItems;

public sealed class TodoItem : AggregateRoot<TodoItemId>
{
    private TodoItem()
    {
        // Required by EF Core.
    }

    private TodoItem(TodoItemId id, UserId ownerId, TodoListId todoListId, TodoItemTitle title, PriorityLevel priority, DateTime? reminderUtc)
        : base(id)
    {
        OwnerId = ownerId;
        TodoListId = todoListId;
        Title = title;
        Priority = priority;
        ReminderUtc = reminderUtc;
        IsDone = false;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public UserId OwnerId { get; private set; }

    public TodoListId TodoListId { get; private set; }

    public TodoItemTitle Title { get; private set; } = null!;

    public string? Note { get; private set; }

    public PriorityLevel Priority { get; private set; } = null!;

    public DateTime? ReminderUtc { get; private set; }

    public bool IsDone { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? CompletedOnUtc { get; private set; }

    public static Result<TodoItem> Create(
        UserId ownerId,
        TodoListId todoListId,
        string title,
        int priority = 0,
        DateTime? reminderUtc = null,
        string? note = null)
    {
        var titleResult = TodoItemTitle.Create(title);
        if (titleResult.IsFailure)
            return Result.Failure<TodoItem>(titleResult.Error);

        var priorityResult = PriorityLevel.FromValue(priority);
        if (priorityResult.IsFailure)
            return Result.Failure<TodoItem>(priorityResult.Error);

        if (reminderUtc is not null && reminderUtc <= DateTime.UtcNow)
            return Result.Failure<TodoItem>(TodoItemErrors.ReminderInPast);

        var item = new TodoItem(TodoItemId.New(), ownerId, todoListId, titleResult.Value, priorityResult.Value, reminderUtc)
        {
            Note = note
        };
        item.RaiseDomainEvent(new TodoItemCreatedEvent(item.Id, item.TodoListId, item.Title.Value, item.ReminderUtc));

        return Result.Success(item);
    }

    public Result UpdateDetails(string title, int priority, string? note, DateTime? reminderUtc)
    {
        var titleResult = TodoItemTitle.Create(title);
        if (titleResult.IsFailure)
            return Result.Failure(titleResult.Error);

        var priorityResult = PriorityLevel.FromValue(priority);
        if (priorityResult.IsFailure)
            return Result.Failure(priorityResult.Error);

        if (reminderUtc is not null && reminderUtc <= DateTime.UtcNow)
            return Result.Failure(TodoItemErrors.ReminderInPast);

        Title = titleResult.Value;
        Priority = priorityResult.Value;
        Note = note;
        ReminderUtc = reminderUtc;

        return Result.Success();
    }

    public Result Complete()
    {
        if (IsDone)
            return Result.Failure(TodoItemErrors.AlreadyCompleted);

        IsDone = true;
        CompletedOnUtc = DateTime.UtcNow;
        RaiseDomainEvent(new TodoItemCompletedEvent(Id, TodoListId, Title.Value));

        return Result.Success();
    }

    public Result Reopen()
    {
        if (!IsDone)
            return Result.Failure(TodoItemErrors.NotCompleted);

        IsDone = false;
        CompletedOnUtc = null;
        RaiseDomainEvent(new TodoItemReopenedEvent(Id, TodoListId));

        return Result.Success();
    }
}
