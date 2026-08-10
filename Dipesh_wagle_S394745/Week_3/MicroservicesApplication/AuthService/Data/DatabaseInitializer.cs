using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AuthService.Data;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public void Initialize()
    {
        var builder = new SqlConnectionStringBuilder(_connectionString);
        var dbName = builder.InitialCatalog;
        builder.InitialCatalog = "master";
        var masterConnection = builder.ConnectionString;

        using (var connection = new SqlConnection(masterConnection))
        {
            connection.Open();
            var checkDbSql = $"SELECT database_id FROM sys.databases WHERE Name = '{dbName}'";
            var dbId = connection.ExecuteScalar<int?>(checkDbSql);

            if (!dbId.HasValue)
            {
                connection.Execute($"CREATE DATABASE [{dbName}]");
            }
        }

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var sql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
                CREATE TABLE Users (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Username NVARCHAR(50) NOT NULL UNIQUE,
                    PasswordHash NVARCHAR(255) NOT NULL
                )";
            connection.Execute(sql);
        }
    }
}
