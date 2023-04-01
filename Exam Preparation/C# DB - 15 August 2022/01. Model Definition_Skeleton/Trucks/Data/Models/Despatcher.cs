using System.ComponentModel.DataAnnotations;
using Trucks.Common;
using Trucks.DataProcessor.ImportDto;

namespace Trucks.Data.Models;

public class Despatcher
{
    public Despatcher()
    {
        this.Trucks = new HashSet<Truck>();
    }
    public Despatcher(ImportDespatherDTO despatherDTO, Truck[] trucks)
        : this ()
    {
        this.Name = despatherDTO.Name;
        this.Position = despatherDTO.Position;
        this.Trucks = trucks;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.DespatcherNameMaxLenght)]
    public string Name { get; set; } = null!;

    public string? Position { get; set; }

    public ICollection<Truck> Trucks { get; set; }
}
