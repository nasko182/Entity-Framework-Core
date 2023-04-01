using System.ComponentModel.DataAnnotations;
using Trucks.Common;
using Trucks.DataProcessor.ImportDto;

namespace Trucks.Data.Models;

public class Client
{
    public Client()
    {
        this.ClientsTrucks = new HashSet<ClientTruck>();
    }
    public Client(ImportClientDTO clientDTO)
        : this ()
    {
        this.Name= clientDTO.Name;
        this.Nationality = clientDTO.Nationality;
        this.Type= clientDTO.Type;
        this.ClientsTrucks = clientDTO.TrucksIds.Select(t => new ClientTruck() { TruckId = t }).ToList();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ClienrNameMaxLenght)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.ClientNationalityMaxLenght)]
    public string Nationality { get; set; } = null!;

    [Required]
    public string Type { get; set; } = null!;

    public ICollection<ClientTruck> ClientsTrucks { get; set; }
}
