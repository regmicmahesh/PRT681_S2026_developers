using CleanApp.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanApp.Persistence.Interceptors;

/// <summary>
/// After a successful SaveChanges, publishes every domain event raised by tracked
/// aggregates through MediatR, then clears them. Runs post-commit so a failure in an
/// event handler can never roll back the write that raised the event.
/// </summary>
public sealed class DispatchDomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            await DispatchDomainEvents(eventData.Context, cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext context, CancellationToken cancellationToken)
    {
        var entitiesWithEvents = context.ChangeTracker.Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Count > 0)
            .ToList();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
                await publisher.Publish(domainEvent, cancellationToken);
        }
    }
}
