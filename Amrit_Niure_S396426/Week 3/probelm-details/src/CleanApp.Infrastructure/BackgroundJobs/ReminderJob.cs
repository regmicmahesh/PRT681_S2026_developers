using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.TodoItems;
using CleanApp.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanApp.Infrastructure.BackgroundJobs;

internal sealed class ReminderJob(
    IApplicationDbContext context,
    IEmailSender emailSender,
    IOptions<EmailSettings> emailOptions,
    ILogger<ReminderJob> logger) : IReminderJob
{
    public async Task SendReminderAsync(Guid todoItemId, CancellationToken cancellationToken)
    {
        var id = new TodoItemId(todoItemId);
        var item = await context.TodoItems.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        if (item is null || item.IsDone)
        {
            logger.LogInformation(
                "Skipping reminder for todo item {TodoItemId}: not found or already done", todoItemId);
            return;
        }

        await emailSender.SendAsync(
            emailOptions.Value.NotificationRecipient,
            $"Reminder: {item.Title.Value}",
            $"<p>Your todo item <strong>{item.Title.Value}</strong> is due.</p>",
            cancellationToken);
    }
}
