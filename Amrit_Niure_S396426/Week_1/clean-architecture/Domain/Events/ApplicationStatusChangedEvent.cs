using Domain.Common;
using Domain.Enums;

namespace Domain.Events;

public record ApplicationStatusChangedEvent(
    Guid ApplicationId,
    ApplicationStatus PreviousStatus,
    ApplicationStatus NewStatus,
    DateTime UpdatedAtUtc
) : IDomainEvent
{
    public DateTime OccurredOnUtc => UpdatedAtUtc;
}
