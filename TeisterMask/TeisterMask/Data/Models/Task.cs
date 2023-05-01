using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeisterMask.Data.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace TeisterMask.Data.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime OpenDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public virtual ExecutionType ExecutionType { get; set; }

        [Required]
        public virtual LabelType LabelType { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } = null!;

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; } = new List<EmployeeTask>();

    }
}
