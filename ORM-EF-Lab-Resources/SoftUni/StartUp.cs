using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;

namespace SoftUni
{
    public class StartUp
    {
        static void Main()
        {
            var context = new SoftUniContext();
            //var result = FindEmployeesWithJobTitle(context);
            //var result = FindProjectWithId(context);
            //CreateNewProject(context);
            var result = UpdateFirstEmployee(context);

            Console.WriteLine(result);
        }

        //Problem 03
        public static string FindEmployeesWithJobTitle(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.JobTitle == "Design Engineer")
                .Select(e => e.FirstName)
                .ToList();

            return string.Join(Environment.NewLine, employees);
        }

        //Problem 04
        public static string FindProjectWithId(SoftUniContext context)
        {
            var project = context.Projects.Find(2);
            return project.Name;
        }

        //Problem 05
        public static void CreateNewProject(SoftUniContext context)
        {
            Project project = new Project()
            {
                Name = "Full Brainless Work",
                StartDate = new DateTime(2023, 12, 1)
            };

            context.Projects.Add(project);
            context.SaveChanges();
        }

        //Problem 06
        public static string UpdateFirstEmployee(SoftUniContext context)
        {
            var employee = context.Employees.FirstOrDefault();

            if (employee != null)
            {
                employee.FirstName = "Alex";
                context.SaveChanges();

                return employee.FirstName;
            }
            return "";
        }

           
    }
 }

