namespace Domain.Common;

/// <summary>
/// Marks an entity as the single entry point of a consistency boundary (an "aggregate").
/// Repositories only ever load/save aggregate roots, never the entities inside them directly.
/// </summary>
public abstract class AggregateRoot : BaseEntity
{
}
