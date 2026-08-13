using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoItems.Commands;

public sealed record CompleteTodoItemCommand(Guid TodoItemId) : IRequest<Result>;

public sealed class CompleteTodoItemCommandValidator : AbstractValidator<CompleteTodoItemCommand>
{
    public CompleteTodoItemCommandValidator() => RuleFor(c => c.TodoItemId).NotEmpty();
}

internal sealed class CompleteTodoItemCommandHandler(ITodoItemRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CompleteTodoItemCommand, Result>
{
    public async Task<Result> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var id = new TodoItemId(request.TodoItemId);
        var item = await repository.GetByIdAsync(id, cancellationToken);
        if (item is null)
            return Result.Failure(TodoItemErrors.NotFound(id));

        var result = item.Complete();
        if (result.IsFailure)
            return result;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
