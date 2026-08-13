using JobBoard.Application.Messaging;

namespace JobBoard.Application.Jobs.ApplyToJob;

public sealed record ApplyToJobCommand(
    Guid JobId,
    string CandidateName,
    string CandidateEmail,
    string ResumeUrl) : ICommand<Guid>;
