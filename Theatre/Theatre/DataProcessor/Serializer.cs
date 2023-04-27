namespace Theatre.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatresToExport = context.Theatres.Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20).ToArray()
                .OrderByDescending(t => t.NumberOfHalls).ThenBy(t => t.Name)
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets.Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5).Sum(s => s.Price),
                    Tickets = t.Tickets.Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5).ToArray().OrderByDescending(ti => ti.Price)
                    .Select(ti => new
                    {
                        Price = ti.Price,
                        RowNumber = ti.RowNumber
                    })
                }).ToArray();
            
            return JsonConvert.SerializeObject(theatresToExport, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TheatreProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportPlayDto[] playDtos = context.Plays.Where(p => (double)p.Rating <= raiting)
               .ProjectTo<ExportPlayDto>(mapper.ConfigurationProvider)
               .OrderBy(p => p.Title)
               .ThenByDescending(p => p.Genre)
               .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPlayDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, playDtos, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
