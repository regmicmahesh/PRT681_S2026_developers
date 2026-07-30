using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.TodoItems.Commands.UpdateTodoItem;

public sealed class UpdateTodoItemCommandHandler(
    ITodoItemRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTodoItemCommand>
{
    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(TodoItem), request.Id);

        todoItem.UpdateDetails(request.Title, request.Description, request.Priority, request.DueDate);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
