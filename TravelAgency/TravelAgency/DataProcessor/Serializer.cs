using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            ExportGuideDto[] guideDtos = context.Guides.Where(g => (int)g.Language == 3)
                .OrderByDescending(g => g.TourPackagesGuides.Count).ThenBy(g => g.FullName)
                .Select(g => new ExportGuideDto 
                { 
                FullName = g.FullName,
                TourPackages = g.TourPackagesGuides.Select(tp => tp.TourPackage)
                    .OrderByDescending(tp => tp.Price)
                    .ThenBy(tp => tp.PackageName)
                    .Select(tp => new ExportTourPacageDto 
                        { 
                        Name = tp.PackageName,
                        Description = tp.Description,
                        Price = tp.Price
                        }).ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportGuideDto[]), new XmlRootAttribute("Guides"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, guideDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context.Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .OrderBy(c => c.FullName)
                .ThenBy(c => c.Bookings.Count())
                .Select(c => new 
                {
                    c.FullName,
                    c.PhoneNumber,
                    Bookings = c.Bookings.ToArray().Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .Select(b => new
                        {
                            TourPackageName = b.TourPackage.PackageName,
                            Date = b.BookingDate
                        }).OrderBy(b => b.Date)
                }).ToArray();

            var customersDtos = customers
                .Select(c => new ExportCustomerDto
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Select(b => new ExportBookingDto
                        {
                            TourPackageName = b.TourPackageName,
                            Date = b.Date.ToString("yyyy-MM-dd")
                        })
                        .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(customersDtos, Formatting.Indented);
        }
    }
}
