using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class TodoItem : AggregateRoot
{
    public TodoTitle Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public Priority Priority { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // EF Core materializes entities without calling application code.
    private TodoItem()
    {
    }

    private TodoItem(TodoTitle title, string? description, Priority priority, DateTime? dueDate)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;

        AddDomainEvent(new TodoItemCreatedEvent(Id, Title.Value));
    }

    public static TodoItem Create(string title, string? description, Priority priority, DateTime? dueDate)
    {
        return new TodoItem(new TodoTitle(title), description, priority, dueDate);
    }

    public void UpdateDetails(string title, string? description, Priority priority, DateTime? dueDate)
    {
        if (IsCompleted)
            throw new DomainException("Cannot edit a todo item that is already completed. Reopen it first.");

        Title = new TodoTitle(title);
        Description = description;
        Priority = priority;
        DueDate = dueDate;
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("Todo item is already completed.");

        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;

        AddDomainEvent(new TodoItemCompletedEvent(Id, Title.Value));
    }

    public void Reopen()
    {
        if (!IsCompleted)
            throw new DomainException("Todo item is not completed.");

        IsCompleted = false;
        CompletedAt = null;
    }
}
