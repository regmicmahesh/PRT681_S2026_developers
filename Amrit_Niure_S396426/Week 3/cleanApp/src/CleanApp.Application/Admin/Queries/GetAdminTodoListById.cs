using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.TodoItems;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.Admin.Queries;

/// <summary>Admin-only oversight query: any single list regardless of who owns it.</summary>
public sealed record GetAdminTodoListByIdQuery(Guid TodoListId) : IRequest<Result<AdminTodoListDetailResponse>>;

internal sealed class GetAdminTodoListByIdQueryHandler(IApplicationDbContext context, IUserDirectory userDirectory)
    : IRequestHandler<GetAdminTodoListByIdQuery, Result<AdminTodoListDetailResponse>>
{
    public async Task<Result<AdminTodoListDetailResponse>> Handle(GetAdminTodoListByIdQuery request, CancellationToken cancellationToken)
    {
        var id = new TodoListId(request.TodoListId);

        var list = await context.TodoLists.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        if (list is null)
            return Result.Failure<AdminTodoListDetailResponse>(TodoListErrors.NotFound(id));

        var items = await context.TodoItems.AsNoTracking()
            .Where(i => i.TodoListId == id)
            .ToListAsync(cancellationToken);

        var emails = await userDirectory.GetEmailsByIdsAsync([list.OwnerId.Value], cancellationToken);
        emails.TryGetValue(list.OwnerId.Value, out var ownerEmail);

        var orderedItems = items
            .OrderBy(i => i.IsDone)
            .ThenByDescending(i => i.Priority.Value)
            .ThenBy(i => i.CreatedOnUtc)
            .Select(MapToResponse)
            .ToList();

        return Result.Success(new AdminTodoListDetailResponse(
            list.Id.Value, list.Title.Value, list.Colour.Code, list.CreatedOnUtc,
            list.OwnerId.Value, ownerEmail ?? "(unknown)", orderedItems));
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
