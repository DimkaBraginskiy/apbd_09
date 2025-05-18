using Tutorial9.Models.DTOs;

namespace Tutorial9.Services;

public interface IWarehouseService
{
    
    public Task<int> CreateProductWarehouseAsync(CancellationToken token, ProductWarehouseCreateDto dto);

    public Task<int> CreateProductWarehouseWithProcedureAsync(CancellationToken token, ProductWarehouseCreateDto dto);
}