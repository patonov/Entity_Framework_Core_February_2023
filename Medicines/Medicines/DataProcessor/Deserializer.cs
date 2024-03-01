namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;
    using System.Text;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPatientsDto[] patientDtos = JsonConvert.DeserializeObject<ImportPatientsDto[]>(jsonString);
            ICollection<Patient> validPatients = new HashSet<Patient>();
            
            foreach (ImportPatientsDto patientDto in patientDtos)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient p = new Patient()
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender,
                };

                foreach (int medicine in patientDto.Medicines)
                {
                    if (p.PatientsMedicines.Any(x => x.MedicineId == medicine))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine patientMedicine = new PatientMedicine()
                    {
                        Patient = p,
                        MedicineId = medicine
                    };

                    p.PatientsMedicines.Add(patientMedicine);
                }
                validPatients.Add(p);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, p.FullName, p.PatientsMedicines.Count));
            }
            context.Patients.AddRange(validPatients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportPharmacyDto[] importPharmacyDtos = xmlHelper.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            ICollection<Pharmacy> pharmacies = new List<Pharmacy>();

            foreach (ImportPharmacyDto pharmacyDto in importPharmacyDtos)
            {
                if (!IsValid(pharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy ph = new Pharmacy()
                {
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                    IsNonStop = bool.Parse(pharmacyDto.IsNonStop)
                };

                foreach (ImportMedicineDto medicineDto in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicineDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (medicineDto.Price < 0.01M || medicineDto.Price > 1000.00M)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(medicineDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime productionDate;
                    bool IsProdDateValid = DateTime.TryParseExact(medicineDto.ProductionDate, "yyyy-MM-dd", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out productionDate);

                    if (!IsProdDateValid)
                    {
                        sb.Append(ErrorMessage);
                        continue;
                    }

                    DateTime expDate; 
                    bool IsExpDateValid = DateTime.TryParseExact(medicineDto.ExpiryDate, "yyyy-MM-dd", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out expDate);

                    if (!IsExpDateValid)
                    {
                        sb.Append(ErrorMessage);
                        continue;
                    }

                    if (expDate <= productionDate) 
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (ph.Medicines.Any(x => x.Name == medicineDto.Name && x.Producer == medicineDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medi = new Medicine()
                    {
                        Name = medicineDto.Name,
                        Price = medicineDto.Price,
                        Category = (Category)Enum.Parse(typeof(Category), medicineDto.Category),
                        ProductionDate = productionDate,
                        ExpiryDate = expDate,
                        Producer = medicineDto.Producer
                    };
                    ph.Medicines.Add(medi);
                }
            
                pharmacies.Add(ph);
            
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, ph.Name, ph.Medicines.Count));
            }
            
            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
