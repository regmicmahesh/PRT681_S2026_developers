using Domain.Common;

namespace Domain.Events;

public sealed class TodoItemCompletedEvent(Guid todoItemId, string title) : IDomainEvent
{
    public Guid TodoItemId { get; } = todoItemId;
    public string Title { get; } = title;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
