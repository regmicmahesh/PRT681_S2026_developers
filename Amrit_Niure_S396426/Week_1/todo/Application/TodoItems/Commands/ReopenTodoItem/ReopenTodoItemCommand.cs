using MediatR;

namespace Application.TodoItems.Commands.ReopenTodoItem;

public sealed record ReopenTodoItemCommand(Guid Id) : IRequest;
