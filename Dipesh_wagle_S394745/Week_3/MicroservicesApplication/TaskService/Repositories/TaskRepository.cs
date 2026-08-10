using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using TaskService.Models;

namespace TaskService.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public async Task<int> CreateTaskAsync(TaskItem task)
    {
        using var connection = CreateConnection();
        var sql = @"
            INSERT INTO Tasks (UserId, Title, Description, DueDate, IsCompleted) 
            VALUES (@UserId, @Title, @Description, @DueDate, @IsCompleted);
            SELECT last_insert_rowid();";
            
        return await connection.ExecuteScalarAsync<int>(sql, task);
    }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM Tasks WHERE Id = @Id AND UserId = @UserId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
        return rowsAffected > 0;
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id, int userId)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM Tasks WHERE Id = @Id AND UserId = @UserId";
        return await connection.QuerySingleOrDefaultAsync<TaskItem>(sql, new { Id = id, UserId = userId });
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM Tasks WHERE UserId = @UserId";
        return await connection.QueryAsync<TaskItem>(sql, new { UserId = userId });
    }

    public async Task<bool> UpdateTaskAsync(TaskItem task)
    {
        using var connection = CreateConnection();
        var sql = @"
            UPDATE Tasks 
            SET Title = @Title, Description = @Description, DueDate = @DueDate, IsCompleted = @IsCompleted
            WHERE Id = @Id AND UserId = @UserId";
            
        var rowsAffected = await connection.ExecuteAsync(sql, task);
        return rowsAffected > 0;
    }
}
