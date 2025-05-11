using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<bool> ProductExistsAsync(CancellationToken token, int IdProduct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("" +
                                                "SELECT 1 FROM Product WHERE IdProduct = @IdProduct", connection);
        command.Parameters.AddWithValue("@IdProduct", IdProduct);
        
        await connection.OpenAsync(token);
        var result = await command.ExecuteScalarAsync(token);

        return result != null;
    }

    public async Task<decimal> GetPriceByIdAsync(CancellationToken token, int IdProduct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("" +
                                                "SELECT Price FROM Product WHERE IdProduct = @IdProduct", connection);
        
        command.Parameters.AddWithValue("@IdProduct", IdProduct);
        
        await connection.OpenAsync(token);

        var result = await command.ExecuteReaderAsync(token);
        
        if (await result.ReadAsync(token))
        {
            return result.GetDecimal(result.GetOrdinal("Price"));
        }
        
        throw new Exception("Product not found");
    }
}