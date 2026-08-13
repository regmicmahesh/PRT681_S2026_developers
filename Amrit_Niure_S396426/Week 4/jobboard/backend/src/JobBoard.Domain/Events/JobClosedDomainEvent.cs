using JobBoard.Domain.Common;

namespace JobBoard.Domain.Events;

public sealed record JobClosedDomainEvent(Guid JobId) : IDomainEvent;
