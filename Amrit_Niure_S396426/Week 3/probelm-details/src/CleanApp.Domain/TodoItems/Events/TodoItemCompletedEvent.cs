using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.TodoItems.Events;

public sealed record TodoItemCompletedEvent(
    TodoItemId TodoItemId,
    TodoListId TodoListId,
    string Title) : DomainEvent;
