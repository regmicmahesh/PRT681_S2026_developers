using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoLists.Events;

public sealed record TodoListCreatedEvent(TodoListId TodoListId, string Title) : DomainEvent;
