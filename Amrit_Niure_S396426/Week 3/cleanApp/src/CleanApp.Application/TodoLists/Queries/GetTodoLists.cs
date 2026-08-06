using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Common.Models;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.TodoLists.Queries;

public enum TodoListSortBy
{
    CreatedOnUtc,
    Title
}

public sealed record GetTodoListsQuery(
    int Page = 1,
    int PageSize = 20,
    string? TitleContains = null,
    TodoListSortBy SortBy = TodoListSortBy.CreatedOnUtc,
    bool SortDescending = false) : IRequest<Result<PagedResult<TodoListResponse>>>;

public sealed class GetTodoListsQueryValidator : AbstractValidator<GetTodoListsQuery>
{
    public GetTodoListsQueryValidator()
    {
        RuleFor(q => q.Page).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).InclusiveBetween(1, 100);
        RuleFor(q => q.TitleContains).MaximumLength(200);
    }
}

internal sealed class GetTodoListsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetTodoListsQuery, Result<PagedResult<TodoListResponse>>>
{
    public async Task<Result<PagedResult<TodoListResponse>>> Handle(GetTodoListsQuery request, CancellationToken cancellationToken)
    {
        // OwnerId is a scalar value-converted column, so this scoping filter translates to
        // SQL fine. Title lives on an owned type, and EF's translation of member access into
        // an owned type inside Where/OrderBy is unreliable across providers - so title
        // filtering/sorting/paging happens in memory, over just this user's own lists.
        var lists = await context.TodoLists.AsNoTracking()
            .Where(l => l.OwnerId == currentUser.UserId)
            .ToListAsync(cancellationToken);

        IEnumerable<TodoList> filtered = lists;
        if (!string.IsNullOrWhiteSpace(request.TitleContains))
            filtered = filtered.Where(l => l.Title.Value.Contains(request.TitleContains, StringComparison.OrdinalIgnoreCase));

        IEnumerable<TodoList> sorted = (request.SortBy, request.SortDescending) switch
        {
            (TodoListSortBy.Title, false) => filtered.OrderBy(l => l.Title.Value, StringComparer.OrdinalIgnoreCase),
            (TodoListSortBy.Title, true) => filtered.OrderByDescending(l => l.Title.Value, StringComparer.OrdinalIgnoreCase),
            (TodoListSortBy.CreatedOnUtc, true) => filtered.OrderByDescending(l => l.CreatedOnUtc),
            _ => filtered.OrderBy(l => l.CreatedOnUtc)
        };

        var sortedList = sorted.ToList();
        var totalCount = sortedList.Count;

        var pageOfLists = sortedList
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var listIds = pageOfLists.Select(l => l.Id).ToList();
        var counts = await context.TodoItems.AsNoTracking()
            .Where(i => listIds.Contains(i.TodoListId))
            .GroupBy(i => i.TodoListId)
            .Select(g => new { ListId = g.Key, Total = g.Count(), Completed = g.Count(i => i.IsDone) })
            .ToListAsync(cancellationToken);
        var countsByList = counts.ToDictionary(c => c.ListId);

        var items = pageOfLists
            .Select(l =>
            {
                countsByList.TryGetValue(l.Id, out var c);
                return new TodoListResponse(l.Id.Value, l.Title.Value, l.Colour.Code, l.CreatedOnUtc, c?.Total ?? 0, c?.Completed ?? 0);
            })
            .ToList();

        return Result.Success(new PagedResult<TodoListResponse>(items, request.Page, request.PageSize, totalCount));
    }
}
