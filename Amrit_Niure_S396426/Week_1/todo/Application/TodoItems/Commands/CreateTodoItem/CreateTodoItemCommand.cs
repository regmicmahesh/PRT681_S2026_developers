using Domain.Enums;
using MediatR;

namespace Application.TodoItems.Commands.CreateTodoItem;

public sealed record CreateTodoItemCommand(
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDate) : IRequest<Guid>;
