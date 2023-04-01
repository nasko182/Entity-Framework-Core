namespace Footballers.DataProcessor.ImportDto;

using Common;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

public class ImportTeamDTO
{
    [Required]
    [JsonProperty("Name")]
    [MinLength(ValidationConstants.TeamNameMinLenght)]
    [MaxLength(ValidationConstants.TeamNameMaxLenght)]
    [RegularExpression(@"[a-zA-z\ \.\-\d]*")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonProperty("Nationality")]
    [MinLength(ValidationConstants.TeamNationalityMinLenght)]
    [MaxLength(ValidationConstants.TeamNationalityMaxLenght)]
    public string Nationality { get; set; } = null!;

    [JsonProperty("Trophies")]
    public int Trophies { get; set; }

    [JsonProperty("Footballers")]
    public int[] Footballers { get; set; }
}
