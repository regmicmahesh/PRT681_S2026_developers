using CleanApp.Application.Common.Interfaces;
using CleanApp.Application.Common.Models;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.TodoItems.Queries;

public enum TodoItemSortBy
{
    CreatedOnUtc,
    Title,
    Priority
}

public sealed record GetTodoItemsQuery(
    Guid TodoListId,
    bool? IsDone = null,
    int? Priority = null,
    string? TitleContains = null,
    int Page = 1,
    int PageSize = 20,
    TodoItemSortBy SortBy = TodoItemSortBy.CreatedOnUtc,
    bool SortDescending = false) : IRequest<Result<PagedResult<TodoItemResponse>>>;

public sealed class GetTodoItemsQueryValidator : AbstractValidator<GetTodoItemsQuery>
{
    public GetTodoItemsQueryValidator()
    {
        RuleFor(q => q.TodoListId).NotEmpty();
        RuleFor(q => q.Page).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).InclusiveBetween(1, 100);
        RuleFor(q => q.Priority).InclusiveBetween(0, 3).When(q => q.Priority is not null);
        RuleFor(q => q.TitleContains).MaximumLength(200);
    }
}

internal sealed class GetTodoItemsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetTodoItemsQuery, Result<PagedResult<TodoItemResponse>>>
{
    public async Task<Result<PagedResult<TodoItemResponse>>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var listId = new TodoListId(request.TodoListId);

        // IsDone/Priority/TodoListId/OwnerId are all scalar (or whole-value-converted-property)
        // comparisons, which translate to SQL fine. Title lives on an owned type - see the
        // same note in GetTodoListsQueryHandler - so title filtering/sorting/paging happens
        // in memory, over the already-narrowed set of this list's items.
        var query = context.TodoItems.AsNoTracking()
            .Where(i => i.TodoListId == listId && i.OwnerId == currentUser.UserId);

        if (request.IsDone is not null)
            query = query.Where(i => i.IsDone == request.IsDone);

        if (request.Priority is not null)
        {
            var priorityLevel = PriorityLevel.FromValueUnsafe(request.Priority.Value);
            query = query.Where(i => i.Priority == priorityLevel);
        }

        var matchingItems = await query.ToListAsync(cancellationToken);

        IEnumerable<TodoItem> filtered = matchingItems;
        if (!string.IsNullOrWhiteSpace(request.TitleContains))
            filtered = filtered.Where(i => i.Title.Value.Contains(request.TitleContains, StringComparison.OrdinalIgnoreCase));

        IEnumerable<TodoItem> sorted = (request.SortBy, request.SortDescending) switch
        {
            (TodoItemSortBy.Title, false) => filtered.OrderBy(i => i.Title.Value, StringComparer.OrdinalIgnoreCase),
            (TodoItemSortBy.Title, true) => filtered.OrderByDescending(i => i.Title.Value, StringComparer.OrdinalIgnoreCase),
            (TodoItemSortBy.Priority, false) => filtered.OrderBy(i => i.Priority.Value),
            (TodoItemSortBy.Priority, true) => filtered.OrderByDescending(i => i.Priority.Value),
            (TodoItemSortBy.CreatedOnUtc, true) => filtered.OrderByDescending(i => i.CreatedOnUtc),
            _ => filtered.OrderBy(i => i.CreatedOnUtc)
        };

        var sortedList = sorted.ToList();
        var totalCount = sortedList.Count;

        var items = sortedList
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(i => new TodoItemResponse(
                i.Id.Value, i.TodoListId.Value, i.Title.Value, i.Note, i.Priority.Value, i.Priority.Name,
                i.ReminderUtc, i.IsDone, i.CreatedOnUtc, i.CompletedOnUtc))
            .ToList();

        return Result.Success(new PagedResult<TodoItemResponse>(items, request.Page, request.PageSize, totalCount));
    }
}
