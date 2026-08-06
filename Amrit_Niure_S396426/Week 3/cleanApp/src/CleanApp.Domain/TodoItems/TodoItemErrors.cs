using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoItems;

public static class TodoItemErrors
{
    public static readonly Error TitleEmpty =
        Error.Validation("TodoItem.TitleEmpty", "Title cannot be empty.");

    public static readonly Error TitleTooLong =
        Error.Validation("TodoItem.TitleTooLong", $"Title cannot exceed {TodoItemTitle.MaxLength} characters.");

    public static readonly Error InvalidPriority =
        Error.Validation("TodoItem.InvalidPriority", "Priority must be one of: 0 (None), 1 (Low), 2 (Medium), 3 (High).");

    public static readonly Error ReminderInPast =
        Error.Validation("TodoItem.ReminderInPast", "Reminder must be set in the future.");

    public static readonly Error AlreadyCompleted =
        Error.Conflict("TodoItem.AlreadyCompleted", "Todo item is already completed.");

    public static readonly Error NotCompleted =
        Error.Conflict("TodoItem.NotCompleted", "Todo item has not been completed yet.");

    public static Error NotFound(TodoItemId id) =>
        Error.NotFound("TodoItem.NotFound", $"Todo item with id '{id}' was not found.");
}
