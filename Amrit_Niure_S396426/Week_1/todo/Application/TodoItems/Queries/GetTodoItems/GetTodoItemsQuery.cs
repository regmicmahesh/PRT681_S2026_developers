using Application.TodoItems.Dtos;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItems;

public sealed record GetTodoItemsQuery(bool? IsCompleted) : IRequest<List<TodoItemDto>>;
