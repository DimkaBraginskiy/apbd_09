namespace Tutorial9.Services;

public interface IWarehouseService
{
    
    public Task<int> CreateProductWarehouseAsync(
        CancellationToken token,
        int IdProduct,
        int IdWarehouse,
        int Amount,
        DateTime CreatedAt);
}