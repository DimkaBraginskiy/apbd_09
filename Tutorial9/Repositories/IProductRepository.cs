namespace Tutorial9.Repositories;

public interface IProductRepository
{
    public Task<bool> ProductExistsAsync(CancellationToken token, int IdProduct);
    
    public Task<decimal> GetPriceByIdAsync(CancellationToken token, int IdProduct);
}