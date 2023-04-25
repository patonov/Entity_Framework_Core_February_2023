namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Numerics;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {

            string[] genres = new string[] { "Drama", "Comedy", "Romance", "Musical" };
            var minTimeSpan = new TimeSpan(1, 0, 0);

            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Plays");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPlayDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportPlayDto[] importPlayDtos = (ImportPlayDto[])xmlSerializer.Deserialize(reader);

            ICollection<Play> plays = new HashSet<Play>();

            foreach (ImportPlayDto playDto in importPlayDtos)
            {
                var now = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture);

                if (IsValid(playDto) == false || (genres.Contains(playDto.Genre)) == false || now < minTimeSpan)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playDto.Title,
                    Duration = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = playDto.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };
                plays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Casts");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCastDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportCastDto[] importCastDtos = (ImportCastDto[])xmlSerializer.Deserialize(reader);

            ICollection<Cast> casts = new HashSet<Cast>();

            foreach (ImportCastDto castDto in importCastDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play? play = context.Plays.FirstOrDefault(p => p.Id == castDto.PlayId);
                if (play == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = bool.Parse(castDto.IsMainCharacter),
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                casts.Add(cast);
                string kindOfRole = castDto.IsMainCharacter == "true" ? "main" : "lesser";
                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, kindOfRole));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTheatreDto[] theatreDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);
            ICollection<Theatre> validTheatres = new HashSet<Theatre>();

            foreach (var theatreDto in theatreDtos)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(theatreDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director
                };

                foreach (ImportTicketDto ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    theatre.Tickets.Add(new Ticket() { Price = ticketDto.Price, RowNumber = ticketDto.RowNumber, PlayId = ticketDto.PlayId });
                }

                validTheatres.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
