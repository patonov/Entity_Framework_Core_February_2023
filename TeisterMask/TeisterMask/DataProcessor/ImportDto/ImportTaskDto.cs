using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeisterMask.Data.Models.Enums;
using TeisterMask.Data.Models;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportTaskDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; } = null!;

        [Required]
        [XmlElement("DueDate")]
        public string DueDate { get; set; } = null!;

        [Required]
        [Range(0, 3)]
        [XmlElement("ExecutionType")]
        public int ExecutionType { get; set; }

        [Required]
        [Range(0, 4)]
        [XmlElement("LabelType")]
        public int LabelType { get; set; }
               
    }
}
