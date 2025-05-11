using Microsoft.Data.SqlClient;
using Tutorial9.Services;

namespace Tutorial9.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly string _connectionString;
    
    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task<bool> WarehouseExistsAsync(CancellationToken token, int IdWarehouse)
    {   
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("" +
                                           "SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse", connection);
        command.Parameters.AddWithValue("@IdWarehouse", IdWarehouse);

        await connection.OpenAsync(token);
        var result = await command.ExecuteScalarAsync(token);

        return result != null;
    }

    public async Task<bool> ProductExistsInOrderAsync(CancellationToken token, int IdProduct)
    {
        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("" +
                                     "SELECT 1 FROM Order WHERE IdProduct = @IdProduct", connection);
        command.Parameters.AddWithValue("@IdProduct", IdProduct);

        await connection.OpenAsync(token);
        var result = await command.ExecuteScalarAsync(token);

        return result != null;
    }
    

    public async Task<int> CreateProductWarehouseAsync(
        CancellationToken token,
        int IdProduct,
        int IdWarehosue,
        int Amount,
        DateTime CreatedAt)
    {
        return IdProduct;
    }
}