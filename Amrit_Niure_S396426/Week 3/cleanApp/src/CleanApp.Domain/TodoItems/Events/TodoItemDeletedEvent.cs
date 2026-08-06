using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.TodoItems.Events;

public sealed record TodoItemDeletedEvent(TodoItemId TodoItemId, TodoListId TodoListId) : DomainEvent;
