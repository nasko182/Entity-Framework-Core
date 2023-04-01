namespace Footballers.DataProcessor;

using Footballers.Data;
using Footballers.Data.Models;
using Footballers.DataProcessor.ImportDto;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Utilities;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedCoach
        = "Successfully imported coach - {0} with {1} footballers.";

    private const string SuccessfullyImportedTeam
        = "Successfully imported team - {0} with {1} footballers.";

    public static string ImportCoaches(FootballersContext context, string xmlString)
    {
        var sb = new StringBuilder();
        var xmlHelper = new XmlHelper();

        var coachDtos = xmlHelper.Deserialize<ImportCoachDTO[]>(xmlString, "Coaches");

        var coaches = new HashSet<Coach>();

        foreach(var coachDTO in coachDtos ) 
        {
            if (!IsValid(coachDTO))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            if (string.IsNullOrEmpty(coachDTO.Nationality))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            var footballers = new HashSet<Footballer>();
            foreach (var footballerDTO in coachDTO.Footballers!)
            {
                if (!IsValid(footballerDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime startDate;
                DateTime endDate;
                if (!DateTime.TryParseExact(footballerDTO.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None, out startDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!DateTime.TryParseExact(footballerDTO.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (endDate<startDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                footballers.Add(new Footballer(footballerDTO,startDate,endDate));
            }

            coaches.Add(new Coach(coachDTO,footballers.ToArray()));
            sb.AppendLine(string.Format(SuccessfullyImportedCoach,coachDTO.Name, footballers.Count()));
        }

        context.Coaches.AddRange(coaches);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportTeams(FootballersContext context, string jsonString)
    {
        var sb = new StringBuilder();
        var teamDTOs = JsonConvert.DeserializeObject<ImportTeamDTO[]>(jsonString);

        var teams = new HashSet<Team>();
        var footballersIds = context.Footballers.Select(f => f.Id).ToArray();
        foreach (var teamDTO in teamDTOs!)
        {
            if (!IsValid(teamDTO))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            if(string.IsNullOrEmpty(teamDTO.Nationality)||
                teamDTO.Trophies == 0)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            var teamFootballers = new HashSet<TeamFootballer>();
            foreach (var footbalerId in teamDTO.Footballers.Distinct())
            {
                if (!footballersIds.Any(fId=>fId == footbalerId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                teamFootballers.Add(new TeamFootballer()
                {
                    FootballerId = footbalerId
                });
            }
            teams.Add(new Team(teamDTO, teamFootballers.ToArray()));
            sb.AppendLine(string.Format(SuccessfullyImportedTeam, teamDTO.Name, teamFootballers.Count()));
        }

        context.Teams.AddRange(teams);
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
