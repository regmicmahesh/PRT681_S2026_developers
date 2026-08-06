namespace CleanApp.Domain.TodoLists;

public interface ITodoListRepository
{
    Task<TodoList?> GetByIdAsync(TodoListId id, CancellationToken cancellationToken = default);

    void Add(TodoList todoList);

    void Remove(TodoList todoList);
}
