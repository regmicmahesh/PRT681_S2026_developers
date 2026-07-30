using MediatR;

namespace Application.TodoItems.Commands.DeleteTodoItem;

public sealed record DeleteTodoItemCommand(Guid Id) : IRequest;
