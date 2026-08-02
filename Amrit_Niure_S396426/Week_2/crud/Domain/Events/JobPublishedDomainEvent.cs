using Domain.Common;

namespace Domain.Events;

public sealed record JobPublishedDomainEvent(Guid JobId) : IDomainEvent;