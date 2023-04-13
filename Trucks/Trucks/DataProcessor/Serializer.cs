namespace Trucks.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TrucksProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportDespatcherDto[] despatcherDtos = context.Despatchers.Where(d => d.Trucks.Any())
                .ProjectTo<ExportDespatcherDto>(mapper.ConfigurationProvider)
                .OrderByDescending(d => d.TrucksCount).ThenBy(d => d.DespatcherName).ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Despatchers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportDespatcherDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, despatcherDtos, namespaces);

            return sb.ToString().TrimEnd();



        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients.Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity)).ToArray()
                .Select(c => new
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks.Where(ct => ct.Truck.TankCapacity >= capacity).ToArray()
                .OrderBy(ct => ct.Truck.MakeType).ThenByDescending(ct => ct.Truck.CargoCapacity)
                .Select(ct => new
                {
                    TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                    VinNumber = ct.Truck.VinNumber,
                    TankCapacity = ct.Truck.TankCapacity,
                    CargoCapacity = ct.Truck.CargoCapacity,
                    CategoryType = ct.Truck.CategoryType.ToString(),
                    MakeType = ct.Truck.MakeType.ToString()
                }).ToArray()
                }).ToArray()
                .OrderByDescending(c => c.Trucks.Count()).ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }
    }
}
