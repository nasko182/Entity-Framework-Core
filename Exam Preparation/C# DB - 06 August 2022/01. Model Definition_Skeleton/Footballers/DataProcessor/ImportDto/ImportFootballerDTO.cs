namespace Footballers.DataProcessor.ImportDto;

using Common;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

[XmlType("Footballer")]
public class ImportFootballerDTO
{
    [XmlElement("Name")]
    [Required]
    [MinLength(ValidationConstants.FootballerNameMinLenght)]
    [MaxLength(ValidationConstants.FootballerNameMaxLenght)]
    public string Name { get; set; } = null!;

    [XmlElement("ContractStartDate")]
    [Required]
    public string ContractStartDate { get; set; } = null!;


    [XmlElement("ContractEndDate")]
    [Required]
    public string ContractEndDate { get; set; } = null!;


    [XmlElement("BestSkillType")]
    [Required]
    public int PositionType { get; set; }

    [XmlElement("PositionType")]
    [Required]
    public int BestSkillType { get; set; }
}
