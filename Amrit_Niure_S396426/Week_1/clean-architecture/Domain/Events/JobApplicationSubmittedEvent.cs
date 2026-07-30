using Domain.Common;

namespace Domain.Events;

public record JobApplicationSubmittedEvent(
    Guid ApplicationId,
    Guid JobPostId,
    Guid JobSeekerId,
    DateTime SubmittedAtUtc
) : IDomainEvent
{
    public DateTime OccurredOnUtc => SubmittedAtUtc;
}
