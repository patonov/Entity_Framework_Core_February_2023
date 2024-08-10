using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.DataProcessor.ExportDtos;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Serialization;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {
           
            ExportHouseholdDto[] exportHouseholdDtos = context.Households.ToArray()
                .Where(h => h.Expenses.Any(e => e.PaymentStatus.ToString() == "Unpaid")).
                OrderBy(h => h.ContactPerson)
                .Select(h => new ExportHouseholdDto 
                { 
                ContactPerson = h.ContactPerson,
                Email = h.Email,
                PhoneNumber = h.PhoneNumber,
                    Expenses = h.Expenses.Where(e => e.PaymentStatus.ToString() != "Paid")
                    .Select(e => new ExportExpenseDto 
                    { 
                    ExpenseName = e.ExpenseName,
                    Amount = e.Amount.ToString("F2"),
                    PaymentDate = e.DueDate.ToString("yyyy-MM-dd"),
                    ServiceName = e.Service.ServiceName
                    }).OrderBy(e => e.PaymentDate).ThenBy(e => e.Amount)
                    .ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportHouseholdDto[]), new XmlRootAttribute("Households"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, exportHouseholdDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportAllServicesWithSuppliers(NetPayContext context)
        {
            return "tralala";
        }
    }
}
