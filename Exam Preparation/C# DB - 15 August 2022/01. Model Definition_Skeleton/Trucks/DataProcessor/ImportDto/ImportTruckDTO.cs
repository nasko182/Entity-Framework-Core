using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Common;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Truck")]
public class ImportTruckDTO
{
    [XmlElement("RegistrationNumber")]
    [MinLength(ValidationConstants.TruckRegistrationNumberLenght)]
    [MaxLength(ValidationConstants.TruckRegistrationNumberLenght)]
    [RegularExpression(@"[A-Z]{2}[0-9]{4}[A-Z]{2}")]
    public string? RegistrationNumber { get; set; }

    [XmlElement("VinNumber")]
    [Required]
    [MinLength(ValidationConstants.TruckVinNumberLenght)]
    [MaxLength(ValidationConstants.TruckVinNumberLenght)]
    public string VinNumber { get; set; } = null!;

    [XmlElement("TankCapacity")]
    [Range(ValidationConstants.TruckTankCapacityMinLenght,
        ValidationConstants.TruckTankCapacityMaxLenght)]
    public int TankCapacity { get; set; }

    [XmlElement("CargoCapacity")]
    [Range(ValidationConstants.TruckCargoCapacityMinLenght, ValidationConstants.TruckCargoCapacityMaxLenght)]
    public int CargoCapacity { get; set; }

    [XmlElement("CategoryType")]
    [Range(ValidationConstants.TruckCategoryTypeMinValue,
        ValidationConstants.TruckCategoryTypeMaxValue)]
    public int CategoryType { get; set; }

    [XmlElement("MakeType")]
    [Range(ValidationConstants.TruckMakeTypeMinValue,
        ValidationConstants.TruckMakeTypeMaxValue)]
    public int MakeType { get; set; }
}
