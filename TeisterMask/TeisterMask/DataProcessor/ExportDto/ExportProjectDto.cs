using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjectDto 
    {
        [XmlElement("ProjectName")]
        public string Name { get; set; } = null!;

        [XmlElement("HasEndDate")]
        public string DueDate { get; set; } = null!;

        [XmlAttribute("TasksCount")]
        public int Count { get; set; }

        [XmlArray("Tasks")]
        public ExportTaskDto[] Tasks { get; set; }
    }
}
