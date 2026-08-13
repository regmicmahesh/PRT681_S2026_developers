using JobBoard.Domain.Common;
using MediatR;

namespace JobBoard.Application.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
