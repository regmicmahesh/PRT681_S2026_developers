using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.TodoItems.Commands.CreateTodoItem;

public sealed class CreateTodoItemCommandHandler(
    ITodoItemRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTodoItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = TodoItem.Create(request.Title, request.Description, request.Priority, request.DueDate);

        await repository.AddAsync(todoItem, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return todoItem.Id;
    }
}
