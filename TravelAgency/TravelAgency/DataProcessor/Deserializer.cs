using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using TravelAgency.Data;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Globalization;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Customers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportCustomerDto[] importCustomerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(reader);

            ICollection<Customer> customers = new List<Customer>();

            foreach (var customerDto in importCustomerDtos)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Customers.Any(c => c.FullName == customerDto.FullName) == true ||
                    context.Customers.Any(c => c.PhoneNumber == customerDto.PhoneNumber) == true ||
                    context.Customers.Any(c => c.Email == customerDto.Email) == true)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer customer = new Customer()
                {
                    FullName = customerDto.FullName,
                    PhoneNumber = customerDto.PhoneNumber,
                    Email = customerDto.Email,
                };

                if(customers.Any(c => c.FullName == customer.FullName) ||
                    customers.Any(c => c.PhoneNumber == customer.PhoneNumber) ||
                    customers.Any(c => c.Email == customer.Email))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                customers.Add(customer);
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customer.FullName));
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportBookingDto[] bookingDtos = JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString);
            ICollection<Booking> validBookings = new HashSet<Booking>();

            foreach (ImportBookingDto bookingDto in bookingDtos)
            {
                if (!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime bookingDate;
                if (DateTime.TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out bookingDate) == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Customer customer = context.Customers.FirstOrDefault(c => c.FullName == bookingDto.CustomerName);
                if (customer == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TourPackage package = context.TourPackages.FirstOrDefault(p => p.PackageName == bookingDto.TourPackageName);
                if (package == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Booking booking = new Booking()
                {
                    BookingDate = bookingDate,
                    CustomerId = customer.Id,
                    Customer = customer,
                    TourPackageId = package.Id,
                    TourPackage = package,
                };

                validBookings.Add(booking);
                sb.AppendLine(string.Format(SuccessfullyImportedBooking, booking.TourPackage.PackageName, bookingDate.ToString("yyyy-MM-dd")));
            }
            context.Bookings.AddRange(validBookings);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
