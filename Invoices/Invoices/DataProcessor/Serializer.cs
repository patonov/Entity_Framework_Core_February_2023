namespace Invoices.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
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

            throw new NotImplementedException();
        }
    }
}