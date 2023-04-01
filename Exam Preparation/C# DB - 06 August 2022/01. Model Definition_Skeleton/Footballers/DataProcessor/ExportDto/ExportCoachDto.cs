using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto;

[XmlType("Coach")]
public class ExportCoachDto
{
    [XmlElement("CoachName")]
    public string Name { get; set; } = null!;

    [XmlAttribute("FootballersCount")]
    public int FootballersCount { get; set; }

    [XmlArray("Footballers")]
    public ExportFootballersDTO[] Footballers { get; set; }
}
