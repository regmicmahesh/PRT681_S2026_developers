using Domain.Common;

namespace Domain.Events;

public sealed record JobApplicationSubmittedDomainEvent(Guid JobId, Guid ApplicationID, string CandidateName) : IDomainEvent;