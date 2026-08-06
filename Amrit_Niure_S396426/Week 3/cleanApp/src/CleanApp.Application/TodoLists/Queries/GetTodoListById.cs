using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.TodoItems;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.TodoLists.Queries;

public sealed record GetTodoListByIdQuery(Guid TodoListId) : IRequest<Result<TodoListDetailResponse>>;

internal sealed class GetTodoListByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetTodoListByIdQuery, Result<TodoListDetailResponse>>
{
    public async Task<Result<TodoListDetailResponse>> Handle(GetTodoListByIdQuery request, CancellationToken cancellationToken)
    {
        var id = new TodoListId(request.TodoListId);

        var list = await context.TodoLists.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        if (list is null || list.OwnerId != currentUser.UserId)
            return Result.Failure<TodoListDetailResponse>(TodoListErrors.NotFound(id));

        var items = await context.TodoItems.AsNoTracking()
            .Where(i => i.TodoListId == id)
            .ToListAsync(cancellationToken);

        var orderedItems = items
            .OrderBy(i => i.IsDone)
            .ThenByDescending(i => i.Priority.Value)
            .ThenBy(i => i.CreatedOnUtc)
            .Select(MapToResponse)
            .ToList();

        return Result.Success(new TodoListDetailResponse(
            list.Id.Value, list.Title.Value, list.Colour.Code, list.CreatedOnUtc, orderedItems));
    }

    private static TodoItemResponse MapToResponse(TodoItem item) => new(
        item.Id.Value,
        item.TodoListId.Value,
        item.Title.Value,
        item.Note,
        item.Priority.Value,
        item.Priority.Name,
        item.ReminderUtc,
        item.IsDone,
        item.CreatedOnUtc,
        item.CompletedOnUtc);
}
