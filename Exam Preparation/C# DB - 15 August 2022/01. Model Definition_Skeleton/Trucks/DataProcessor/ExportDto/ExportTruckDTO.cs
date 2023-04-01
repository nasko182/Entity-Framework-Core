using System.Xml;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto;

[XmlType("Truck")]
public class ExportTruckDTO
{
    [XmlElement("RegistrationNumber")]
    public string? RegistrationNumber { get; set; }

    [XmlElement("Make")]
    public MakeType Make { get; set; }
}
