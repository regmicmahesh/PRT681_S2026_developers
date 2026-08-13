using CleanApp.Domain.TodoLists;

namespace CleanApp.Domain.TodoItems;

public interface ITodoItemRepository
{
    Task<TodoItem?> GetByIdAsync(TodoItemId id, CancellationToken cancellationToken = default);

    Task<bool> AnyForListAsync(TodoListId todoListId, CancellationToken cancellationToken = default);

    void Add(TodoItem item);

    void Remove(TodoItem item);
}
