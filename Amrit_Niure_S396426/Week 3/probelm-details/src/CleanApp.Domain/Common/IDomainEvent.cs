using MediatR;

namespace CleanApp.Domain.Common;

/// <summary>
/// Marker for domain events. Extends MediatR's INotification (via the lightweight
/// MediatR.Contracts package) so events raised inside aggregates can be published
/// through the same MediatR pipeline the Application layer already uses, without
/// pulling the full MediatR implementation into the Domain layer.
/// </summary>
public interface IDomainEvent : INotification
{
    DateTime OccurredOnUtc { get; }
}
