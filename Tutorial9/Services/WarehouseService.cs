using Microsoft.AspNetCore.Http.HttpResults;

namespace Tutorial9.Services;

public class WarehouseService : IWarehouseService
{
    public async Task<int> AddWarehouseProductAsync(
        CancellationToken token,
        int IdProduct,
        int IdWarehouse,
        int Amount,
        DateTime CreatedAt)
    {
        
    }
}