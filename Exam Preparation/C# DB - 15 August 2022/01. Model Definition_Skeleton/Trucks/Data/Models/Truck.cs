namespace Trucks.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore.ChangeTracking;

using Common;
using Enums;
using Trucks.DataProcessor.ImportDto;

public class Truck
{
    public Truck()
    {
            this.ClientsTrucks = new HashSet<ClientTruck>();
    }
    public Truck(ImportTruckDTO truckDTO)
        :this()
    {
        this.RegistrationNumber = truckDTO.RegistrationNumber;
        this.VinNumber = truckDTO.VinNumber;
        this.TankCapacity= truckDTO.TankCapacity;
        this.CargoCapacity= truckDTO.CargoCapacity;
        this.CategoryType= (CategoryType)truckDTO.CategoryType;
        this.MakeType= (MakeType)truckDTO.MakeType;
    }
    [Key]
    public int Id { get; set; }

    [MaxLength(ValidationConstants.TruckRegistrationNumberLenght)]
    public string? RegistrationNumber { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TruckVinNumberLenght)]
    public string VinNumber { get; set; } = null!;

    public int TankCapacity { get; set; }

    public int CargoCapacity { get; set; }

    [Required]
    public CategoryType CategoryType { get; set; }

    [Required]
    public MakeType MakeType { get; set; }

    [Required]
    [ForeignKey(nameof (Despatcher))]
    public int DespatcherId { get; set; }

    public Despatcher Despatcher { get; set; } = null!;

    public ICollection<ClientTruck> ClientsTrucks { get; set; }

}
