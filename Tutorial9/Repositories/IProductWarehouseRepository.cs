using Tutorial9.Models.DTOs;

namespace Tutorial9.Repositories;

public interface IProductWarehouseRepository
{
    public Task<int> CreateProductWarehouseWithProcedureAsync(CancellationToken token,
        ProductWarehouseCreateDto dto);
}