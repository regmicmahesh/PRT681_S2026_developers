using Domain.ValueObjects;
using FluentValidation;

namespace Application.TodoItems.Commands.CreateTodoItem;

public sealed class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(TodoTitle.MaxLength);

        RuleFor(c => c.Priority)
            .IsInEnum();
    }
}
