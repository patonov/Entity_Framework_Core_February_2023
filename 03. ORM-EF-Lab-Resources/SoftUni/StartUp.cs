using Microsoft.Extensions.Configuration;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {        
        static void Main()
        {
            var context = new SoftUniContext();

            //Console.WriteLine(DeleteFirstProject(context));

            Console.WriteLine(UpdateAddresses(context));
        }

        
        //Problem 07
        public static string DeleteFirstProject(SoftUniContext context)
        {
            Project project = context.Projects.FirstOrDefault();

            if (project != null)
            {
                context.Projects.Remove(project);
                context.SaveChanges();
                return project.Name;
            }

            return "";
        }

        //Problem 08
        public static string UpdateAddresses(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Where(a => a.AddressText.Contains("Drive")).ToList();

            foreach (var a in addresses)
            {
                a.TownId = 2;
            }

            context.SaveChanges();

            return addresses.Count.ToString();
        }
    }
}
