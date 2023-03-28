using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export;

public class ExportUserDTO
{
    public ExportUserDTO()
    {
        this.ProductsSold = new HashSet<SoldProductsDTO>();
    }

    public ExportUserDTO(User user):
        base()
    {
        this.FirstName = user.FirstName;
        this.LastName = user.LastName;
        ProductsSold = user.ProductsSold.Select(ps=> new SoldProductsDTO(ps)).ToList();
    }
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; } = null!;

    [JsonProperty("soldProducts")]
    public ICollection<SoldProductsDTO> ProductsSold { get; set; }
}
