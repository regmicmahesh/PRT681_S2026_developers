using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;

namespace CleanApp.Application.Tests.TestSupport;

internal sealed class CurrentUserServiceStub(UserId userId) : ICurrentUserService
{
    public UserId UserId { get; } = userId;

    public bool IsAuthenticated => true;
}
