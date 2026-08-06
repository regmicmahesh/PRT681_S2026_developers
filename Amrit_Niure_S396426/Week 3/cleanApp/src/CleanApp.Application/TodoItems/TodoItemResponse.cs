namespace CleanApp.Application.TodoItems;

public sealed record TodoItemResponse(
    Guid Id,
    Guid TodoListId,
    string Title,
    string? Note,
    int Priority,
    string PriorityName,
    DateTime? ReminderUtc,
    bool IsDone,
    DateTime CreatedOnUtc,
    DateTime? CompletedOnUtc);
