using CarDealer.Models;

namespace CarDealer.DTOs.Import;

public class importPartDTO
{
    public importPartDTO()
    {

    }
    public importPartDTO(Part part)
        : base() 
    {
        this.Name = part.Name;
        this.Price = part.Price;
        this.Quantity = part.Quantity;
        this.SupplierId = part.SupplierId;
    }
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int SupplierId { get; set; }
}
