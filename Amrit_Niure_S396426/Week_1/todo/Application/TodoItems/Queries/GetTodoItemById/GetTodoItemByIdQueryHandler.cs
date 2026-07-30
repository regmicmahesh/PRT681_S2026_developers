using Application.TodoItems.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItemById;

public sealed class GetTodoItemByIdQueryHandler(ITodoItemRepository repository)
    : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto>
{
    public async Task<TodoItemDto> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(TodoItem), request.Id);

        return TodoItemDto.FromEntity(todoItem);
    }
}
