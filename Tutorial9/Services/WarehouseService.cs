using Microsoft.AspNetCore.Http.HttpResults;
using Tutorial9.Repositories;

namespace Tutorial9.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _repository;
    
    public WarehouseService(IWarehouseRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<int> CreateProductWarehouseAsync(
        CancellationToken token,
        int IdProduct,
        int IdWarehouse,
        int Amount,
        DateTime CreatedAt)
    {
        // 1. Check if Product exists.
        // In SQL Procedure
        
        // 2. Check if Warehouse exists.
        // In SQL Procedure
        
        
        // 3. Check if the Amount > 0.
        if (Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0.");
        }

        // 4. Check if the CreatedAt is not in the future.
        if (CreatedAt > DateTime.Now)
        {
            throw new ArgumentException("CreatedAt can not be a future date.");
        }


        var id = await _repository.CreateProductWarehouseAsync(
            token,
            IdProduct,
            IdWarehouse,
            Amount,
            CreatedAt);

        return id;
    }
}