using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.TodoLists.Queries;

public sealed record GetTodoListsQuery : IRequest<Result<List<TodoListResponse>>>;

internal sealed class GetTodoListsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTodoListsQuery, Result<List<TodoListResponse>>>
{
    public async Task<Result<List<TodoListResponse>>> Handle(GetTodoListsQuery request, CancellationToken cancellationToken)
    {
        var lists = await context.TodoLists.AsNoTracking()
            .OrderBy(l => l.CreatedOnUtc)
            .ToListAsync(cancellationToken);

        var counts = await context.TodoItems.AsNoTracking()
            .GroupBy(i => i.TodoListId)
            .Select(g => new { ListId = g.Key, Total = g.Count(), Completed = g.Count(i => i.IsDone) })
            .ToListAsync(cancellationToken);

        var countsByList = counts.ToDictionary(c => c.ListId);

        var response = lists
            .Select(l =>
            {
                countsByList.TryGetValue(l.Id, out var c);
                return new TodoListResponse(l.Id.Value, l.Title.Value, l.Colour.Code, l.CreatedOnUtc, c?.Total ?? 0, c?.Completed ?? 0);
            })
            .ToList();

        return Result.Success(response);
    }
}
