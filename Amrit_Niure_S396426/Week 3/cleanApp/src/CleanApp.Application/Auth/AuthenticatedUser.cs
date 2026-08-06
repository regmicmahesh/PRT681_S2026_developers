using CleanApp.Domain.Common;

namespace CleanApp.Application.Auth;

public sealed record AuthenticatedUser(UserId UserId, IReadOnlyCollection<string> Roles);
