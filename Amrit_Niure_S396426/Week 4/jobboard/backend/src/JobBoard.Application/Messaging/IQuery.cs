using JobBoard.Domain.Common;
using MediatR;

namespace JobBoard.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
