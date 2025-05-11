using System.ComponentModel.DataAnnotations;

namespace Tutorial9.Models.DTOs;

public class ProductWarehouseCreateDto
{
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public ProductWarehouseCreateDto(int idProduct, int idWarehouse, int amount, DateTime createdAt)
    {
        IdProduct = idProduct;
        IdWarehouse = idWarehouse;
        Amount = amount;
        CreatedAt = createdAt;
    }
}