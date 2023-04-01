using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Trucks.Common;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto;

public class ImportClientDTO
{
    [JsonProperty("Name")]
    [Required]
    [MinLength(ValidationConstants.ClientNameMinLenght)]
    [MaxLength(ValidationConstants.ClienrNameMaxLenght)]
    public string Name { get; set; } = null!;

    [JsonProperty("Nationality")]
    [Required]
    [MinLength(ValidationConstants.ClientNationalityMinLenght)]
    [MaxLength(ValidationConstants.ClientNationalityMaxLenght)]
    public string Nationality { get; set; } = null!;

    [JsonProperty("Type")]
    [Required]
    public string Type { get; set; } = null!;

    [JsonProperty("Trucks")]
    public int[] TrucksIds { get; set; }
}
