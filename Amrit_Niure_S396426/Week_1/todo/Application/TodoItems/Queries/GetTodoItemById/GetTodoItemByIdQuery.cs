using Application.TodoItems.Dtos;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItemById;

public sealed record GetTodoItemByIdQuery(Guid Id) : IRequest<TodoItemDto>;
