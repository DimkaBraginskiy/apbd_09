using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tutorial9.Models.DTOs;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController
{
    private readonly IWarehouseService _warehouseService;
    
    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddWarehouseProductAsync(CancellationToken token,
        [FromBody] WarehouseProductCreateDto warehouseProductDto)
    {
        if (warehouseProductDto == null)
        {
            return BadRequest("Warehouse_Product can not be null");
        }
        
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    }
}