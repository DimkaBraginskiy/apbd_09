namespace Tutorial9.Repositories;

public interface IWarehouseRepository
{
    public Task<int> CreateProductWarehouseAsync(
        CancellationToken token,
        int IdProduct,
        int IdWarehouse,
        int Amount,
        DateTime CreatedAt);
    

    public Task<bool> WarehouseExistsAsync(CancellationToken token, int IdWarehouse);
    public Task<bool> ProductExistsInOrderAsync(CancellationToken token, int IdProduct);
}