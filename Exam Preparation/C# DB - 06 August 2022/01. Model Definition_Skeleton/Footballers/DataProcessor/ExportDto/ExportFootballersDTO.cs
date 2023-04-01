using Footballers.Data.Models.Enums;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto;

[XmlType("Footballer")]
public class ExportFootballersDTO
{
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("Position")]
    public string PositionType { get; set; } = null!;
}
