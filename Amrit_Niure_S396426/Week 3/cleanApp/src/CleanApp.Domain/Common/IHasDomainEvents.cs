namespace CleanApp.Domain.Common;

/// <summary>Non-generic marker so infrastructure code can find aggregates with pending domain events via the change tracker.</summary>
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
