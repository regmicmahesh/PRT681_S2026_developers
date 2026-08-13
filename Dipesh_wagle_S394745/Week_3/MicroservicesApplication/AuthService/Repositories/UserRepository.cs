using System.Data;
using AuthService.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AuthService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using var connection = CreateConnection();
        var sql = @"
            INSERT INTO Users (Username, PasswordHash) 
            VALUES (@Username, @PasswordHash);
            SELECT CAST(SCOPE_IDENTITY() as int);";
            
        return await connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM Users WHERE Username = @Username";
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }
}
