using Application.TodoItems.Dtos;
using Domain.Repositories;
using MediatR;

namespace Application.TodoItems.Queries.GetTodoItems;

public sealed class GetTodoItemsQueryHandler(ITodoItemRepository repository)
    : IRequestHandler<GetTodoItemsQuery, List<TodoItemDto>>
{
    public async Task<List<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var todoItems = await repository.GetAllAsync(request.IsCompleted, cancellationToken);

        return todoItems.Select(TodoItemDto.FromEntity).ToList();
    }
}
