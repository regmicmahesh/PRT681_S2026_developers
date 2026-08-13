using JobBoard.Domain.Common;

namespace JobBoard.Domain.Events;

public sealed record JobApplicationSubmittedDomainEvent(Guid JobId, Guid ApplicationId, string CandidateName) : IDomainEvent;
