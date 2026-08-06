namespace CleanApp.Domain.Common;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
