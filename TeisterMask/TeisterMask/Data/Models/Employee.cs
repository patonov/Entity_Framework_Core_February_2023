using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TeisterMask.Data.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(3)]
        [RegularExpression(@"^([A-Z]+[0-9]+|[a-z]+[0-9]+|[0-9]+)")]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"[0-9]{3}\-[0-9]{3}\-[0-9]{4}")]
        public string Phone { get; set; } = null!; 

        public ICollection<EmployeeTask> EmployeesTasks { get; set; } = new List<EmployeeTask>();





    }
}
