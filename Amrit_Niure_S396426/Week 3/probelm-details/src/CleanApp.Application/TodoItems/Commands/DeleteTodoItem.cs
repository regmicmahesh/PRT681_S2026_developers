using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoItems.Commands;

public sealed record DeleteTodoItemCommand(Guid TodoItemId) : IRequest<Result>;

public sealed class DeleteTodoItemCommandValidator : AbstractValidator<DeleteTodoItemCommand>
{
    public DeleteTodoItemCommandValidator() => RuleFor(c => c.TodoItemId).NotEmpty();
}

internal sealed class DeleteTodoItemCommandHandler(ITodoItemRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTodoItemCommand, Result>
{
    public async Task<Result> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var id = new TodoItemId(request.TodoItemId);
        var item = await repository.GetByIdAsync(id, cancellationToken);
        if (item is null)
            return Result.Failure(TodoItemErrors.NotFound(id));

        repository.Remove(item);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
