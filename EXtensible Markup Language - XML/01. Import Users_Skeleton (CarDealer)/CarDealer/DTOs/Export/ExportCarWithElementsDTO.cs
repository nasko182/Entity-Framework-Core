using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export;

[XmlType("car")]
public class ExportCarWithElementsDTO
{
    public ExportCarWithElementsDTO()
    {

    }
    public ExportCarWithElementsDTO(Car car)
        : this ()
    {
        this.Make = car.Make;
        this.Model = car.Model;
        this.TraveledDistance = car.TraveledDistance;
    }
    [XmlElement("make")]
    public string Make { get; set; } = null!;

    [XmlElement("model")]
    public string Model { get; set; } = null!;

    [XmlElement("traveled-distance")]
    public long TraveledDistance { get; set; }
}
