using Domain.Enums;
using MediatR;

namespace Application.TodoItems.Commands.UpdateTodoItem;

public sealed record UpdateTodoItemCommand(
    Guid Id,
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDate) : IRequest;
