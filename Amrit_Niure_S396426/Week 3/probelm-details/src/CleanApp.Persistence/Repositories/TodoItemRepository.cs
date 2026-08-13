using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Persistence.Repositories;

internal sealed class TodoItemRepository(ApplicationDbContext context) : ITodoItemRepository
{
    public Task<TodoItem?> GetByIdAsync(TodoItemId id, CancellationToken cancellationToken = default) =>
        context.TodoItems.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public Task<bool> AnyForListAsync(TodoListId todoListId, CancellationToken cancellationToken = default) =>
        context.TodoItems.AnyAsync(i => i.TodoListId == todoListId, cancellationToken);

    public void Add(TodoItem item) => context.TodoItems.Add(item);

    public void Remove(TodoItem item) => context.TodoItems.Remove(item);
}
