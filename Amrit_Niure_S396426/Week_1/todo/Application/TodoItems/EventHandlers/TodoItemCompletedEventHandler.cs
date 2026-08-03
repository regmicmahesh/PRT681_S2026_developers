using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.TodoItems.EventHandlers;

public sealed class TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    : INotificationHandler<TodoItemCompletedEvent>
{
    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Todo item {TodoItemId} \"{Title}\" was completed at {OccurredOn}",
            notification.TodoItemId, notification.Title, notification.OccurredOn);

        return Task.CompletedTask;
    }
}
