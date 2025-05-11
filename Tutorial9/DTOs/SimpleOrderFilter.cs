namespace Tutorial9.Models.DTOs;

public class SimpleOrderFilter
{
    public int? IdProduct { get; set; }
    public int? Amount { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public SimpleOrderFilter(int? idProduct = null, int? amount = null , DateTime? createdAt = null)
    {
        IdProduct = idProduct;
        Amount = amount;
        CreatedAt = createdAt;
    }
    
    
}