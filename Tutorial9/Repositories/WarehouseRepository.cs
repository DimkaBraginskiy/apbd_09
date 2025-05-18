using Microsoft.Data.SqlClient;
using Tutorial9.Models.DTOs;
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
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("" +
                                                "SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse", connection);
        command.Parameters.AddWithValue("@IdWarehouse", IdWarehouse);

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