using System.Xml;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto;

[XmlType("Despatcher")]
public class ExportDespatcherDTO
{
    [XmlAttribute("TrucksCount")]
    public int TrucksCount { get; set; }

    [XmlElement("DespatcherName")]
    public string Name { get; set; } = null!;

    [XmlArray("Trucks")]
    public ExportTruckDTO[] Trucks { get; set; }
}
