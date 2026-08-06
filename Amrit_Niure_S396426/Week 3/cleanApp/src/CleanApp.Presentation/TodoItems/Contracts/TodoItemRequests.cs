namespace CleanApp.Presentation.TodoItems.Contracts;

public sealed record CreateTodoItemRequest(Guid TodoListId, string Title, int Priority, string? Note, DateTime? ReminderUtc);

public sealed record UpdateTodoItemRequest(string Title, int Priority, string? Note, DateTime? ReminderUtc);
