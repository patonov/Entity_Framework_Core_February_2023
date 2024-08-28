using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Serialization;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {
           
            var exportHouseholds = context.Households.ToArray()
                .Where(h => h.Expenses.Any(e => e.PaymentStatus != PaymentStatus.Paid))
                .OrderBy(h => h.ContactPerson)
                .Select(h => new  
                { 
                ContactPerson = h.ContactPerson,
                Email = h.Email,
                PhoneNumber = h.PhoneNumber,
                    Expenses = h.Expenses.Where(e => e.PaymentStatus != PaymentStatus.Paid)
                    .Select(e => new  
                    { 
                    ExpenseName = e.ExpenseName,
                    Amount = e.Amount,
                    PaymentDate = e.DueDate,
                    ServiceName = e.Service.ServiceName
                    }).OrderBy(e => e.PaymentDate)
                }).ToArray();

            var exportHouseholdDtos = exportHouseholds.Select(h => new ExportHouseholdDto
            {
                ContactPerson = h.ContactPerson,
                Email = h.Email,
                PhoneNumber = h.PhoneNumber,
                Expenses = h.Expenses.Select(e => new ExportExpenseDto
                {
                    ExpenseName = e.ExpenseName,
                    Amount = e.Amount.ToString("F2"),
                    PaymentDate = e.PaymentDate.ToString("yyyy-MM-dd"),
                    ServiceName = e.ServiceName
                })
                .OrderBy(e => e.PaymentDate)
                .ThenBy(e => e.Amount)
                .ToList()
            })
            .OrderBy(h => h.ContactPerson)
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
            var services = context.Services
                .Select(s => new
                {
                    s.ServiceName,
                    Suppliers = s.SuppliersServices
                    .Select(ss => new
                    {
                        ss.Supplier.SupplierName
                    })
                    .OrderBy(s => s.SupplierName)
                    .ToArray()
                })
                .OrderBy(s => s.ServiceName)
                .ToArray();

            return JsonConvert.SerializeObject(services, Formatting.Indented);
        }
    }
}
