using Castle.Core;
using Castle.Core.Resource;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Households");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportHouseholdDto[]), root);

            StringReader reader = new StringReader(xmlString);

            ImportHouseholdDto[] importHouseholdDtos = (ImportHouseholdDto[])xmlSerializer.Deserialize(reader);

            ICollection<Household> households = new List<Household>();

            foreach (var houseDto in importHouseholdDtos)
            {
                if (!IsValid(houseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Households.Any(h => h.ContactPerson == houseDto.ContactPerson) == true ||
                    context.Households.Any(h => h.Email == houseDto.Email) == true ||
                    context.Households.Any(h => h.PhoneNumber == houseDto.PhoneNumber) == true)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Household household = new Household()
                {
                    ContactPerson = houseDto.ContactPerson,
                    Email = houseDto.Email,
                    PhoneNumber = houseDto.PhoneNumber,
                };

                if (households.Any(h => h.ContactPerson == household.ContactPerson) == true ||
                    households.Any(h => h.Email == household.Email) == true ||
                    households.Any(h => h.PhoneNumber == household.PhoneNumber) == true)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                households.Add(household);
                sb.AppendLine(string.Format(SuccessfullyImportedHousehold, household.ContactPerson));
            }
            context.Households.AddRange(households);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportExpenseDto[] expenseDtos = JsonConvert.DeserializeObject<ImportExpenseDto[]>(jsonString);
            ICollection<Expense> validExpenses = new HashSet<Expense>();

            foreach (ImportExpenseDto expenseDto in expenseDtos)
            {
                if (!IsValid(expenseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Household household = context.Households.FirstOrDefault(h => h.Id == expenseDto.HouseholdId);
                if (household == null) 
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Service service = context.Services.FirstOrDefault(s => s.Id == expenseDto.ServiceId);
                if (service == null)
                {
                   sb.AppendLine(ErrorMessage);
                   continue;
                }

                DateTime expenseDueDate;
                if (DateTime.TryParseExact(expenseDto.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out expenseDueDate) == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string[] status = { "Paid", "Unpaid", "Overdue", "Expired" };
                if (!status.Contains(expenseDto.PaymentStatus))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                                
                Expense e = new Expense()
                {
                    ExpenseName = expenseDto.ExpenseName,
                    Amount = expenseDto.Amount,
                    DueDate = expenseDueDate,
                    PaymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), expenseDto.PaymentStatus),
                    HouseholdId = expenseDto.HouseholdId,
                    ServiceId = expenseDto.ServiceId,
                };

                validExpenses.Add(e);
                sb.AppendLine(string.Format(SuccessfullyImportedExpense, e.ExpenseName, e.Amount.ToString("F2")));
            }

            context.Expenses.AddRange(validExpenses);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach(var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
