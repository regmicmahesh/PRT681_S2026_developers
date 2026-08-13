using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoItems.Commands;

public sealed record ReopenTodoItemCommand(Guid TodoItemId) : IRequest<Result>;

public sealed class ReopenTodoItemCommandValidator : AbstractValidator<ReopenTodoItemCommand>
{
    public ReopenTodoItemCommandValidator() => RuleFor(c => c.TodoItemId).NotEmpty();
}

internal sealed class ReopenTodoItemCommandHandler(ITodoItemRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<ReopenTodoItemCommand, Result>
{
    public async Task<Result> Handle(ReopenTodoItemCommand request, CancellationToken cancellationToken)
    {
        var id = new TodoItemId(request.TodoItemId);
        var item = await repository.GetByIdAsync(id, cancellationToken);
        if (item is null)
            return Result.Failure(TodoItemErrors.NotFound(id));

        var result = item.Reopen();
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
