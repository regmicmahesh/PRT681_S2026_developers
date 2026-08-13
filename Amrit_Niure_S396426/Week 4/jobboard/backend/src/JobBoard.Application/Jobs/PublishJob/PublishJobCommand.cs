using JobBoard.Application.Messaging;

namespace JobBoard.Application.Jobs.PublishJob;

public sealed record PublishJobCommand(Guid JobId) : ICommand;
