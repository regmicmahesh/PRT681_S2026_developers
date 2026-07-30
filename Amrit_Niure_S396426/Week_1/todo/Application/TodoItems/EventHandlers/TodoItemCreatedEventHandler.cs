using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.TodoItems.EventHandlers;

/// <summary>
/// Reacts to a domain event as a side effect, decoupled from the command that raised it —
/// CreateTodoItemCommandHandler has no idea this handler exists.
/// </summary>
public sealed class TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    : INotificationHandler<TodoItemCreatedEvent>
{
    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Todo item {TodoItemId} \"{Title}\" was created at {OccurredOn}",
            notification.TodoItemId, notification.Title, notification.OccurredOn);

        return Task.CompletedTask;
    }
}
