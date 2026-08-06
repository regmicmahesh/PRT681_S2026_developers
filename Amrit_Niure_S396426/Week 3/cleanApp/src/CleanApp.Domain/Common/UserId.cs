namespace CleanApp.Domain.Common;

/// <summary>
/// Opaque reference to the authenticated user who owns an aggregate. The Domain only ever
/// tags data with this id for ownership checks - it knows nothing about how users are
/// authenticated or stored (that's Infrastructure/Persistence's job).
/// </summary>
public readonly record struct UserId(Guid Value)
{
    public static readonly UserId Empty = new(Guid.Empty);

    public override string ToString() => Value.ToString();
}
