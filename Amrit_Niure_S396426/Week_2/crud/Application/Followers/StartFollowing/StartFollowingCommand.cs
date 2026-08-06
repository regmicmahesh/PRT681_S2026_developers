using Application.Messaging;
using Domain.Common;
namespace Application.Followers.StartFollowing;

public sealed record StartFollowingCommand(Guid userId, Guid FollowedId) : ICommand;

internal sealed class StartFollowingCommandHandler : ICommandHandler<StartFollowingCommand>
{
    public Task<Result> Handle(StartFollowingCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}