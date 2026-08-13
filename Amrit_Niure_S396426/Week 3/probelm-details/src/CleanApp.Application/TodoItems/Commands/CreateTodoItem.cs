using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoItems.Commands;

public sealed record CreateTodoItemCommand(
    Guid TodoListId,
    string Title,
    int Priority,
    string? Note,
    DateTime? ReminderUtc) : IRequest<Result<Guid>>;

public sealed class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(c => c.TodoListId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty().MaximumLength(TodoItemTitle.MaxLength);
        RuleFor(c => c.Priority).InclusiveBetween(0, 3);
        RuleFor(c => c.ReminderUtc)
            .GreaterThan(_ => DateTime.UtcNow)
            .When(c => c.ReminderUtc is not null)
            .WithMessage("Reminder must be set in the future.");
    }
}

internal sealed class CreateTodoItemCommandHandler(
    ITodoListRepository listRepository,
    ITodoItemRepository itemRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTodoItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var listId = new TodoListId(request.TodoListId);
        if (!await listRepository.ExistsAsync(listId, cancellationToken))
            return Result.Failure<Guid>(TodoListErrors.NotFound(listId));

        var itemResult = TodoItem.Create(listId, request.Title, request.Priority, request.ReminderUtc, request.Note);
        if (itemResult.IsFailure)
            return Result.Failure<Guid>(itemResult.Error);

        itemRepository.Add(itemResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return itemResult.Value.Id.Value;
    }
}
