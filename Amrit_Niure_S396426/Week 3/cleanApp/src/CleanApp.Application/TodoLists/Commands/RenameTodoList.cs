using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoLists.Commands;

public sealed record RenameTodoListCommand(Guid TodoListId, string Title) : IRequest<Result>;

public sealed class RenameTodoListCommandValidator : AbstractValidator<RenameTodoListCommand>
{
    public RenameTodoListCommandValidator()
    {
        RuleFor(c => c.TodoListId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty().MaximumLength(TodoListTitle.MaxLength);
    }
}

internal sealed class RenameTodoListCommandHandler(
    ITodoListRepository repository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<RenameTodoListCommand, Result>
{
    public async Task<Result> Handle(RenameTodoListCommand request, CancellationToken cancellationToken)
    {
        var id = new TodoListId(request.TodoListId);
        var list = await repository.GetByIdAsync(id, cancellationToken);
        // Ownership mismatch is reported as NotFound, not Forbidden, so callers can't use
        // this endpoint to probe for the existence of other users' resources.
        if (list is null || list.OwnerId != currentUser.UserId)
            return Result.Failure(TodoListErrors.NotFound(id));

        var renameResult = list.Rename(request.Title);
        if (renameResult.IsFailure)
            return renameResult;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
