namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Countries");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCountryDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportCountryDto[] importCountryDtos = (ImportCountryDto[])xmlSerializer.Deserialize(reader);

            ICollection<Country> countries = new HashSet<Country>();

            foreach (ImportCountryDto countryDto in importCountryDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(countryDto.CountryName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };

                countries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Manufacturers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportManufacturerDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportManufacturerDto[] importManufacturerDtos = (ImportManufacturerDto[])xmlSerializer.Deserialize(reader);

            ICollection<Manufacturer> manufacturers = new HashSet<Manufacturer>();

            foreach (var manufacturerDto in importManufacturerDtos)
            {
                if (!IsValid(manufacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(manufacturerDto.ManufacturerName)
                    || manufacturers.Any(m => m.ManufacturerName == manufacturerDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };

                manufacturers.Add(manufacturer);

                var details = manufacturer.Founded.Split(", ").ToArray();
                var townAndCountry = details.Skip(Math.Max(0, details.Count() - 2)).ToArray();
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, string.Join(", ", townAndCountry)));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Shells");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportShellDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportShellDto[] importShellDtos = (ImportShellDto[])xmlSerializer.Deserialize(reader);

            ICollection<Shell> shells = new HashSet<Shell>();

            foreach (ImportShellDto shellDto in importShellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };

                shells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var gunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            ICollection<Gun> validGuns = new HashSet<Gun>();

            string[] validTypes = new string[] { "Howitzer", "Mortar", "FieldGun", "AntiAircraftGun", "MountainGun", "AntiTankGun" };

            foreach (ImportGunDto gunDto in gunDtos)
            {

                if (!IsValid(gunDto) || !validTypes.Any(x => x == gunDto.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), gunDto.GunType),
                    ShellId = gunDto.ShellId,
                };

                foreach (var importCountriesGuns in gunDto.Countries)
                {
                    Country country = context.Countries.FirstOrDefault(c => c.Id == importCountriesGuns.Id);

                    if (country == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    gun.CountriesGuns.Add(new CountryGun() { CountryId = importCountriesGuns.Id , Country = country, Gun = gun , GunId = gun.Id });
                }
                validGuns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}