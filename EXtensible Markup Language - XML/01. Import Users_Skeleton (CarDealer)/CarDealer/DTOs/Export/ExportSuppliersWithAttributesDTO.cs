using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export;

[XmlType("supplier")]
public class ExportSuppliersWithAttributesDTO
{
    public ExportSuppliersWithAttributesDTO()
    {

    }
    public ExportSuppliersWithAttributesDTO(Supplier supplier)
        : this()
    {
        this.Id = supplier.Id;
        this.Name = supplier.Name;
        this.PartsCount = supplier.Parts.Count;
    }
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; } = null!;

    [XmlAttribute("parts-count")]
    public int PartsCount { get; set; }
}
