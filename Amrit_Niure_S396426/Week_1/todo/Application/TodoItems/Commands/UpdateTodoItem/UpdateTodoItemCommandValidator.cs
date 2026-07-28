using Domain.ValueObjects;
using FluentValidation;

namespace Application.TodoItems.Commands.UpdateTodoItem;

public sealed class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(TodoTitle.MaxLength);

        RuleFor(c => c.Priority)
            .IsInEnum();
    }
}
