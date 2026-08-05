using Dapper;
using Microsoft.Data.SqlClient;
using SimpleCrudApp.Models;

namespace SimpleCrudApp.Repositories;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection not found in configuration");
        
        EnsureDatabaseAndTableCreated();
    }

    private void EnsureDatabaseAndTableCreated()
    {
        var masterConnectionString = _connectionString.Replace("Database=SimpleCrudDb", "Database=master");
        
        using var masterConn = new SqlConnection(masterConnectionString);
        var dbExists = masterConn.ExecuteScalar<int>("SELECT 1 FROM sys.databases WHERE name = 'SimpleCrudDb'");
        if (dbExists == 0)
        {
            masterConn.Execute("CREATE DATABASE SimpleCrudDb");
        }

        using var dbConn = new SqlConnection(_connectionString);
        dbConn.Execute(@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' and xtype='U')
            BEGIN
                CREATE TABLE Products (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Price DECIMAL(18,2) NOT NULL,
                    Description NVARCHAR(500) NOT NULL
                )
            END
        ");
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<Product>("SELECT * FROM Products");
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
    }

    public async Task CreateAsync(Product product)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(@"
            INSERT INTO Products (Name, Price, Description) 
            VALUES (@Name, @Price, @Description)", product);
    }

    public async Task UpdateAsync(Product product)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(@"
            UPDATE Products 
            SET Name = @Name, Price = @Price, Description = @Description 
            WHERE Id = @Id", product);
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
    }
}
