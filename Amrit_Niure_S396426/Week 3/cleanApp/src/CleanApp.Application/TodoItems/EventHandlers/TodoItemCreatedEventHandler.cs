using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.TodoItems.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanApp.Application.TodoItems.EventHandlers;

/// <summary>When a todo item is created with a reminder, schedules a background job to fire at that time.</summary>
public sealed class TodoItemCreatedEventHandler(
    IBackgroundJobService backgroundJobService,
    IDateTimeProvider dateTimeProvider,
    ILogger<TodoItemCreatedEventHandler> logger) : INotificationHandler<TodoItemCreatedEvent>
{
    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Todo item {TodoItemId} created in list {TodoListId}", notification.TodoItemId, notification.TodoListId);

        if (notification.ReminderUtc is not null)
        {
            var delay = notification.ReminderUtc.Value - dateTimeProvider.UtcNow;
            if (delay < TimeSpan.Zero)
                delay = TimeSpan.Zero;

            var todoItemId = notification.TodoItemId.Value;
            backgroundJobService.Schedule<IReminderJob>(job => job.SendReminderAsync(todoItemId, CancellationToken.None), delay);
        }

        return Task.CompletedTask;
    }
}
