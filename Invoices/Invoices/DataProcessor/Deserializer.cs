﻿namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection.PortableExecutable;
    using System.Text;
    using System.Xml.Serialization;
    using Invoices.Data;
    using DataProcessor.ImportDto;
    using System.Diagnostics.Metrics;
    using Invoices.Data.Models;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Clients");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportClientDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportClientDto[] importClientDtos = (ImportClientDto[])xmlSerializer.Deserialize(reader);
            
            ICollection<Client> clients = new List<Client>();

            foreach (ImportClientDto clientDto in importClientDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client c = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat
                };

                foreach (AddressImportDto addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = new Address()
                    {
                    StreetName = addressDto.StreetName,
                    StreetNumber = addressDto.StreetNumber,
                    PostCode = addressDto.PostCode,
                    City = addressDto.City,
                    Country = addressDto.Country
                    };

                    c.Addresses.Add(address);
                }
                clients.Add(c);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, c.Name));
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {


            throw new NotImplementedException();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
