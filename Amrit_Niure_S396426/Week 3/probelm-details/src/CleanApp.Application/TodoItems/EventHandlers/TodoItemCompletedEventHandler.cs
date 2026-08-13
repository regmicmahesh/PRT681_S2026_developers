using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.TodoItems.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanApp.Application.TodoItems.EventHandlers;

/// <summary>When a todo item is completed, enqueues a background job to send a completion notification email.</summary>
public sealed class TodoItemCompletedEventHandler(
    IBackgroundJobService backgroundJobService,
    ILogger<TodoItemCompletedEventHandler> logger) : INotificationHandler<TodoItemCompletedEvent>
{
    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Todo item {TodoItemId} completed", notification.TodoItemId);

        var todoItemId = notification.TodoItemId.Value;
        var title = notification.Title;
        backgroundJobService.Enqueue<ITodoItemNotificationJob>(
            job => job.SendCompletionNotificationAsync(todoItemId, title, CancellationToken.None));

        return Task.CompletedTask;
    }
}
