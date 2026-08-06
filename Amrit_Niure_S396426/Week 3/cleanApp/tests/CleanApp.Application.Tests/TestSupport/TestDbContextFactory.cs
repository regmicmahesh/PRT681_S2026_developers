using CleanApp.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Application.Tests.TestSupport;

/// <summary>
/// Builds a real <see cref="ApplicationDbContext"/> backed by a real SQLite in-memory
/// database, so query handler tests exercise the exact same entity configurations and the
/// exact same provider as production - unlike the EF Core InMemory provider, which has known
/// bugs materializing entities with more than one owned type (TodoList has Title AND Colour).
/// The backing connection is deliberately never closed; it lives for the test process.
/// </summary>
public static class TestDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}
