namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Districts");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportDistrictDto[]), root);

            StringReader reader = new StringReader(xmlDocument);

            ImportDistrictDto[] importDistrictDtos = (ImportDistrictDto[])xmlSerializer.Deserialize(reader);

            ICollection<District> districts = new List<District>();

            foreach (var districtDto in importDistrictDtos)
            {
                
                if (!IsValid(districtDto))
                { 
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dbContext.Districts.Any(d => d.Name == districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District d = new District()
                {
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                    Region = (Region)Enum.Parse(typeof(Region), districtDto.Region)
                };

                foreach (var propertyDto in districtDto.Properties)
                {
                    if (!IsValid(propertyDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime acquisitionDate = DateTime.ParseExact(propertyDto.DateOfAcquisition, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture, DateTimeStyles.None);

                    if (dbContext.Properties.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier) || d.Properties.Any(dp => dp.PropertyIdentifier == propertyDto.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (dbContext.Properties.Any(p => p.Address == propertyDto.Address) || d.Properties.Any(dp => dp.Address == propertyDto.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property p = new Property() 
                    {
                        PropertyIdentifier = propertyDto.PropertyIdentifier,
                        Area = propertyDto.Area,
                        Details = propertyDto.Details,
                        Address = propertyDto.Address,
                        DateOfAcquisition = acquisitionDate
                    };

                    d.Properties.Add(p);                    
                }
                districts.Add(d);
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, d.Name, d.Properties.Count()));
            }

            dbContext.Districts.AddRange(districts);
            dbContext.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            ImportCitizenDto[] citizenDtos = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);
            ICollection<Citizen> validCitizens = new HashSet<Citizen>();

            foreach (ImportCitizenDto citizenDto in citizenDtos) 
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime birthDate = DateTime.ParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                Citizen c = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = birthDate,
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), citizenDto.MaritalStatus)
                };

                foreach (int propertyId in citizenDto.Properties)
                {
                    PropertyCitizen pc = new PropertyCitizen()
                    {
                        Citizen = c,
                        PropertyId = propertyId
                    };
                    c.PropertiesCitizens.Add(pc);
                }
                validCitizens.Add(c);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen, c.FirstName, c.LastName, c.PropertiesCitizens.Count));
            }
            dbContext.Citizens.AddRange(validCitizens);
            dbContext.SaveChanges();

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
