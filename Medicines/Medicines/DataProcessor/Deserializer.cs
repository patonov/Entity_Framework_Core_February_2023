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
            return "tralala";
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
