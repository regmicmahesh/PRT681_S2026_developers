using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.Common.Interfaces;

/// <summary>
/// Read-side access to persistence, used directly by query handlers so reads can be
/// projected efficiently without going through the write-side repositories/aggregates.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
}
