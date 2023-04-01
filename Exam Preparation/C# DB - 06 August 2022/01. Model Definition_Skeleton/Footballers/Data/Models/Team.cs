namespace Footballers.Data.Models;

using Common;
using Footballers.DataProcessor.ImportDto;
using System.ComponentModel.DataAnnotations;


public class Team
{
    public Team()
    {
        this.TeamsFootballers = new HashSet<TeamFootballer>();
    }
    public Team(ImportTeamDTO teamDTO, TeamFootballer[] teamFootballers)
        :this()
    {
        this.Name= teamDTO.Name;
        this.Nationality= teamDTO.Nationality;
        this.Trophies = teamDTO.Trophies;
        this.TeamsFootballers = teamFootballers;
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TeamNameMaxLenght)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.TeamNameMaxLenght)]
    public string Nationality { get; set; } = null!;

    public int Trophies { get; set; }

    public ICollection<TeamFootballer> TeamsFootballers { get; set; }

}
