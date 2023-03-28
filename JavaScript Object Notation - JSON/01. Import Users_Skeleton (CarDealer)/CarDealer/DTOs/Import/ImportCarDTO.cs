using CarDealer.Models;

namespace CarDealer.DTOs.Import;

public class ImportCarDTO
{
    public ImportCarDTO()
    {
        this.PartsId = new List<int>();
    }
    public ImportCarDTO(Car car)
        : base() 
    {
        this.Make = car.Make;
        this.Model = car.Model;
        this.TravelledDistance = car.TraveledDistance;
    }
    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public long TravelledDistance { get; set; }

    public ICollection<int> PartsId { get; set; } 
}
