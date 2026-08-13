using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.TodoItems.Queries;

public sealed record GetTodoItemsQuery(Guid TodoListId, bool? IsDone = null) : IRequest<Result<List<TodoItemResponse>>>;

internal sealed class GetTodoItemsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTodoItemsQuery, Result<List<TodoItemResponse>>>
{
    public async Task<Result<List<TodoItemResponse>>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var listId = new TodoListId(request.TodoListId);

        var query = context.TodoItems.AsNoTracking().Where(i => i.TodoListId == listId);
        if (request.IsDone is not null)
            query = query.Where(i => i.IsDone == request.IsDone);

        var items = await query.ToListAsync(cancellationToken);

        var response = items
            .OrderBy(i => i.IsDone)
            .ThenByDescending(i => i.Priority.Value)
            .ThenBy(i => i.CreatedOnUtc)
            .Select(i => new TodoItemResponse(
                i.Id.Value, i.TodoListId.Value, i.Title.Value, i.Note, i.Priority.Value, i.Priority.Name,
                i.ReminderUtc, i.IsDone, i.CreatedOnUtc, i.CompletedOnUtc))
            .ToList();

        return Result.Success(response);
    }
}
