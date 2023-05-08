namespace TeisterMask.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TeisterMaskProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportProjectDto[] projectDtos = context.Projects.Where(p => p.Tasks.Any())
                .ProjectTo<ExportProjectDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.Name)
                .ToArray();


            XmlRootAttribute root = new XmlRootAttribute("Projects");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProjectDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, projectDtos, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var yakoBusyEmps = context.Employees.Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date)).ToArray()
                .Select(e => new 
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date).ToArray()
                    .OrderByDescending(et => et.Task.DueDate).ThenBy(et => et.Task.Name)
                    .Select(et => new 
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    }).ToArray()                
                }).ToArray()
                .OrderByDescending(e => e.Tasks.Count()).ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(yakoBusyEmps, Formatting.Indented);
        }
    }
}