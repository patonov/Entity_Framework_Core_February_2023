namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPatientDto[]), new XmlRootAttribute("Patients"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter stringWriter = new StringWriter(sb);

            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var patients = context.Patients.ToArray()
                .Where(p => p.PatientsMedicines.Any(p => p.Medicine.ProductionDate >= dateTime))
                .Select(p => new ExportPatientDto() 
                { 
                Name = p.FullName,
                Gender = p.Gender.ToString(),
                AgeGroup = p.AgeGroup.ToString(),
                Medicines = context.Medicines.ToArray()
                .Where(m => m.ProductionDate >= dateTime).OrderByDescending(m => m.ExpiryDate).ThenBy(m => m.Price)
                .Select(m => new ExportMedicinesDto() 
                    { 
                    Name = m.Name,
                    Price = m.Price.ToString("F2"),
                    Producer = m.Producer,
                    BestBefore = m.ExpiryDate.ToString("yyyy-MM-dd"),
                    Category = m.Category.ToString()
                    }).ToArray()
                }).OrderByDescending(p => p.Medicines.Length)
                .ThenBy(p => p.Name)
            .ToArray();

            xmlSerializer.Serialize(stringWriter, patients, namespaces);

            return sb.ToString().Trim();


        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines.AsNoTracking()
                .Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new 
                { 
                Name = m.Name,
                Price = m.Price.ToString("F2"),
                Pharmacy = new 
                    {
                    Name = m.Pharmacy.Name,
                    PhoneNumber = m.Pharmacy.PhoneNumber
                    }
                }).ToArray();

            return JsonConvert.SerializeObject(medicines, Formatting.Indented);
        }
    }
}
