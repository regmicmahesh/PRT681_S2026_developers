using CleanApp.Domain.TodoLists;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Persistence.Repositories;

internal sealed class TodoListRepository(ApplicationDbContext context) : ITodoListRepository
{
    public Task<TodoList?> GetByIdAsync(TodoListId id, CancellationToken cancellationToken = default) =>
        context.TodoLists.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public void Add(TodoList todoList) => context.TodoLists.Add(todoList);

    public void Remove(TodoList todoList) => context.TodoLists.Remove(todoList);
}
