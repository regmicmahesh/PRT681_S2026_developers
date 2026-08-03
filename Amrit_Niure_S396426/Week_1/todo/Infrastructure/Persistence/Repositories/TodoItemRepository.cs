using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class TodoItemRepository(TodoDbContext context) : ITodoItemRepository
{
    public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.TodoItems.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<List<TodoItem>> GetAllAsync(bool? isCompleted, CancellationToken cancellationToken = default)
    {
        var query = context.TodoItems.AsQueryable();

        if (isCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == isCompleted.Value);

        return await query
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default) =>
        await context.TodoItems.AddAsync(todoItem, cancellationToken);

    public void Remove(TodoItem todoItem) => context.TodoItems.Remove(todoItem);
}
