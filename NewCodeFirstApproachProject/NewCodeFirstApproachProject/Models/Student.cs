using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCodeFirstApproachProject.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int ClassId { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; } = null!;

        public int TownId { get; set; }

        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

    }
}
