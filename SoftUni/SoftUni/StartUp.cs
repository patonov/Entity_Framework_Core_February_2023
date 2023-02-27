using Microsoft.VisualBasic;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            //Console.WriteLine("Connection success!");

            //Console.WriteLine(GetEmployeesFullInformation(context));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            //Console.WriteLine(AddNewAddressToEmployee(context));
            //Console.WriteLine(GetEmployeesInPeriod(context)); --------> It doesn't work.
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context)); --------> It doesn't work.
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProjects(context));

            //Console.WriteLine(IncreaseSalaries(context));

            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
            //Console.WriteLine(DeleteProjectById(context)); --------> It doesn't work.

            Console.WriteLine(RemoveTown(context));
        
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.OrderBy(e => e.EmployeeId).ToArray();

            foreach ( Employee e in employees) 
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.Salary > 50000).OrderBy(e => e.FirstName).ToList();

            foreach (Employee e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(employee => employee.Department.Name == "Research and Development")
                .OrderBy(employee => employee.Salary)
                .ThenByDescending(employee => employee.FirstName);

            foreach (Employee e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from Research and Development - ${e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {            
            var newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee? employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
            
            employee.Address = newAddress;

            context.SaveChanges();

            var employeeAddresses = context.Employees
            .OrderByDescending(employee => employee.Address.AddressId)
            .Take(10)
            .Select(employee => employee.Address.AddressText);

            StringBuilder sb = new StringBuilder();

            foreach (string e in employeeAddresses) 
            {
               sb.AppendLine(e);
            }
            return sb.ToString().TrimEnd();           
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employeesWithProjects = context.Employees.Where(e => e.EmployeesProjects
            .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ProjectStartDate = ep.Project.StartDate,
                        ProjectEndDate = ep.Project.EndDate
                    })
                }).Take(10);

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employeesWithProjects)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.ProjectEndDate.HasValue ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";

                    sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses.OrderByDescending(a => a.Employees.Count).ThenBy(a => a.Town!.Name)
                    .Take(10)
                    .Select(a => new
                    {
                        Text = a.AddressText,
                        Town = a.Town.Name,
                        EmployeesCount = a.Employees.Count
                    }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in addresses) 
            {
                sb.AppendLine($"{a.Text}, {a.Town} - {a.EmployeesCount} employees");          
            }
            return sb.ToString().Trim();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context
                           .Employees
                           .Where(e => e.EmployeeId == 147)
                           .Select(e => new
                           {
                               e.FirstName,
                               e.LastName,
                               e.JobTitle,
                               Projects = e.EmployeesProjects.Select(ep => ep.Project.Name.OrderBy(n => n))
                           }).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine(p.ToString());
                }
            }
            return sb.ToString().Trim();

        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments.Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count).ThenBy(d => d.Name)
                    .Select(d => new
                    {
                        d.Name,
                        managerFirstName = d.Manager.FirstName,
                        ManagerLastName = d.Manager.LastName,
                        employees = d.Employees
                            .Select(e => new
                            {
                                e.FirstName,
                                e.LastName,
                                e.JobTitle
                            }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
                    }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} – {d.managerFirstName} {d.ManagerLastName}");

                foreach (var e in d.employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString().Trim();

        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects.OrderByDescending(p => p.StartDate).Take(10)
                    .Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.StartDate
                    }).OrderBy(p => p.Name).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }
            return sb.ToString().Trim();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                    .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design"
                    || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                    .Select(e => new { 
                    e.FirstName,
                    e.LastName,
                    Salary = e.Salary * 1.12m
                    })
                    .OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in employees) 
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
            }
            return sb.ToString().Trim();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees.Where(e => e.FirstName.StartsWith("Sa"))
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        e.Salary
                    }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
            
            StringBuilder sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }
            return sb.ToString().Trim();

        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var epToDelete = context.EmployeesProject.Where(ep => ep.ProjectId == 2);
            context.EmployeesProject.RemoveRange(epToDelete);

            Project projectToDelete = context.Projects.Find(2)!;
            context.Projects.Remove(projectToDelete);

            context.SaveChanges();

            string[] prNames = context.Projects.Take(10).Select(p => p.Name).ToArray();

            return string.Join(Environment.NewLine, prNames);
        
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context
                    .Towns
                    .First(t => t.Name == "Seattle");

            var addsToDelete =
                context
                    .Addresses
                    .Where(a => a.TownId == town.TownId);

            int addsNumber = addsToDelete.Count();

            var employeesWithAddsDeleted = 
                context
                    .Employees
                    .Where(e => addsToDelete.Any(a => a.AddressId == e.AddressId));

            foreach (var e in employeesWithAddsDeleted) 
            {
                e.AddressId = null;
            }

            foreach (var address in addsToDelete)
            {
                context.Addresses.Remove(address);
            }

            context.Remove(town);

            context.SaveChanges();

            return $"{addsNumber} addresses in Seattle were deleted";

        }


    }
}