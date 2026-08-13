using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoItems.Commands;

public sealed record UpdateTodoItemCommand(
    Guid TodoItemId,
    string Title,
    int Priority,
    string? Note,
    DateTime? ReminderUtc) : IRequest<Result>;

public sealed class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(c => c.TodoItemId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty().MaximumLength(TodoItemTitle.MaxLength);
        RuleFor(c => c.Priority).InclusiveBetween(0, 3);
        RuleFor(c => c.ReminderUtc)
            .GreaterThan(_ => DateTime.UtcNow)
            .When(c => c.ReminderUtc is not null)
            .WithMessage("Reminder must be set in the future.");
    }
}

internal sealed class UpdateTodoItemCommandHandler(ITodoItemRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTodoItemCommand, Result>
{
    public async Task<Result> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var id = new TodoItemId(request.TodoItemId);
        var item = await repository.GetByIdAsync(id, cancellationToken);
        if (item is null)
            return Result.Failure(TodoItemErrors.NotFound(id));

        var updateResult = item.UpdateDetails(request.Title, request.Priority, request.Note, request.ReminderUtc);
        if (updateResult.IsFailure)
            return updateResult;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
