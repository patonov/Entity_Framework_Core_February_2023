using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
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
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(dbContext));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(dbContext));
            //Console.WriteLine(AddNewAddressToEmployee(dbContext));
            //Console.WriteLine(GetEmployeesInPeriod(dbContext));
            //Console.WriteLine(GetAddressesByTown(dbContext));
            Console.WriteLine(GetEmployee147(dbContext));
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
            var employees = context.Employees.Where(e => e.Department.Name == "Research and Development")
                .Select(e => new 
                { 
                e.FirstName,
                e.LastName,
                DepartmentName = e.Department.Name,
                e.Salary
                }).OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 04
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            if (employee != null) 
            { 
                employee.Address = address;
            }

            var emps = context.Employees
                .Select(e => new { e.FirstName, e.AddressId, e.Address }).OrderByDescending(e => e.AddressId).Take(10).ToArray();

            foreach (var emp in emps)
            { 
            sb.AppendLine(emp.Address.AddressText);
            }

            return sb.ToString().Trim();
        }

        //Problem 05
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001
                                                              && ep.Project.StartDate.Year <= 2003)).Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager.FirstName,
                ManagerLastName = e.Manager.LastName,
                Projects = e.EmployeesProjects.Select(ep => new 
                {
                    ProjectName = ep.Project.Name,
                    StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndDate = ep.Project.EndDate.HasValue ? 
                            ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                }).ToArray()
            }).ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 06
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses.Where(a => a.Employees.Count > 0)
                .Select(a => new 
                { 
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    NumberOfEmployees = a.Employees.Count
                }).OrderByDescending(a => a.NumberOfEmployees)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            foreach (var a in addresses) 
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.NumberOfEmployees} employees");
            }

            return sb.ToString().Trim();
        }

        //Problem 07
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees.Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => new
                    {
                        p.Project.Name
                    }).OrderBy(p => p.Name).ToArray()
                }).Single();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var p in employee.Projects) 
            { 
                sb.AppendLine($"{p.Name}");
            }

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
