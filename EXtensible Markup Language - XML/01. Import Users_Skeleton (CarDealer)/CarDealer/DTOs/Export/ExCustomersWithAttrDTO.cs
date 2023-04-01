using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export;

[XmlType("customer")]
public class ExCustomersWithAttrDTO
{
    public ExCustomersWithAttrDTO()
    {

    }
    public ExCustomersWithAttrDTO(Customer customer, string spentMoney)
        : this()
    {
        this.FullName= customer.Name;
        this.BoughtCars = customer.Sales.Where(c=>c.CustomerId==customer.Id).Count();
        this.SpentMoney= spentMoney;
    }

    [XmlAttribute("full-name")]
    public string FullName { get; set; } = null!;

    [XmlAttribute("bought-cars")]
    public int BoughtCars { get; set; }

    [XmlAttribute("spent-money")]
    public string SpentMoney { get; set; }
}
