using Domain.Entities;

namespace Domain.Repositories;

public interface ITodoItemRepository
{
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<TodoItem>> GetAllAsync(bool? isCompleted, CancellationToken cancellationToken = default);

    Task AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default);

    void Remove(TodoItem todoItem);
}
