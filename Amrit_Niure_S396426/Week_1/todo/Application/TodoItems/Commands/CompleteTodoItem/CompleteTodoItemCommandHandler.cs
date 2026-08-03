using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.TodoItems.Commands.CompleteTodoItem;

public sealed class CompleteTodoItemCommandHandler(
    ITodoItemRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CompleteTodoItemCommand>
{
    public async Task Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(TodoItem), request.Id);

        todoItem.Complete();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
