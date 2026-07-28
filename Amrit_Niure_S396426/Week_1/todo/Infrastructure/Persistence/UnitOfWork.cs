using Domain.Common;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork(TodoDbContext context, IPublisher publisher) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Snapshot which tracked aggregates raised domain events before SaveChanges,
        // since ClearDomainEvents() below empties them and EF Core detaches nothing yet.
        var entitiesWithEvents = context.ChangeTracker
            .Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToList();

        var result = await context.SaveChangesAsync(cancellationToken);

        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
                await publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
