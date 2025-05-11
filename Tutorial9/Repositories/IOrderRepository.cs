using Tutorial9.Models;
using Tutorial9.Models.DTOs;

namespace Tutorial9.Repositories;

public interface IOrderRepository
{
    public Task<bool> OrderExistsAsync(CancellationToken token, int IdOrder);

    public Task<Order> GetEligibleOrderAsync(CancellationToken token, SimpleOrderFilter filter);

    public Task<int> UpdateFulfilledAtAsync(CancellationToken token, FulfillOrderRequestDto dto);
}