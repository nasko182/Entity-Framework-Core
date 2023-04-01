using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("part")]
    public class ExportCarPartsWithAttributesDTOs
    {
        public ExportCarPartsWithAttributesDTOs()
        {

        }
        public ExportCarPartsWithAttributesDTOs(Part part)
        {
            this.Name = part.Name;
            this.Price = part.Price;
        }

        [XmlAttribute("name")]
        public string Name { get; set; } = null!;

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
