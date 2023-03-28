using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export;

public class ExportProductDTO
{
    public ExportProductDTO(Product product)
    {
        this.SellerName = $"{product.Seller.FirstName} {product.Seller.LastName}";

        this.Price = product.Price;
        this.Name = product.Name;
    }

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("seller")]
    public string SellerName { get; set; } = null!;


}
