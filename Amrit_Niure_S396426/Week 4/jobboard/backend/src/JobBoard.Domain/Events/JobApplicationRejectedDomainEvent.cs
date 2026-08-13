using JobBoard.Domain.Common;

namespace JobBoard.Domain.Events;

public sealed record JobApplicationRejectedDomainEvent(Guid JobId, Guid ApplicationId) : IDomainEvent;
