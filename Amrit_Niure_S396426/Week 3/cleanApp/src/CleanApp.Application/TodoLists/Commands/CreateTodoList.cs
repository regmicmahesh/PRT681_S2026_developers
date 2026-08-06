using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoLists.Commands;

public sealed record CreateTodoListCommand(string Title, string? Colour) : IRequest<Result<Guid>>;

public sealed class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MaximumLength(TodoListTitle.MaxLength);

        RuleFor(c => c.Colour)
            .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
            .When(c => c.Colour is not null)
            .WithMessage("Colour must be a valid hex code, e.g. #FFAA00.");
    }
}

internal sealed class CreateTodoListCommandHandler(
    ITodoListRepository repository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<CreateTodoListCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var result = TodoList.Create(currentUser.UserId, request.Title, request.Colour);
        if (result.IsFailure)
            return Result.Failure<Guid>(result.Error);

        repository.Add(result.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.Id.Value;
    }
}
