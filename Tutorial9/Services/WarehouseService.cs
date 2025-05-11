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
        CancellationToken token,
        int IdProduct,
        int IdWarehouse,
        int Amount,
        DateTime CreatedAt)
    {
        if (Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0.");
        }

        
        if (CreatedAt > DateTime.Now)
        {
            throw new ArgumentException("CreatedAt can not be a future date.");
        }
        
        if(await _productRepository.ProductExistsAsync(token, IdProduct) == false)
        {
            throw new ArgumentException("Product does not exist.");
        }
        
        if(await _warehouseRepository.WarehouseExistsAsync(token, IdWarehouse) == false)
        {
            throw new ArgumentException("Warehouse does not exist.");
        }
        
        if(await _orderRepository.ProductExistsInOrderAsync(token, IdProduct) == false)
        {
            throw new ArgumentException("Product does not exist in order.");
        }

        var order = await _orderRepository.GetEligibleOrderAsync(token, new SimpleOrderFilter
        {
            IdProduct = IdProduct,
            Amount = Amount,
            CreatedAt = CreatedAt
        });

        if (order == null)
            throw new Exception("No matching order found.");


        var fulfillDto = new FulfillOrderRequestDto
        {
            IdProduct = IdProduct,
            IdWarehouse = IdWarehouse,
            IdOrder = order.IdOrder,
            Amount = Amount
        };
            
        

        var id = await _orderRepository.UpdateFulfilledAtAsync(token, fulfillDto);

        return id;
    }
}