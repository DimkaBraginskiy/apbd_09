﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tutorial9.Models.DTOs;
using Tutorial9.Services;

namespace Tutorial9.Controllers;


[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    
    private readonly IWarehouseService _warehouseService;
    
    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouseProductAsync(CancellationToken token,
        [FromBody] ProductWarehouseCreateDto productWarehouseDto)
    {
        try
        {
            var id = await _warehouseService.CreateProductWarehouseAsync(
                token, productWarehouseDto);
            

            return Ok( new { Id = id});
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpPost("procedure")]
    public async Task<IActionResult> CreateWarehosueProductWithProcedureAsync(CancellationToken token, 
        [FromBody] ProductWarehouseCreateDto productWarehouseDto)
    {
        try
        {
            var id = await _warehouseService.CreateProductWarehouseWithProcedureAsync(
                token, productWarehouseDto);
            

            return Ok( new { Id = id});
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}