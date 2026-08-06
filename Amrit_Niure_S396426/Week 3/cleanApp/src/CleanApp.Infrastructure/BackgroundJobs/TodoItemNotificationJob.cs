using CleanApp.Application.Common.Interfaces;
using CleanApp.Infrastructure.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanApp.Infrastructure.BackgroundJobs;

internal sealed class TodoItemNotificationJob(
    IEmailSender emailSender,
    IOptions<EmailSettings> emailOptions,
    ILogger<TodoItemNotificationJob> logger) : ITodoItemNotificationJob
{
    public Task SendCompletionNotificationAsync(Guid todoItemId, string title, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending completion notification for todo item {TodoItemId}", todoItemId);

        return emailSender.SendAsync(
            emailOptions.Value.NotificationRecipient,
            $"Completed: {title}",
            $"<p>Todo item <strong>{title}</strong> was marked as completed.</p>",
            cancellationToken);
    }
}
