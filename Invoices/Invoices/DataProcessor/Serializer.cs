namespace Invoices.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.AddProfile<InvoicesProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportClientDto[] clientDtos = context.Clients.Where(c => c.Invoices.Any())
                .ProjectTo<ExportClientDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.InvoicesCount).ThenBy(c => c.Name).ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Clients");

            XmlSerializer serializer = new XmlSerializer(typeof(ExportClientDto[]), root);

            XmlSerializerNamespaces serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add(string.Empty, string.Empty);

            using StringWriter sw = new StringWriter(sb);

            serializer.Serialize(sw, clientDtos, serializerNamespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products.Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength)).ToArray()
               .Select(p => new {
                   p.Name,
                   p.Price,
                   Category = p.CategoryType.ToString(),
                   Clients = p.ProductsClients
                       .Where(pc => pc.Client.Name.Length >= nameLength)
                       .ToArray()
                       .OrderBy(pc => pc.Client.Name)
                       .Select(pc => new
                       {
                           pc.Client.Name,
                           pc.Client.NumberVat,
                       }).ToArray()
               })
               .OrderByDescending(p => p.Clients.Length)
               .ThenBy(p => p.Name)
               .Take(5)
               .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);

        }
    }
}