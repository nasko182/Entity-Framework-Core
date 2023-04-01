using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export;

[XmlType("car")]
public class ExportCarWithAttributesDTO
{
    public ExportCarWithAttributesDTO()
    {

    }
    public ExportCarWithAttributesDTO(Car car)
        : this()
    {
        this.Id = car.Id;
        this.Model = car.Model;
        this.TraveledDistance = car.TraveledDistance;
    }
    [XmlAttribute("id")]
    public int Id { get; set; } 

    [XmlAttribute("model")]
    public string Model { get; set; } = null!;

    [XmlAttribute("traveled-distance")]
    public long TraveledDistance { get; set; }
}
