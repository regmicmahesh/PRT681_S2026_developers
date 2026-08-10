using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace TaskService.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var sql = @"
            CREATE TABLE IF NOT EXISTS Tasks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Title TEXT NOT NULL,
                Description TEXT,
                DueDate TEXT,
                IsCompleted INTEGER NOT NULL DEFAULT 0
            )";
        connection.Execute(sql);
    }
}
