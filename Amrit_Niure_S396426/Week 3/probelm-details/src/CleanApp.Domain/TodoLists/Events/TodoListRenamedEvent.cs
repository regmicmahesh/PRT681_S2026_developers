using CleanApp.Domain.Common;

namespace CleanApp.Domain.TodoLists.Events;

public sealed record TodoListRenamedEvent(TodoListId TodoListId, string NewTitle) : DomainEvent;
