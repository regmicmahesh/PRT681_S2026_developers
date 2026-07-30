using MediatR;

namespace Application.TodoItems.Commands.CompleteTodoItem;

public sealed record CompleteTodoItemCommand(Guid Id) : IRequest;
