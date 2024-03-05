namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Diagnostics;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            throw new NotImplementedException();
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
