using SoftUni.Data;
using System;
using System.Linq;
using System.Text;
namespace SoftUni
{
    public class StartUp
    {
        static void Main()
        {
            var dbContext = new SoftUniContext();
            
            //Console.WriteLine(GetEmployeesFullInformation(dbContext));
            Console.WriteLine(GetEmployeesWithSalaryOver50000(dbContext));
        }

        //Problem 01
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new 
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                }).OrderBy(e => e.EmployeeId).ToList();

            foreach (var employee in employees) 
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 02
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 03
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 04
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 05
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 06
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 07
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 08
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 09
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 10
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 11
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Problem 12
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString().Trim();
        }

        //Probem 13
        public static string RemoveTown(SoftUniContext context)
        {
            return null;
            //return $"{addressesCount} addresses in Seattle were deleted";
        }
    }
}
