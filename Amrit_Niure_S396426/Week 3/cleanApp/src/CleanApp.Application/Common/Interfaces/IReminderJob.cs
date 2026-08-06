namespace CleanApp.Application.Common.Interfaces;

/// <summary>Executed by the background job runner at the scheduled reminder time.</summary>
public interface IReminderJob
{
    Task SendReminderAsync(Guid todoItemId, CancellationToken cancellationToken);
}
