namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var rootName = "Despatchers";
            var xmlHelper = new XmlHelper();

            var despatcherDtos = xmlHelper.Deserialize<ImportDespatherDTO[]>(xmlString, rootName);

            var despathers = new HashSet<Despatcher>();

            foreach (var despatcherDto in despatcherDtos!)
            {
                if (!IsValid(despatcherDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (string.IsNullOrEmpty(despatcherDto.Name)
                    || string.IsNullOrEmpty(despatcherDto.Position)
                    || despatcherDto.Trucks.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var trucks = new HashSet<Truck>();
                foreach (var truckDTO in despatcherDto.Trucks)
                {
                    if (!IsValid(truckDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    trucks.Add(new Truck(truckDTO));
                }

                despathers.Add(new Despatcher(despatcherDto, trucks.ToArray()));
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcherDto.Name, trucks.Count()));
            }

            context.Despatchers.AddRange(despathers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var clientsDTOs = JsonConvert.DeserializeObject<ImportClientDTO[]>(jsonString);

            var clients = new HashSet<Client>();
            var dbTrucks = context.Trucks.Select(t => t.Id).ToArray();
            foreach (var clientDTO in clientsDTOs!)
            {
                if (!IsValid(clientDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (clientDTO.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var trucks = new List<int>();
                foreach (var truckId in clientDTO.TrucksIds.Distinct())
                {
                    if (!dbTrucks.Any(t => t == truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    trucks.Add(truckId);
                }
                clientDTO.TrucksIds = trucks.ToArray();

                clients.Add(new Client(clientDTO));

                sb.AppendLine(string.Format(SuccessfullyImportedClient,clientDTO.Name,trucks.Count()));
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}