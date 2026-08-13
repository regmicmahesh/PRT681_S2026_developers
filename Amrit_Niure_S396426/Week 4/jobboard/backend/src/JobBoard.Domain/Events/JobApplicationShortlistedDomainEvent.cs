using JobBoard.Domain.Common;

namespace JobBoard.Domain.Events;

public sealed record JobApplicationShortlistedDomainEvent(Guid JobId, Guid ApplicationId) : IDomainEvent;
