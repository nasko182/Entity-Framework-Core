using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Despatcher")]
public class ImportDespatherDTO
{
    [XmlElement("Name")]
    [Required]
    [MinLength(ValidationConstants.DespatcherNameMinLenght)]
    [MaxLength(ValidationConstants.DespatcherNameMaxLenght)]
    public string Name { get; set; } = null!;

    [XmlElement("Position")]
    public string? Position { get; set; }

    [XmlArray("Trucks")]
    public ImportTruckDTO[] Trucks { get; set; }
}
