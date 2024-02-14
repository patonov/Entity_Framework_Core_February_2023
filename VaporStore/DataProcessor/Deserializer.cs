namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGameDto[] gameDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);
            ICollection<Game> validGames = new HashSet<Game>();

            ICollection<Developer> developers = new HashSet<Developer>();
            ICollection<Genre> genres = new HashSet<Genre>();
            ICollection<Tag> tags = new HashSet<Tag>();

            foreach (ImportGameDto gameDto in gameDtos)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime releaseDate;

                bool IsReleaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                if (!IsReleaseDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (gameDto.Tags.Count() == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game g = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = releaseDate,
                };

                Developer developer = developers.FirstOrDefault(d => d.Name == gameDto.Developer);

                if (developer == null) 
                {
                    Developer d = new Developer()
                    {
                        Name = gameDto.Developer
                    };

                    developers.Add(d);
                    g.Developer = d;
                }
                else 
                { 
                    g.Developer = developer;
                }

                Genre genre = genres.FirstOrDefault(g => g.Name == gameDto.Genre);

                if (genre == null)
                {
                    Genre genreNew = new Genre()
                    {
                        Name = gameDto.Genre
                    };
                    genres.Add(genreNew);
                    g.Genre = genreNew;
                }
                else
                { 
                    g.Genre = genre;
                }

                foreach (string tName in gameDto.Tags)
                {
                    if (String.IsNullOrEmpty(tName))
                    {
                        continue;
                    }

                    Tag gameTag = tags.FirstOrDefault(t => t.Name == tName);

                    if (gameTag == null) 
                    {
                        Tag newTag = new Tag()
                        {
                            Name = tName
                        };
                        tags.Add(newTag);

                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = newTag
                        });
                    }
                    else 
                    {
                        g.GameTags.Add(new GameTag()
                        {
                            Game = g,
                            Tag = gameTag
                        });
                    }
                }

                if (g.GameTags.Count() == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                validGames.Add(g);
                sb.AppendLine(String.Format(SuccessfullyImportedGame, g.Name, g.Genre.Name, g.GameTags.Count));
            }
            context.Games.AddRange(validGames);
            context.SaveChanges();
            return sb.ToString().Trim();
        }


        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}