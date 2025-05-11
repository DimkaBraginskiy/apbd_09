namespace Tutorial9.Models;

public class Product_Warehouse
{
    public int IdProductWarehouse { get; set; }
    
    public int IdProduct { get; set; }
    public Product Product { get; set; }
    
    public int IdOrder { get; set; }
    public Order Order { get; set; }
    
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public DateTime Createdt { get; set; }

}