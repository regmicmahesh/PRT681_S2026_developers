using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoLists.Commands;

public sealed record DeleteTodoListCommand(Guid TodoListId) : IRequest<Result>;

public sealed class DeleteTodoListCommandValidator : AbstractValidator<DeleteTodoListCommand>
{
    public DeleteTodoListCommandValidator() => RuleFor(c => c.TodoListId).NotEmpty();
}

internal sealed class DeleteTodoListCommandHandler(
    ITodoListRepository listRepository,
    ITodoItemRepository itemRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<DeleteTodoListCommand, Result>
{
    public async Task<Result> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var id = new TodoListId(request.TodoListId);
        var list = await listRepository.GetByIdAsync(id, cancellationToken);
        if (list is null || list.OwnerId != currentUser.UserId)
            return Result.Failure(TodoListErrors.NotFound(id));

        if (await itemRepository.AnyForListAsync(id, cancellationToken))
            return Result.Failure(TodoListErrors.CannotDeleteNonEmpty);

        listRepository.Remove(list);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
