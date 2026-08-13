using JobBoard.Domain.Common;

namespace JobBoard.Domain.Events;

public sealed record JobPublishedDomainEvent(Guid JobId) : IDomainEvent;
