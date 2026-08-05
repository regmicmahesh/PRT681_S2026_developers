using SimpleCrudAppAPI.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace SimpleCrudAppAPI.Data;

public interface IProductRepository
{
    Task<int> InsertProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
}

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<int> InsertProductAsync(Product product)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string query = @"
                INSERT INTO Products (Name, Price, Description)
                VALUES (@Name, @Price, @Description);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = await connection.QuerySingleAsync<int>(query, product);
            return id;
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string query = "SELECT * FROM Products WHERE Id = @Id";
            var product = await connection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id });
            return product;
        }
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        using (IDbConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            const string query = "SELECT * FROM Products";
            var products = await connection.QueryAsync<Product>(query);
            return products;
        }
    }
}
