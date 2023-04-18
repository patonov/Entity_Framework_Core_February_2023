
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells.Where(sh => sh.ShellWeight > shellWeight).ToArray()
                .Select(sh => new
                {
                    ShellWeight = sh.ShellWeight,
                    Caliber = sh.Caliber,
                    Guns = sh.Guns
                    .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    }).ToArray().OrderByDescending(g => g.GunWeight)
                }).ToArray().OrderBy(sh => sh.ShellWeight);

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }


        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ArtilleryProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportGunDto[] gunDtos = context.Guns.Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .ProjectTo<ExportGunDto>(mapper.ConfigurationProvider)
                .OrderBy(g => g.BarrelLength)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Guns");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportGunDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, gunDtos, namespaces);

            return sb.ToString().TrimEnd();

        }
    }
}
