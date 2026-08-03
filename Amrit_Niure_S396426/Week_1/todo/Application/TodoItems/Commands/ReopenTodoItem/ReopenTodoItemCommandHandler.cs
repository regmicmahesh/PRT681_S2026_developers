using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.TodoItems.Commands.ReopenTodoItem;

public sealed class ReopenTodoItemCommandHandler(
    ITodoItemRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReopenTodoItemCommand>
{
    public async Task Handle(ReopenTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(TodoItem), request.Id);

        todoItem.Reopen();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
