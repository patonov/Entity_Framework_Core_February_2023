using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCodeFirstApproachProject.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        public string ClassNumber { get; set; } = null!;

        public string Description { get; set; } = null!;

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
