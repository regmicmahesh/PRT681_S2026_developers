using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.TodoItems.Events;

public sealed record TodoItemCreatedEvent(
    TodoItemId TodoItemId,
    TodoListId TodoListId,
    string Title,
    DateTime? ReminderUtc) : DomainEvent;
