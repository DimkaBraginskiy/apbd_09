using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial9.Models.DTOs;

namespace Tutorial9.Repositories;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    
    private readonly string _connectionString;

    public ProductWarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<int> CreateProductWarehouseWithProcedureAsync(CancellationToken token, ProductWarehouseCreateDto dto)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("AddProductToWarehouse", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };
        
        command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
        command.Parameters.AddWithValue("@Amount", dto.Amount);
        command.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);
            
        await connection.OpenAsync(token);

        await using var reader = await command.ExecuteReaderAsync(token);
        
        if(await reader.ReadAsync(token))
        {
            return Convert.ToInt32(reader["NewId"]);
        }
        
        
        /*command.CommandText = "AddProductToWarehouse";
        command.CommandType = CommandType.StoredProcedure;*/

        throw new InvalidOperationException("No id returned by a Stored Procedure.");
    }
}