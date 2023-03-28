namespace ProductShop.DTOs.Import;

public class ProductDTO
{
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int SellerId { get; set; }

    public int? BuyerId { get; set; }
}
