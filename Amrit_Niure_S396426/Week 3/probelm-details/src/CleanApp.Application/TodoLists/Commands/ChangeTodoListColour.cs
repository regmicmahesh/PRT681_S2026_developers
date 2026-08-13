using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using FluentValidation;
using MediatR;

namespace CleanApp.Application.TodoLists.Commands;

public sealed record ChangeTodoListColourCommand(Guid TodoListId, string Colour) : IRequest<Result>;

public sealed class ChangeTodoListColourCommandValidator : AbstractValidator<ChangeTodoListColourCommand>
{
    public ChangeTodoListColourCommandValidator()
    {
        RuleFor(c => c.TodoListId).NotEmpty();
        RuleFor(c => c.Colour).Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
            .WithMessage("Colour must be a valid hex code, e.g. #FFAA00.");
    }
}

internal sealed class ChangeTodoListColourCommandHandler(ITodoListRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeTodoListColourCommand, Result>
{
    public async Task<Result> Handle(ChangeTodoListColourCommand request, CancellationToken cancellationToken)
    {
        var id = new TodoListId(request.TodoListId);
        var list = await repository.GetByIdAsync(id, cancellationToken);
        if (list is null)
            return Result.Failure(TodoListErrors.NotFound(id));

        var colourResult = list.ChangeColour(request.Colour);
        if (colourResult.IsFailure)
            return colourResult;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
