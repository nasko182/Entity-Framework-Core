using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Import;

public class UserDTO
{
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; } = null!;

    [JsonProperty("age")]
    public int? Age { get; set; }
}
