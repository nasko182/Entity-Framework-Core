namespace Footballers.Data.Models;

using Common;

using Footballers.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;

public class Coach
{
    public Coach()
    {
        this.Footballers = new HashSet<Footballer>();
    }
    public Coach(ImportCoachDTO coachDTO, Footballer[] footballers)
        : this()
    {
        this.Name = coachDTO.Name;
        this.Nationality = coachDTO.Nationality;
        this.Footballers = footballers;
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.CoachNameMaxLenght)]
    public string Name { get; set; } = null!;

    [Required]
    public string Nationality { get; set; } = null!;

    public ICollection<Footballer> Footballers { get; set; }
}
