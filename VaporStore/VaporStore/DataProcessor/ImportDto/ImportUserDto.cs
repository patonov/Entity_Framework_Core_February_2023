using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    
    public class ImportUserDto
    {
        [Required]
        [RegularExpression("^([A-Z]{1}[a-z]+)\\s([A-Z]{1}[a-z]+)$")]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Range(3, 103)]
        public int Age { get; set; }

        [Required]
        public ImportCardDto[] Cards { get; set; }
    }
}
