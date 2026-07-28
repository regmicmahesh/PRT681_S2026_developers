namespace Domain.Common;

/// <summary>
/// Represents a domain event that occurs within the domain model.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
