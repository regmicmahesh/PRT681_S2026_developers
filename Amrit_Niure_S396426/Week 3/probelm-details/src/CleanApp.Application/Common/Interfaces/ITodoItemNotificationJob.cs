namespace CleanApp.Application.Common.Interfaces;

/// <summary>Executed by the background job runner to notify about a completed todo item.</summary>
public interface ITodoItemNotificationJob
{
    Task SendCompletionNotificationAsync(Guid todoItemId, string title, CancellationToken cancellationToken);
}
