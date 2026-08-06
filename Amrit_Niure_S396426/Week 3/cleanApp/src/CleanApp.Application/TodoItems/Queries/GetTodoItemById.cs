using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.TodoItems.Queries;

public sealed record GetTodoItemByIdQuery(Guid TodoItemId) : IRequest<Result<TodoItemResponse>>;

internal sealed class GetTodoItemByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetTodoItemByIdQuery, Result<TodoItemResponse>>
{
    public async Task<Result<TodoItemResponse>> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var id = new TodoItemId(request.TodoItemId);
        var item = await context.TodoItems.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        if (item is null || item.OwnerId != currentUser.UserId)
            return Result.Failure<TodoItemResponse>(TodoItemErrors.NotFound(id));

        return Result.Success(new TodoItemResponse(
            item.Id.Value, item.TodoListId.Value, item.Title.Value, item.Note, item.Priority.Value, item.Priority.Name,
            item.ReminderUtc, item.IsDone, item.CreatedOnUtc, item.CompletedOnUtc));
    }
}
