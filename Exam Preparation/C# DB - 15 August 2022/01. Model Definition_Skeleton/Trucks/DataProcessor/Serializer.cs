namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatcher = context.Despatchers
                .Where(d => d.Trucks.Count() > 0)
                .Include(t => t.Trucks)
                .OrderByDescending(d=>d.Trucks.Count())
                .ThenBy(d => d.Name)
                .ToArray()
                .Select(d => new ExportDespatcherDTO() 
                {
                    TrucksCount = d.Trucks.Count(),
                    Name= d.Name,
                    Trucks = d.Trucks.Select(t=> new ExportTruckDTO()
                    {
                        RegistrationNumber= t.RegistrationNumber,
                        Make= t.MakeType,
                    }).OrderBy(t=>t.RegistrationNumber).ToArray()
                }).ToArray();

            var xmlHelper = new XmlHelper();
            var rootName = "Despatchers";
            return xmlHelper.Serialize<ExportDespatcherDTO[]>(despatcher, rootName);

        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var validClients = context.Clients
                .AsNoTracking()
                .Include(c => c.ClientsTrucks)
                .ThenInclude(c => c.Truck)
                .Where(c => c.ClientsTrucks.FirstOrDefault(t => t.Truck.TankCapacity >= capacity) != null)
                .Select(c => c.Id).ToArray();

            var clients = context.Clients
                .AsNoTracking()
                .Include(c => c.ClientsTrucks)
                .ThenInclude(c => c.Truck)
                .Where(c => c.ClientsTrucks.FirstOrDefault(t => t.Truck.TankCapacity >= capacity) != null)
                .ToArray()
                .Select(c => new
                {
                    c.Name,
                    Trucks = c.ClientsTrucks.Select(ct => ct.Truck).Where(t => t.TankCapacity >= capacity).Select(t => new
                    {
                        TruckRegistrationNumber = t.RegistrationNumber,
                        t.VinNumber,
                        t.TankCapacity,
                        t.CargoCapacity,
                        CategoryType = t.CategoryType.ToString(),
                        MakeType = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.MakeType)
                    .ThenByDescending(t => t.CargoCapacity)
                    .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10);

            return JsonConvert.SerializeObject(clients,Formatting.Indented);
        }
    }
}
