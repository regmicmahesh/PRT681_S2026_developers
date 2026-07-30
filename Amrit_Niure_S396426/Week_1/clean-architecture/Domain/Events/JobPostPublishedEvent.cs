using Domain.Common;

namespace Domain.Events;

public record JobPostPublishedEvent(
    Guid JobPostId,
    Guid CompanyId,
    string Title,
    DateTime PublishedAtUtc
) : IDomainEvent
{
    public DateTime OccurredOnUtc => PublishedAtUtc;
}
