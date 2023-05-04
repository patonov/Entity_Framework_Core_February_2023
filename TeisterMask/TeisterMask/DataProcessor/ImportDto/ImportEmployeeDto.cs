using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeisterMask.Data.Models;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(3)]
        [RegularExpression(@"^[A-Za-z0-9]{3,}$")]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"[0-9]{3}\-[0-9]{3}\-[0-9]{4}")]
        public string Phone { get; set; } = null!;

        public int[] Tasks { get; set; }
    }
}
