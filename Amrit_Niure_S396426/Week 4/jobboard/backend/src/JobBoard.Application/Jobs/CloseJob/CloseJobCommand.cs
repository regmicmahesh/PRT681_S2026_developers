using JobBoard.Application.Messaging;

namespace JobBoard.Application.Jobs.CloseJob;

public sealed record CloseJobCommand(Guid JobId) : ICommand;
