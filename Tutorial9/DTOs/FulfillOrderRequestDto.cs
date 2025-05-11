using System.ComponentModel.DataAnnotations;

namespace Tutorial9.Models.DTOs;

public class FulfillOrderRequestDto
{
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int IdOrder { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public FulfillOrderRequestDto(int idProduct, int idWarehouse, int amount, DateTime createdAt)
    {
        IdProduct = idProduct;
        IdWarehouse = idWarehouse;
        Amount = amount;
        CreatedAt = createdAt;
    }
}