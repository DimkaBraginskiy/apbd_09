using Microsoft.AspNetCore.Http.HttpResults;
using Tutorial9.Models.DTOs;
using Tutorial9.Repositories;

namespace Tutorial9.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IProductWarehouseRepository _productWarehouseRepository;
    
    public WarehouseService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IProductWarehouseRepository productWarehouseRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _productWarehouseRepository = productWarehouseRepository;
    }
    
    public async Task<int> CreateProductWarehouseAsync(
        CancellationToken token, ProductWarehouseCreateDto dto)
    {
        if (dto.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0.");
        }

        
        if (dto.CreatedAt > DateTime.Now)
        {
            throw new ArgumentException("CreatedAt can not be a future date.");
        }
        
        if(await _productRepository.ProductExistsAsync(token, dto.IdProduct) == false)
        {
            throw new ArgumentException("Product does not exist.");
        }
        
        if(await _warehouseRepository.WarehouseExistsAsync(token, dto.IdWarehouse) == false)
        {
            throw new ArgumentException("Warehouse does not exist.");
        }
        
        if(await _orderRepository.ProductExistsInOrderAsync(token, dto.IdProduct) == false)
        {
            throw new ArgumentException("Product does not exist in order.");
        }

        var order = await _orderRepository.GetEligibleOrderAsync(token, new SimpleOrderFilter
        {
            IdProduct = dto.IdProduct,
            Amount = dto.Amount,
            CreatedAt = dto.CreatedAt
        });

        if (order == null)
            throw new Exception("No matching order found.");


        var fulfillDto = new FulfillOrderRequestDto
        {
            IdProduct = dto.IdProduct,
            IdWarehouse = dto.IdWarehouse,
            IdOrder = order.IdOrder,
            Amount = dto.Amount
        };
            
        

        var id = await _orderRepository.UpdateFulfilledAtAsync(token, fulfillDto);

        return id;
    }


    public async Task<int> CreateProductWarehouseWithProcedureAsync(CancellationToken token,
        ProductWarehouseCreateDto dto)
    {
        var result = await _productWarehouseRepository.CreateProductWarehouseWithProcedureAsync(
            token, dto);

        return result;
    }
}