using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Common.Models;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.Admin.Queries;

public enum AdminTodoListSortBy
{
    CreatedOnUtc,
    Title
}

/// <summary>Admin-only oversight query: every todo list across every user. Access is gated
/// entirely at the endpoint (RequireAdmin policy) - the query itself never filters by owner.</summary>
public sealed record GetAllTodoListsQuery(
    int Page = 1,
    int PageSize = 20,
    string? TitleContains = null,
    string? OwnerEmailContains = null,
    AdminTodoListSortBy SortBy = AdminTodoListSortBy.CreatedOnUtc,
    bool SortDescending = false) : IRequest<Result<PagedResult<AdminTodoListResponse>>>;

public sealed class GetAllTodoListsQueryValidator : AbstractValidator<GetAllTodoListsQuery>
{
    public GetAllTodoListsQueryValidator()
    {
        RuleFor(q => q.Page).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).InclusiveBetween(1, 100);
        RuleFor(q => q.TitleContains).MaximumLength(200);
        RuleFor(q => q.OwnerEmailContains).MaximumLength(256);
    }
}

internal sealed class GetAllTodoListsQueryHandler(IApplicationDbContext context, IUserDirectory userDirectory)
    : IRequestHandler<GetAllTodoListsQuery, Result<PagedResult<AdminTodoListResponse>>>
{
    public async Task<Result<PagedResult<AdminTodoListResponse>>> Handle(GetAllTodoListsQuery request, CancellationToken cancellationToken)
    {
        var query = context.TodoLists.AsNoTracking();

        // OwnerId is a scalar value-converted column, so comparing it against a client-side
        // list of ids translates to SQL fine. Title lives on an owned type - see the same
        // note in GetTodoListsQueryHandler - so title filtering/sorting/paging happens in
        // memory, over the already-narrowed (by owner, if filtered) result set.
        if (!string.IsNullOrWhiteSpace(request.OwnerEmailContains))
        {
            var matchingOwnerIds = (await userDirectory.FindUserIdsByEmailAsync(request.OwnerEmailContains, cancellationToken))
                .Select(id => new UserId(id))
                .ToList();
            query = query.Where(l => matchingOwnerIds.Contains(l.OwnerId));
        }

        var lists = await query.ToListAsync(cancellationToken);

        IEnumerable<TodoList> filtered = lists;
        if (!string.IsNullOrWhiteSpace(request.TitleContains))
            filtered = filtered.Where(l => l.Title.Value.Contains(request.TitleContains, StringComparison.OrdinalIgnoreCase));

        IEnumerable<TodoList> sorted = (request.SortBy, request.SortDescending) switch
        {
            (AdminTodoListSortBy.Title, false) => filtered.OrderBy(l => l.Title.Value, StringComparer.OrdinalIgnoreCase),
            (AdminTodoListSortBy.Title, true) => filtered.OrderByDescending(l => l.Title.Value, StringComparer.OrdinalIgnoreCase),
            (AdminTodoListSortBy.CreatedOnUtc, true) => filtered.OrderByDescending(l => l.CreatedOnUtc),
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

        var ownerIds = pageOfLists.Select(l => l.OwnerId.Value).Distinct();
        var emailsByOwner = await userDirectory.GetEmailsByIdsAsync(ownerIds, cancellationToken);

        var items = pageOfLists
            .Select(l =>
            {
                countsByList.TryGetValue(l.Id, out var c);
                emailsByOwner.TryGetValue(l.OwnerId.Value, out var email);
                return new AdminTodoListResponse(
                    l.Id.Value, l.Title.Value, l.Colour.Code, l.CreatedOnUtc,
                    l.OwnerId.Value, email ?? "(unknown)", c?.Total ?? 0, c?.Completed ?? 0);
            })
            .ToList();

        return Result.Success(new PagedResult<AdminTodoListResponse>(items, request.Page, request.PageSize, totalCount));
    }
}
