using CleanApp.Domain.Common;

namespace CleanApp.Application.Common.Interfaces;

/// <summary>Resolves the authenticated caller from the current request (JWT claims). Never
/// trust a user id supplied by the client in a request body - always read it from here.</summary>
public interface ICurrentUserService
{
    UserId UserId { get; }

    bool IsAuthenticated { get; }
}
