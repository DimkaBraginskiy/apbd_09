using System.Data.Common;
using Microsoft.Data.SqlClient;
using Tutorial9.Models;
using Tutorial9.Models.DTOs;

namespace Tutorial9.Repositories;

public class OrderRepository
{
    private readonly string _connectionString;
    private readonly IProductRepository _productRepository;

    public OrderRepository(IConfiguration configuration, IProductRepository productRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _productRepository = productRepository; 
    }

    public async Task<bool> OrderExistsAsync(CancellationToken token, int IdOrder)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("" +
                                                 "SELECT 1 FROM Order WHERE IdOrder = @IdOrder", connection);
        command.Parameters.AddWithValue("@IdOrder", IdOrder);

        await connection.OpenAsync(token);
        var result = await command.ExecuteScalarAsync(token);

        return result != null;
    }

    public async Task<bool> ProductExistsInOrderAsync(CancellationToken token, int IdProduct)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("" +
                                                 "SELECT 1 FROM Order WHERE IdProduct = @IdProduct", connection);
        command.Parameters.AddWithValue("@IdProduct", IdProduct);

        await connection.OpenAsync(token);
        var result = await command.ExecuteScalarAsync(token);

        return result != null;
    }

    public async Task<Order> GetEligibleOrderAsync(
        CancellationToken token, 
        SimpleOrderFilter filter)
    {

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(
            "SELECT TOP 1 o.Idorder, p.Price FROM [Order] o" +
            "JOIN [Product] p ON p.IdProduct = o.IdProduct" +
            "LEFT JOIN [Product_Warehouse] pw on pw.IdProduct = p.IdProduct" +
            "WHERE 1 = 1;", connection);

        if (filter.IdProduct != null)
        {
            command.CommandText += " AND o.IdProduct = @IdProduct";
            command.Parameters.AddWithValue("@IdProduct", filter.IdProduct);
        }

        if (filter.Amount != null)
        {
            command.CommandText += " AND o.Amount <= @Amount";
            command.Parameters.AddWithValue("@Amount", filter.Amount);
        }

        if (filter.CreatedAt != null)
        {
            command.CommandText += " AND o.CreatedAt <= @CreatedAt";
            command.Parameters.AddWithValue("@CreatedAt", filter.CreatedAt);
        }

        await connection.OpenAsync(token);
        var reader = await command.ExecuteReaderAsync(token);

        if (await reader.ReadAsync(token))
        {
            var order = new Order
            {
                IdOrder = reader.GetInt32(reader.GetOrdinal("IdOrder")),
                IdProduct = reader.GetInt32(reader.GetOrdinal("IdProduct")),
                Amount = reader.GetInt32(reader.GetOrdinal("Amount")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                FulfilledAt = reader.IsDBNull(reader.GetOrdinal("FulfilledAt"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("FilfilledAt"))
            };
            return order;
        }
        return null;
    }

    public async Task<int> UpdateFulfilledAtAsync(CancellationToken token, FulfillOrderRequestDto dto)
    {
        var product = await _productRepository.GetPriceByIdAsync(token, dto.IdProduct);
        var totalPrice = product * dto.Amount;
        var now = DateTime.Now;
        
        
        
        string query = "UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
        
        
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(token);
        
        DbTransaction transaction = await connection.BeginTransactionAsync(token);


        try
        {
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
            command.Parameters.AddWithValue("@IdOrder", dto.IdProduct);

            int affected = await command.ExecuteNonQueryAsync(token);
            if (affected == 0)
            {
                throw new Exception("No rows were updated");
            }

            command.Parameters.Clear();

            command.CommandText = @"
            INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
            OUTPUT INSERTED.IdProductWarehouse
            VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";


            command.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
            command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
            command.Parameters.AddWithValue("@IdOrder", dto.IdOrder);
            command.Parameters.AddWithValue("@Amount", dto.Amount);
            command.Parameters.AddWithValue("@Price", totalPrice);
            command.Parameters.AddWithValue("@CreatedAt", now);

            var result = await command.ExecuteScalarAsync(token);
            await transaction.CommitAsync(token);

            return (int)result;
        }
        catch
        {
            await transaction.RollbackAsync(token);
            throw;
        }
    }
}