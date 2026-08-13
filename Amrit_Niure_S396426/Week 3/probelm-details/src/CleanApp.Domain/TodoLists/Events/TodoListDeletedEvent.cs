using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoLists.Events;

public sealed record TodoListDeletedEvent(TodoListId TodoListId) : DomainEvent;
