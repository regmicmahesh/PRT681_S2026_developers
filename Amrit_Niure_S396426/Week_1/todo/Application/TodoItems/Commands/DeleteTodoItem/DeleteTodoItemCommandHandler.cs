using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.TodoItems.Commands.DeleteTodoItem;

public sealed class DeleteTodoItemCommandHandler(
    ITodoItemRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTodoItemCommand>
{
    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(TodoItem), request.Id);

        repository.Remove(todoItem);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
