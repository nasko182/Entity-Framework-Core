namespace Footballers.Data.Models;

using Common;
using Enums;
using Footballers.DataProcessor.ImportDto;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

public class Footballer
{
    public Footballer()
    {
        this.TeamsFootballers = new HashSet<TeamFootballer>();  
    }
    public Footballer(ImportFootballerDTO footballerDTO,DateTime contractStartDate, DateTime contractEndDate)
        :this()
    {
        this.Name= footballerDTO.Name;
        this.ContractStartDate = contractStartDate;
        this.ContractEndDate= contractEndDate;
        this.BestSkillType = (BestSkillType)footballerDTO.BestSkillType;
        this.PositionType = (PositionType)footballerDTO.PositionType;
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.FootballerNameMaxLenght)]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime ContractStartDate  { get; set; }


    [Required]
    public DateTime ContractEndDate  { get; set; }

    [Required]
    public PositionType PositionType { get; set; }

    [Required]
    public BestSkillType BestSkillType  { get; set; }

    [Required]
    [ForeignKey(nameof(Coach))]
    public int CoachId { get; set; }

    public Coach Coach { get; set; } = null!;

    public ICollection<TeamFootballer> TeamsFootballers { get; set; }
}
