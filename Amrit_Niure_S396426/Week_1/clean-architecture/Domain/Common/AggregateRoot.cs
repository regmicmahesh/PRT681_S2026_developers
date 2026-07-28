namespace Domain.Common;

/// <summary>
/// Abstract base class for Aggregate Roots in Domain Driven Design.
/// </summary>
/// <typeparam name="TId">The type of the aggregate root primary key identifier.</typeparam>
public abstract class AggregateRoot<TId> : BaseEntity<TId>
{
    protected AggregateRoot() { }

    protected AggregateRoot(TId id) : base(id) { }
}
