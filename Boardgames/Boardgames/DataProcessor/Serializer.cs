namespace Boardgames.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Boardgames.Data;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BoardgamesProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportCreatorDto[] creatorDtos = context.Creators.Where(c => c.Boardgames.Any())
                .ProjectTo<ExportCreatorDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.Name)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Creators");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCreatorDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, creatorDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellersToExport = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(bs => bs.Boardgame.YearPublished >= year)
                && s.BoardgamesSellers.Any(bs => bs.Boardgame.Rating <= rating)).ToArray()
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                .Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating).ToArray()
                .OrderByDescending(bs => bs.Boardgame.Rating).ThenBy(bs => bs.Boardgame.Name)
                .Select(bs => new
                {
                    Name = bs.Boardgame.Name,
                    Rating = bs.Boardgame.Rating,
                    Mechanics = bs.Boardgame.Mechanics,
                    Category = bs.Boardgame.CategoryType.ToString()
                }).ToArray()
                }).ToArray()
                .OrderByDescending(s => s.Boardgames.Count()).ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellersToExport, Formatting.Indented);
        }
    }
}