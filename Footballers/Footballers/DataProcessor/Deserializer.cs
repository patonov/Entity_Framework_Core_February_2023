namespace Footballers.DataProcessor
{
    using AutoMapper;
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FootballersProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Coaches");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCoachDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportCoachDto[] importCoachDtos = (ImportCoachDto[])xmlSerializer.Deserialize(reader);

            ICollection<Coach> coaches = new HashSet<Coach>();

            foreach (ImportCoachDto coachDto in importCoachDtos)
            {

                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(coachDto.Name) || string.IsNullOrEmpty(coachDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                Coach coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (ImportFootballerDto footballerDto in coachDto.Footballers)
                {
                    if (!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime startDate;
                    if ((DateTime.TryParseExact(footballerDto.ContractStartDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out startDate) == false))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime endDate;
                    if ((DateTime.TryParseExact(footballerDto.ContractEndDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out endDate) == false))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (startDate >= endDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = startDate,
                        ContractEndDate = endDate,
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType = (PositionType)footballerDto.PositionType
                    };
                                       
                    coach.Footballers.Add(footballer);
                }
                
            coaches.Add(coach);
                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }
            context.Coaches.AddRange(coaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {

            StringBuilder sb = new StringBuilder();

            ImportTemDto[] teamDtos = JsonConvert.DeserializeObject<ImportTemDto[]>(jsonString);
            ICollection<Team> validTeamss = new HashSet<Team>();

            foreach (ImportTemDto team in teamDtos)
            {
                if (!IsValid(team))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                   
                Team validTeam = new Team()
                {
                    Name = team.Name,
                    Nationality = team.Nationality,
                    Trophies = team.Trophies
                };
                
                if (validTeam.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (int footballerId in team.Footballers.Distinct())
                {                  
                    Footballer footballer = context.Footballers.FirstOrDefault(f => f.Id == footballerId);
                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validTeam.TeamsFootballers.Add(new TeamFootballer() { Footballer = footballer });
                }
            validTeamss.Add(validTeam);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, validTeam.Name, validTeam.TeamsFootballers.Count));
            }
            context.Teams.AddRange(validTeamss);
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
