using Newtonsoft.Json;

namespace ProductShop.DTOs.Import;

public class CategoryDTO
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
}
